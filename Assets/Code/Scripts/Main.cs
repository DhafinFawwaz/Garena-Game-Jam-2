using UnityEngine;

public class Main {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() {
        Application.targetFrameRate = 60;
    }
}