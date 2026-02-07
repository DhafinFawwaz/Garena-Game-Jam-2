using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class HerdHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Action<Herd> S_OnHerdHoverEnter;
    public static Action<Herd> S_OnHerdHoverExit;

    [SerializeField] float _colliderPadding = 1f;

    Herd _herd;
    BoxCollider2D _collider;

    void Awake() {
        _herd = GetComponent<Herd>();
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }

    void Update() {
        UpdateColliderBounds();
    }

    void UpdateColliderBounds() {
        if (_herd == null || _herd.Members.Count == 0) {
            _collider.size = Vector2.zero;
            return;
        }

        Vector3 herdPos = transform.position;
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (var member in _herd.Members) {
            if (member == null) continue;
            Vector3 pos = member.transform.position;
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        float centerX = (minX + maxX) / 2f - herdPos.x;
        float centerY = (minY + maxY) / 2f - herdPos.y;
        float sizeX = (maxX - minX) + _colliderPadding * 2f;
        float sizeY = (maxY - minY) + _colliderPadding * 2f;

        _collider.offset = new Vector2(centerX, centerY);
        _collider.size = new Vector2(sizeX, sizeY);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        S_OnHerdHoverEnter?.Invoke(_herd);
    }

    public void OnPointerExit(PointerEventData eventData) {
        S_OnHerdHoverExit?.Invoke(_herd);
    }
}
