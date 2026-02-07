using UnityEngine;

public class HungerBarObserver : MonoBehaviour
{
    [SerializeField] HungerBar _hungerBar;
    [SerializeField] Transform _hungerBarRoot;
    [SerializeField] float _yOffset = 1.5f;

    Herd _currentHerd;

    void OnEnable()
    {
        HerdHolder.S_OnHerdHoverEnter += HandleHerdHoverEnter;
        HerdHolder.S_OnHerdHoverExit += HandleHerdHoverExit;
        Herd.S_OnHungerChanged += HandleHungerChanged;
        _hungerBarRoot.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        HerdHolder.S_OnHerdHoverEnter -= HandleHerdHoverEnter;
        HerdHolder.S_OnHerdHoverExit -= HandleHerdHoverExit;
        Herd.S_OnHungerChanged -= HandleHungerChanged;
    }

    void HandleHerdHoverEnter(Herd herd)
    {
        _currentHerd = herd;
        _hungerBar.SetHungerNormalizedImmediate(herd.CalculateHungerNormalized());
        _hungerBarRoot.gameObject.SetActive(true);
        UpdatePosition();
    }

    void HandleHerdHoverExit(Herd herd)
    {
        if (_currentHerd != herd) return;
        _currentHerd = null;
        _hungerBarRoot.gameObject.SetActive(false);
    }

    void HandleHungerChanged(HungerEvent e)
    {
        if (e.Herd != _currentHerd) return;
        _hungerBar.SetHungerNormalized(e.HungerNormalized);
    }

    void LateUpdate()
    {
        if (_currentHerd == null) return;
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (_currentHerd == null) return;

        float topY = GetHerdTopY(_currentHerd);
        float centerX = GetHerdCenterX(_currentHerd);
        _hungerBarRoot.position = new Vector3(centerX, topY + _yOffset, 0f);
    }

    float GetHerdTopY(Herd herd)
    {
        float maxY = herd.transform.position.y;
        foreach (var member in herd.Members)
        {
            if (member == null) continue;
            if (member.transform.position.y > maxY)
            {
                maxY = member.transform.position.y;
            }
        }
        return maxY;
    }

    float GetHerdCenterX(Herd herd)
    {
        if (herd.Members.Count == 0) return herd.transform.position.x;

        float minX = float.MaxValue, maxX = float.MinValue;
        foreach (var member in herd.Members)
        {
            if (member == null) continue;
            float x = member.transform.position.x;
            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
        }
        return (minX + maxX) / 2f;
    }
}
