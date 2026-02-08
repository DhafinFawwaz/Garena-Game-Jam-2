using System;
using UnityEngine;

public class HerdNeutralHolder : MonoBehaviour
{
    public static Action<SheepCore> S_OnNeutralConverted;

    [SerializeField] Herd _herd;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) return;
        if (!other.attachedRigidbody.TryGetComponent<SheepCore>(out var sheep)) return;
        if (sheep.Stats.State != EntityType.Neutral) return;

        S_OnNeutralConverted?.Invoke(sheep);

        if (_herd.IsPlayerHerd)
            sheep.ConvertToFriendly();
        else
            sheep.ConvertToEnemy();

        sheep.Stats.TeamID = _herd.Members.Count > 0 ? _herd.Members[0].Stats.TeamID : 0;
        _herd.AddMember(sheep);
    }
}
