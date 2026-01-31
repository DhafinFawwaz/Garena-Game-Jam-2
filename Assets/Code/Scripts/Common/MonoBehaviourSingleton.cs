using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance => _instance;
    protected static T _instance;

    
    void Awake() {
        if(_instance == null) _instance = this as T;

#if UNITY_EDITOR
        else if(!Application.isPlaying) DestroyImmediate(gameObject);
#endif
        else Destroy(gameObject);

#if UNITY_EDITOR
        if(Application.isPlaying)
#endif
        DontDestroyOnLoad(gameObject);

    }
}
