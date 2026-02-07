using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialStep", menuName = "Game/TutorialStep")]
public class TutorialStep : ScriptableObject
{
    [SerializeField] string _title;
    public string Title => _title;

    [SerializeField] [TextArea(3, 5)] string _message;
    public string Message => _message;

    [SerializeField] Sprite _image;
    public Sprite Image => _image;

    [SerializeField] bool _pauseGame = true;
    public bool PauseGame => _pauseGame;

    [SerializeField] float _autoAdvanceDelay = 0f;
    public float AutoAdvanceDelay => _autoAdvanceDelay;
}
