using DhafinFawwaz.AnimationUI;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] AnimationUI _collectedAUI;
    [SerializeField] TMP_Text _text;
    public RectTransform CollectPoint;

    public void SetCount(int newCount) {
        _text.text = newCount.ToString();
        _collectedAUI.Stop();
        _collectedAUI.Play();
    }
}
