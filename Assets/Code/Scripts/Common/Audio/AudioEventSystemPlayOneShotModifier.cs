using UnityEngine;
using DhafinFawwaz.AudioEventSystem;
[RequireComponent(typeof(AudioEventSystemPlayOneShot))]
public class AudioEventSystemPlayOneShotModifier : MonoBehaviour
{
    [SerializeField] AudioEventSystemPlayOneShot _aespo;
    void Awake() => _aespo.AudioSource = AudioManager.Instance.SoundSource;

    void Reset() => _aespo = GetComponent<AudioEventSystemPlayOneShot>();
}
