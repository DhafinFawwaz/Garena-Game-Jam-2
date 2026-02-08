using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HerdNeutralHolder : MonoBehaviour
{
    public static Action<SheepCore> S_OnNeutralConverted;
    public static Action S_OnConverted;

    [SerializeField] SheepCore _owner;
    [SerializeField] ParticleSystem _convertVFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_owner.Herd == null) return;
        if (_owner.Stats.State == EntityType.Neutral) return;


        if (other.attachedRigidbody == null) return;
        if (!other.attachedRigidbody.TryGetComponent<SheepCore>(out var sheep)) return;
        if (sheep.Stats.State != EntityType.Neutral) return;
        if (sheep.Herd != null) return;

        Herd herd = _owner.Herd;

        S_OnNeutralConverted?.Invoke(sheep);

        if (herd.IsPlayerHerd)
            sheep.ConvertToFriendly();
        else
            sheep.ConvertToEnemy();
        
        _convertVFX.Play();
        S_OnConverted?.Invoke();

        sheep.Stats.TeamID = herd.TeamID;
        sheep.transform.SetParent(herd.transform);
        herd.AddMember(sheep);
    }
}
