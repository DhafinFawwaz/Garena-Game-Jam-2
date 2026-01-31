using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DhafinFawwaz.ActionExtension
{
    public static class SerializedPropertyUtility
    {

        #region Helpers

        public static SerializedProperty FindPropertyRelativeOrFail(this SerializedProperty property, string path)
        {
            var subProperty = property.FindPropertyRelative(path);

            if (subProperty == null)
            {
                throw new InvalidOperationException($"Failed to find property '{SerializedObjectLabel(property.serializedObject)}.{property.propertyPath}.{path}'.");
            }

            return subProperty;
        }

        public static SerializedProperty FindPropertyOrFail(this SerializedObject serializedObject, string path)
        {
            var property = serializedObject.FindProperty(path);

            if (property == null)
            {
                throw new InvalidOperationException($"Failed to find property '{SerializedObjectLabel(serializedObject)}.{path}'.");
            }

            return property;
        }

        public static string FixedPropertyPath(this SerializedProperty property)
        {
            // Unity structures array paths like "fieldName.Array.data[i]".
            // Fix that quirk and directly go to index, i.e. "fieldName[i]".
            return property.propertyPath.Replace(".Array.data[", "[");
        }

        public static string[] PropertyPathParts(this SerializedProperty property)
        {
            return property.FixedPropertyPath().Split('.');
        }

        public static bool IsPropertyIndexer(string propertyPart, out string fieldName, out int index)
        {
            var regex = new Regex(@"(.+)\[(\d+)\]");
            var match = regex.Match(propertyPart);

            if (match.Success) // Property refers to an array or list
            {
                fieldName = match.Groups[1].Value;
                index = int.Parse(match.Groups[2].Value);
                return true;
            }
            else
            {
                fieldName = propertyPart;
                index = -1;
                return false;
            }
        }

        #endregion

        #region Reflection

        private static void EnsureReflectable(SerializedProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.serializedObject.isEditingMultipleObjects)
            {
                throw new NotSupportedException($"Attempting to reflect property '{property.propertyPath}' on multiple objects.");
            }

            if (property.serializedObject.targetObject == null)
            {
                throw new NotSupportedException($"Attempting to reflect property '{property.propertyPath}' on a null object.");
            }
        }

        private static string SerializedObjectLabel(SerializedObject serializedObject)
        {
            return serializedObject.isEditingMultipleObjects ? "[Multiple]" : serializedObject.targetObject.GetType().Name;
        }

        public static object GetUnderlyingValue(this SerializedProperty property)
        {
            EnsureReflectable(property);

            object parent = property.serializedObject.targetObject;
            var parts = PropertyPathParts(property);

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                if (parent == null)
                {
                    throw new NullReferenceException($"Parent of '{SerializedObjectLabel(property.serializedObject)}.{string.Join(".", parts, 0, i + 1)}' is null.");
                }

                parent = GetPropertyPartValue(part, parent);
            }

            return parent;
        }

        public static Type GetUnderlyingType(this SerializedProperty property)
        {
            EnsureReflectable(property);

            var type = property.serializedObject.targetObject.GetType();

            foreach (var part in PropertyPathParts(property))
            {
                type = GetPropertyPartType(part, type);
            }

            return type;
        }

        public static FieldInfo GetUnderlyingField(this SerializedProperty property)
        {
            EnsureReflectable(property);

            var type = property.serializedObject.targetObject.GetType();
            var parts = PropertyPathParts(property);

            for (var i = 0; i < parts.Length - 1; i++)
            {
                type = GetPropertyPartType(parts[i], type);
            }

            string fieldName;
            int index;
            IsPropertyIndexer(parts[parts.Length - 1], out fieldName, out index);

            return GetSerializedFieldInfo(type, fieldName);
        }

        public static void SetUnderlyingValue(this SerializedProperty property, object value)
        {
            EnsureReflectable(property);

            // Serialize so we don't overwrite other modifications with our deserialization later
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();

            object parent = property.serializedObject.targetObject;
            var parts = PropertyPathParts(property);

            for (var i = 0; i < parts.Length - 1; i++)
            {
                var part = parts[i];

                if (parent == null)
                {
                    throw new NullReferenceException($"Parent of '{SerializedObjectLabel(property.serializedObject)}.{string.Join(".", parts, 0, i + 1)}' is null.");
                }

                parent = GetPropertyPartValue(part, parent);
            }

            string fieldName;
            int index;
            IsPropertyIndexer(parts[parts.Length - 1], out fieldName, out index);

            var field = GetSerializedFieldInfo(parent.GetType(), fieldName);

            var previousValue = field.GetValue(parent);
            field.SetValue(parent, value);

            // Since we're using reflection to set the value of the serialized objects directly,
            // we should dirty the object if the value is changed by our action. 
            // Otherwise, any external code has no way of knowing the data was changed and that may break caching. 
            // An example: LocalizedAudioClip property drawer caches parts of TableReference and only updates if dirty.
            if (!previousValue.Equals(value))
            {
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }

            // Deserialize the object for continued operations after this call
            property.serializedObject.Update();
        }

        private static object GetPropertyPartValue(string propertyPathPart, object parent)
        {
            string fieldName;
            int index;

            if (IsPropertyIndexer(propertyPathPart, out fieldName, out index))
            {
                var list = (IList)GetSerializedFieldInfo(parent.GetType(), fieldName).GetValue(parent);

                return list[index];
            }
            else
            {
                return GetSerializedFieldInfo(parent.GetType(), fieldName).GetValue(parent);
            }
        }

        private static Type GetPropertyPartType(string propertyPathPart, Type type)
        {
            string fieldName;
            int index;

            if (IsPropertyIndexer(propertyPathPart, out fieldName, out index))
            {
                var listType = GetSerializedFieldInfo(type, fieldName).FieldType;

                if (listType.IsArray)
                {
                    return listType.GetElementType();
                }
                else // List<T> is the only other Unity-serializable collection
                {
                    return listType.GetGenericArguments()[0];
                }
            }
            else
            {
                return GetSerializedFieldInfo(type, fieldName).FieldType;
            }
        }

        private static FieldInfo GetSerializedFieldInfo(Type type, string name)
        {
            var field = type.GetFieldUnambiguous(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (field == null)
            {
                throw new MissingMemberException(type.FullName, name);
            }

            return field;
        }

        public static FieldInfo GetFieldUnambiguous(this Type type, string name, BindingFlags flags)
        {
            flags |= BindingFlags.DeclaredOnly;

            while (type != null)
            {
                var field = type.GetField(name, flags);

                if (field != null)
                {
                    return field;
                }

                type = type.BaseType;
            }

            return null;
        }

        #endregion
    }
}
