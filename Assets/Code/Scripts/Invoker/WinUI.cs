using TMPro;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] TMP_Text _armyCountText;

    void OnEnable()
    {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state)
    {
        if (state != GameState.Win) return;

        var spawner = HerdSpawner.Instance;
        int count = spawner != null && spawner.PlayerHerd != null ? spawner.PlayerHerd.transform.childCount : 0;

        if (_armyCountText != null)
            _armyCountText.text = count.ToString();
    }
}
