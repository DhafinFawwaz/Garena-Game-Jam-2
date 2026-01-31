using System;
using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public static Action<CollectEvent> S_OnCollectibleCollected;
    void OnTriggerEnter2D(Collider2D col) {
        if(!col.TryGetComponent<Collectibles>(out var collectible)) return;
        collectible.ToUninteractable();
        S_OnCollectibleCollected?.Invoke(new CollectEvent { Collectible = collectible });
    }
}
