using System;
using UnityEngine;

[SelectionBase]
public class LetterPlacable : MonoBehaviour
{
    [ReadOnly] public LetterDragable PlacedLetter;

    public char GetLetter() {
        if (PlacedLetter == null) return ' ';
        return PlacedLetter.Letter;
    }

    public Action<LetterPlacable> OnLetterPlaced;
    public void Place(LetterDragable letter)
    {
        PlacedLetter = letter;
        OnLetterPlaced?.Invoke(this);
    }
    public void Unplace()
    {
        PlacedLetter = null;
        OnLetterPlaced?.Invoke(this);
    }


    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Color _highlightColor = Color.yellow;
    [SerializeField] Color _defaultColor = Color.white;
    public void Highlight(LetterDragable letter)
    {
        _renderer.color = _highlightColor;
    }
    public void Unhighlight(LetterDragable letter)
    {
        _renderer.color = _defaultColor;
    }
}
