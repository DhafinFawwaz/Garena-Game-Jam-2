using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Collider2D _col;
    public AnimationUI AnimationUI;
    public void ToUninteractable() {
        _rb.bodyType = RigidbodyType2D.Static;
        _col.enabled = false;
    }
}
