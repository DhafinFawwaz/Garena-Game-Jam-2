using DhafinFawwaz.AnimationUI;
using UnityEngine;
using Event = DhafinFawwaz.AnimationUI.Event;

public class MeteorEvent : MonoBehaviour
{
    [SerializeField] AnimationUI _meteorAUI;
    [SerializeField] int _spawnEventStepIndex = 1;

    Sign _signPrefab;

    public void Init(Sign signPrefab)
    {
        _signPrefab = signPrefab;

        _meteorAUI.Get<Event>(_spawnEventStepIndex).UnityEvent.AddListener(OnAnimationFinished);
        _meteorAUI.Play();
    }

    void OnAnimationFinished()
    {
        if (_signPrefab != null)
        {
            Instantiate(_signPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
