using System.Linq;
using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class EntityConverter : MonoBehaviour
{
    // [friendly, normal, enemy]
    public Sprite[] DepanBawah;
    public Sprite[] BelakangAtas;

    public Sprite[] Fall;
    public SpriteArray[] Attack;
    public SpriteArray[] Eat;

    [Header("References")]
    [SerializeField] EntitySkin _skin;
    [SerializeField] SpriteRenderer _skinRenderer;

    public void ConvertToEnemy()
    {
        Convert(2);
    }
    public void ConvertToFriendly()
    {
        Convert(0);
    }
    public void ConvertToNeutral()
    {
        Convert(1);
    }

    [SerializeField] AnimationUI _cliffAUI;
    [SerializeField] AnimationUI _dieAUI;
    [SerializeField] AnimationUI _eatAUI;
    [SerializeField] AnimationUI _attackAUI;
    // [SerializeField] AnimationUI _fallAUI;
    void Convert(int idx)
    {
        _skin.SetSprites(DepanBawah[idx], BelakangAtas[idx]);
        _skinRenderer.sprite = DepanBawah[idx];

        var tween = _cliffAUI.Get<SpriteFlipbookTween>(1);
        tween.Sprites[0] = DepanBawah[idx];
        tween.Sprites[1] = Fall[idx];

        var tweendie = _dieAUI.Get<SpriteFlipbookTween>(2);
        tweendie.Sprites[0] = DepanBawah[idx];
        tweendie.Sprites[1] = Fall[idx];

        _eatAUI.Get<SpriteFlipbookTween>(0).Sprites = Eat[idx].Sprites.ToList();
        _attackAUI.Get<SpriteFlipbookTween>(0).Sprites = Attack[idx].Sprites.ToList();
    }
}

[System.Serializable]
public class SpriteArray
{
    public Sprite[] Sprites;
}
