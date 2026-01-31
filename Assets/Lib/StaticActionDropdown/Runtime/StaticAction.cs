using System;
using UnityEngine;

namespace DhafinFawwaz.ActionExtension {


[Serializable]
public class StaticAction : StaticActionBase {

    public static StaticAction operator +(StaticAction staticAction, Action callback) {
        staticAction._actionPathString.AddListener(callback);
        return staticAction;
    }

    public static StaticAction operator -(StaticAction staticAction, Action callback) {
        staticAction._actionPathString.RemoveListener(callback);
        return staticAction;
    }

    public void SetListener(Action callback) {
        _actionPathString.RemoveAllListener();
        _actionPathString.AddListener(callback);
    }

}

[Serializable]
public class StaticAction<T> : StaticActionBase {

    public static StaticAction<T> operator +(StaticAction<T> staticAction, Action<T> callback) {
        staticAction._actionPathString.AddListener(callback);
        return staticAction;
    }

    public static StaticAction<T> operator -(StaticAction<T> staticAction, Action<T> callback) {
        staticAction._actionPathString.RemoveListener(callback);
        return staticAction;
    }

    public void SetListener(Action<T> callback) {
        _actionPathString.RemoveAllListener();
        _actionPathString.AddListener(callback);
    }
}

[Serializable]
public class StaticAction<T,U> : StaticActionBase {

    public static StaticAction<T,U> operator +(StaticAction<T,U> staticAction, Action<T,U> callback) {
        staticAction._actionPathString.AddListener(callback);
        return staticAction;
    }

    public static StaticAction<T,U> operator -(StaticAction<T,U> staticAction, Action<T,U> callback) {
        staticAction._actionPathString.RemoveListener(callback);
        return staticAction;
    }

    public void SetListener(Action<T,U> callback) {
        _actionPathString.RemoveAllListener();
        _actionPathString.AddListener(callback);
    }
}

[Serializable]
public class StaticAction<T,U,V> : StaticActionBase {

    public static StaticAction<T,U,V> operator +(StaticAction<T,U,V> staticAction, Action<T,U,V> callback) {
        staticAction._actionPathString.AddListener(callback);
        return staticAction;
    }

    public static StaticAction<T,U,V> operator -(StaticAction<T,U,V> staticAction, Action<T,U,V> callback) {
        staticAction._actionPathString.RemoveListener(callback);
        return staticAction;
    }

    public void SetListener(Action<T,U,V> callback) {
        _actionPathString.RemoveAllListener();
        _actionPathString.AddListener(callback);
    }
}


public class StaticActionBase {
    [SerializeField] protected string _actionPathString;

#if UNITY_EDITOR
    [SerializeField, HideInInspector] string _displayActionPathString;
#endif

    public void RemoveAllListener() {
        _actionPathString.RemoveAllListener();
    }
}



}
