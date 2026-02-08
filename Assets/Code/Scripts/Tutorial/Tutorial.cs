using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorial", menuName = "Game/Tutorial")]
public class Tutorial : ScriptableObject
{
    [SerializeField] string _tutorialId;
    public string TutorialId => _tutorialId;

    [SerializeField] List<TutorialStep> _steps = new();
    public List<TutorialStep> Steps => _steps;

    [SerializeField] bool _showOnlyOnce = true;
    public bool ShowOnlyOnce => _showOnlyOnce;

    public bool HasBeenCompleted()
    {
        return PlayerPrefs.GetInt("Tutorial_" + _tutorialId, 0) == 1;
    }

    public void MarkCompleted()
    {
        // PlayerPrefs.SetInt("Tutorial_" + _tutorialId, 1);
        // PlayerPrefs.Save();
    }
}
