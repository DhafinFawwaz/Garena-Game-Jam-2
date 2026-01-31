using System;
using DhafinFawwaz.ActionExtension;
using DhafinFawwaz.AnimationUI;
using UnityEngine;
using Event = DhafinFawwaz.AnimationUI.Event;
public class CounterObserver : MonoBehaviour
{
    void OnEnable() {
        Collector.S_OnCollectibleCollected += HandleCollectibleCollected;
    }

    void OnDisable() {
        Collector.S_OnCollectibleCollected -= HandleCollectibleCollected;
    }

    [SerializeField] Counter _counter;
    public static Action S_OnCounterChanged;

    void HandleCollectibleCollected(CollectEvent e) {
        e.Collectible.AnimationUI.Get<PositionToRtTween>(0).SetTo(_counter.CollectPoint).SetFromAsTargetValue();
        e.Collectible.AnimationUI.Get<Event>(2).UnityEvent.AddListener(() => {
            _counter.SetCount(e.Count);
            S_OnCounterChanged?.Invoke();
            Destroy(e.Collectible.gameObject);
        });
        e.Collectible.AnimationUI.Play();
    }
}
