
using System;
using System.Collections.Generic;
using System.Reflection;
using DhafinFawwaz.ActionExtension;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomPropertyDrawer(typeof(StaticActionBase), true)]
public class StaticActionDrawer : PropertyDrawer {

    
    private AdvancedDropdownState _dropdownState = new();
    private Dictionary<Type, List<FieldInfo>> _cachedActions;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        var actionPathString = property.FindPropertyRelative("_actionPathString");
        var displayActionPathString = property.FindPropertyRelative("_displayActionPathString");
        var actionPathStringRect = new Rect(position.x, position.y, position.width, position.height);

        Type propertyType = property.GetUnderlyingField().FieldType;
        Type[] genericTypes = null; if (propertyType.IsGenericType) genericTypes = propertyType.GetGenericArguments();

        if (_cachedActions == null) {
            if(genericTypes != null && genericTypes.Length > 0) {
                _cachedActions = GetAllStaticActionFields(genericTypes);
            }
            else {
                _cachedActions = GetAllStaticActionFields();
            }
        }

        EditorGUI.BeginProperty(position, label, property);

        string fieldName = actionPathString.stringValue;
        string fieldDisplayName = displayActionPathString.stringValue;
        if (string.IsNullOrEmpty(fieldName)) {
            fieldName = "None";
        }

        if (GUI.Button(position, fieldDisplayName, EditorStyles.popup)) {
            var dropdown = new StaticActionFieldDropdown(_dropdownState, _cachedActions, selectedField => {
                actionPathString.stringValue = $"{selectedField.DeclaringType.FullName}.{selectedField.Name}";
                displayActionPathString.stringValue = StaticActionFieldDropdown.GetConvertedName(selectedField);
                property.serializedObject.ApplyModifiedProperties();
            });

            dropdown.Show(new Rect(Event.current.mousePosition, new Vector2(200, 0)));
        }

        EditorGUI.EndProperty();
    }


    private static Dictionary<Type, List<FieldInfo>> _cachedActionFieldMap = new();

    public static Dictionary<Type, List<FieldInfo>> GetAllStaticActionFields() {
        // if (_cachedActionFieldMap != null) return _cachedActionFieldMap;
        Dictionary<Type, List<FieldInfo>> actionsDict = new();
        var asms = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var asm in asms) {
            foreach (var t in asm.GetTypes()) {
                List<FieldInfo> actions = new();
                if (t.IsClass && t.IsPublic) {
                    foreach (var m in t.GetFields(BindingFlags.Public | BindingFlags.Static)) {
                        Type fieldType = m.FieldType;
                        if (fieldType == typeof(Action) || (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition().FullName.StartsWith("System.Action"))) {
                            Type[] actualGenericType = fieldType.GetGenericArguments();
                            if (actualGenericType.Length > 0) continue;
                            actions.Add(m);
                        }
                    }
                }
                if (actions.Count > 0) {
                    actionsDict[t] = actions;
                }
            }
        }
        _cachedActionFieldMap = actionsDict;
        return actionsDict;
    }

    private static Dictionary<Type[], Dictionary<Type, List<FieldInfo>>> _cachedActionFieldMapFiltered = new();
    public static Dictionary<Type, List<FieldInfo>> GetAllStaticActionFields(Type[] requiredGenericType) {
        // if (_cachedActionFieldMapFiltered != null && _cachedActionFieldMapFiltered.ContainsKey(requiredGenericType)) return _cachedActionFieldMapFiltered[requiredGenericType];
        Dictionary<Type, List<FieldInfo>> actionsDict = new();
        var asms = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var asm in asms) {
            foreach (var t in asm.GetTypes()) {
                List<FieldInfo> actions = new();
                if (t.IsClass && t.IsPublic) {
                    foreach (var m in t.GetFields(BindingFlags.Public | BindingFlags.Static)) {
                        Type fieldType = m.FieldType;

                        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Action<>)) {
                            Type[] actualGenericType = fieldType.GetGenericArguments();
                            if (actualGenericType.Length != requiredGenericType.Length) continue;

                            bool isMatch = true;
                            for (int i = 0; i < requiredGenericType.Length; i++) {
                                if (actualGenericType[i] != requiredGenericType[i]) {
                                    isMatch = false;
                                    break;
                                }
                            }
                            if(isMatch) actions.Add(m);
                        }
                    }
                }
                if (actions.Count > 0) {
                    actionsDict[t] = actions;
                }
            }
        }
        _cachedActionFieldMapFiltered[requiredGenericType] = actionsDict;
        return actionsDict;
    }
}
