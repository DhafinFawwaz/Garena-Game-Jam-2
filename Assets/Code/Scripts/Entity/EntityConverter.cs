using UnityEngine;

public class EntityConverter : MonoBehaviour
{
    // [friendly, normal, enemy]
    public Sprite[] DepanBawah;
    public Sprite[] DepanAtas;

    public Sprite[] Fall;
    public SpriteArray[] Attack;
    public SpriteArray[] Eat;
}

[System.Serializable]
public class SpriteArray {
    public Sprite[] Sprites;
}
