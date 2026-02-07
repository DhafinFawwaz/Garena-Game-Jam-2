using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem _alertVFX;
    [SerializeField] ParticleSystem _alertConfusedVFX;

    public void PlayAlertVFX()
    {
        _alertVFX.Play();
    }

    public void PlayAlertConfusedVFX()
    {
        _alertConfusedVFX.Play();
    }
}
