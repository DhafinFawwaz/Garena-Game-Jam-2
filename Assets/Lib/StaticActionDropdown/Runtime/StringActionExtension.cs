using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DhafinFawwaz.ActionExtension {


public static class StringActionExtensions {
    public static void AddListener(this string actionPath, Action callback) {
        ModifyAction(actionPath, callback, subscribe: true);
    }
    public static void AddListener<T>(this string actionPath, Action<T> callback) {
        ModifyAction(actionPath, callback, subscribe: true);
    }
    public static void AddListener<T,U>(this string actionPath, Action<T,U> callback) {
        ModifyAction(actionPath, callback, subscribe: true);
    }
    public static void AddListener<T,U,V>(this string actionPath, Action<T,U,V> callback) {
        ModifyAction(actionPath, callback, subscribe: true);
    }

    public static void RemoveListener(this string actionPath, Action callback) {
        ModifyAction(actionPath, callback, subscribe: false);
    }
    public static void RemoveListener<T>(this string actionPath, Action<T> callback) {
        ModifyAction(actionPath, callback, subscribe: false);
    }
    public static void RemoveListener<T,U>(this string actionPath, Action<T,U> callback) {
        ModifyAction(actionPath, callback, subscribe: false);
    }
    public static void RemoveListener<T,U,V>(this string actionPath, Action<T,U,V> callback) {
        ModifyAction(actionPath, callback, subscribe: false);
    }

    public static void SetListener(this string actionPath, Action callback) {
        RemoveAllListener(actionPath);
        AddListener(actionPath, callback);
    }
    public static void SetListener<T>(this string actionPath, Action<T> callback) {
        RemoveAllListener(actionPath);
        AddListener(actionPath, callback);
    }
    public static void SetListener<T,U>(this string actionPath, Action<T,U> callback) {
        RemoveAllListener(actionPath);
        AddListener(actionPath, callback);
    }
    public static void SetListener<T,U,V>(this string actionPath, Action<T,U,V> callback) {
        RemoveAllListener(actionPath);
        AddListener(actionPath, callback);
    }

    public static void RemoveAllListener(this string actionPath) {
        (FieldInfo, string) p = ParseFieldInfo(actionPath);
        FieldInfo field = p.Item1;
        field.SetValue(null, null);
    }

    private static void ModifyAction(string actionPath, Delegate callback, bool subscribe) {

        (FieldInfo, string) p = ParseFieldInfo(actionPath);
        FieldInfo field = p.Item1;
        string actionName = p.Item2;

        if (!typeof(Delegate).IsAssignableFrom(field.FieldType)) throw new Exception($"Field '{actionName}' is not an Action or delegate type.");

        Delegate currentDelegate = field.GetValue(null) as Delegate;

        if (currentDelegate != null && currentDelegate.GetType() != callback.GetType()) throw new Exception($"Delegate type mismatch: Expected {currentDelegate.GetType()}, but got {callback.GetType()}.");

        if (subscribe) field.SetValue(null, Delegate.Combine(currentDelegate, callback));
        else field.SetValue(null, Delegate.Remove(currentDelegate, callback));
    }


    
    // static Dictionary<string, (FieldInfo, string)> _cachedFieldInfo = new ();
    private static (FieldInfo, string) ParseFieldInfo(string actionPath) {
        // if (_cachedFieldInfo.ContainsKey(actionPath)) return _cachedFieldInfo[actionPath];
        if (string.IsNullOrEmpty(actionPath)) throw new ArgumentException("Action path cannot be null or empty.");


        string[] parts = actionPath.Split('.');
        if (parts.Length != 2) {
            throw new ArgumentException($"Invalid action format: {actionPath}. Expected format: ClassName.ActionName");
        }

        string className = parts[0];
        string actionName = parts[1];

        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == className);

        if (type == null)  throw new Exception($"Class '{className}' not found.");

        FieldInfo field = type.GetField(actionName, BindingFlags.Public | BindingFlags.Static);
        if (field == null) {
            string availableFields = string.Join(", ", type.GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.Name));
            throw new Exception($"Field '{actionName}' not found in class '{className} in type {type}'. Available fields: {availableFields}");
        };
        if (!typeof(Delegate).IsAssignableFrom(field.FieldType)) throw new Exception($"Field '{actionName}' is not an Action or delegate type.");

        var ret = (field, actionName);
        // _cachedFieldInfo[actionPath] = ret;
        return ret;
    }
}

}
