using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    public const string MUSIC_VOLUME_KEY = "musicVolume";
    public const string SOUND_VOLUME_KEY = "soundVolume";
    public const string AUDIO_MIXER_VOLUME_KEY = "Volume";
    public const float DEFAULT_MUSIC_VOLUME = 0.4f;
    public const float DEFAULT_SOUND_VOLUME = 0.6f;
    public const string AUDIO_MIXER_MUSIC_RESOURCE_PATH = "Music";
    public const string AUDIO_MIXER_SOUND_RESOURCE_PATH = "Sound";
    public const string AUDIO_MIXER_MUSIC_GROUP = "Master";
    public const string AUDIO_MIXER_SOUND_GROUP = "Master";


    const float MIN_VOLUME = -50f;
    const float MAX_VOLUME = 10f;

    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _soundSource;
    public AudioSource MusicSource => _musicSource;
    public AudioSource SoundSource => _soundSource;
    [SerializeField] AudioMixer _musicMixer;
    [SerializeField] AudioMixer _soundMixer;
    static float OutCubic(float x) => -((1-x)*(1-x)*(1-x)) + 1;
    void Start()
    {
        _musicMixer.SetFloat(AUDIO_MIXER_VOLUME_KEY, 
            Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, OutCubic(
                GetMusicVolume()
            ))
        );
        _soundMixer.SetFloat(AUDIO_MIXER_VOLUME_KEY, 
            Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, OutCubic(
                GetSoundVolume()
            ))
        );
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() {
        var go = new GameObject("AudioManager");
        var am = go.AddComponent<AudioManager>();
        _instance = am;
        var musicGo = new GameObject("Music");
        var soundGo = new GameObject("Sound");
        musicGo.transform.SetParent(go.transform);
        soundGo.transform.SetParent(go.transform);
        am._musicSource = musicGo.AddComponent<AudioSource>();
        am._soundSource = soundGo.AddComponent<AudioSource>();
        am._musicSource.loop = true;
        am._soundSource.playOnAwake = false;
        var musicMixer = Resources.Load<AudioMixer>(AUDIO_MIXER_MUSIC_RESOURCE_PATH);
        var soundMixer = Resources.Load<AudioMixer>(AUDIO_MIXER_SOUND_RESOURCE_PATH);
        am._musicMixer = musicMixer;
        am._soundMixer = soundMixer;

        am._musicSource.outputAudioMixerGroup = am._musicMixer.FindMatchingGroups(AUDIO_MIXER_MUSIC_GROUP)[0];
        am._soundSource.outputAudioMixerGroup = am._soundMixer.FindMatchingGroups(AUDIO_MIXER_SOUND_GROUP)[0];
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_MUSIC_VOLUME);
    public float GetSoundVolume() => PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, DEFAULT_SOUND_VOLUME);

    public AudioClip GetCurrentMusicClip() => _musicSource.clip;

    public void PlayMusic(AudioClip audioClip) {
        _musicSource.clip = audioClip;
        _musicSource.Stop();
        _musicSource.Play();
    }
    public void StopMusic() {
        _musicSource.clip = null;
        _musicSource.Stop();
    }
    public void StopPlayMusic() {
        _musicSource.Stop();
        _musicSource.Play();
    }


    public void PlaySound(AudioClip audioClip, float volume) => _soundSource.PlayOneShot(audioClip, volume);
    public void PlaySound(AudioClip audioClip) => _soundSource.PlayOneShot(audioClip);
    public void SetMusicSourceVolume(float t) => _musicSource.volume = t;

    public void SetMusicMixerVolume(float newVal)
    {
        if(newVal < 0.01f) _musicSource.mute = true;
        else _musicSource.mute = false;
        _musicMixer.SetFloat(AUDIO_MIXER_VOLUME_KEY, 
            Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, OutCubic(newVal))
        );
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, newVal);
    }
    public void SetSoundMixerVolume(float newVal)
    {
        if(newVal < 0.01f) _soundSource.mute = true;
        else _soundSource.mute = false;
        _soundMixer.SetFloat(AUDIO_MIXER_VOLUME_KEY, 
            Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, OutCubic(newVal))
        );
        PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, newVal);
    }

    public void ToggleLoop(bool isLooping) => _musicSource.loop = isLooping;
    public void MusicFadeOut(float duration) => StartCoroutine(MusicFadeOutIEnumerator(duration));
    public void MusicFadeIn(float duration) => StartCoroutine(MusicFadeInIEnumerator(duration));

    IEnumerator MusicFadeInIEnumerator(float duration) {
        float t = 0;
        while(t <= 1) {
            _musicSource.volume = t;
            t += Time.unscaledDeltaTime/duration;
            yield return null;
        }
        _musicSource.volume = 0;
    }

    IEnumerator MusicFadeOutIEnumerator(float duration) {
        float t = 0;
        while(t <= 1) {
            _musicSource.volume = 1 - t;
            t += Time.unscaledDeltaTime/duration;
            yield return null;
        }
        _musicSource.volume = 0;
    }
    public void MusicFadeOutAndChangeTo(AudioClip _musicClip, bool isLooping, float duration, float delayBeforeChangeDuration)
        => StartCoroutine(MusicFadeOutAndChangeToIEnumerator(_musicClip, isLooping, duration, delayBeforeChangeDuration));
    IEnumerator MusicFadeOutAndChangeToIEnumerator(AudioClip _musicClip, bool isLooping, float duration, float delayBeforeChangeDuration = 0) {
        yield return StartCoroutine(MusicFadeOutIEnumerator(duration));
        yield return new WaitForSecondsRealtime(delayBeforeChangeDuration);
        _musicSource.volume = 1;
        PlayMusic(_musicClip);
        ToggleLoop(isLooping);
    }
}

