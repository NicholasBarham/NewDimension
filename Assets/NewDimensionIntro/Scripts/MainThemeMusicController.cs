using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class MainThemeMusicController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private float minimumVolume = -80f;

    [SerializeField]
    private float maximumVolume = 0f;

    [SerializeField]
    private float lengthToFadeOut = 2f;

    [SerializeField]
    private float lengthToFadeIn = 0.1f;

    private const string MUSIC_VOLUME_KEY = "music_volume";

    private Coroutine routine = null;

    [SerializeField]
    private UnityEvent<float> onVolumeInit = null;

    private void Awake()
    {
        SetInitAudioVolume();
    }

    private void SetInitAudioVolume()
    {
        float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, maximumVolume);

        mixer.SetFloat(MUSIC_VOLUME_KEY, volume);
        onVolumeInit?.Invoke(volume);
    }

    public void FadeInMusic()
    {
        if (routine != null)
            StopCoroutine(routine);

        source.Play();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float volume;

        mixer.GetFloat(MUSIC_VOLUME_KEY, out volume);

        float multiplier = Mathf.Abs(maximumVolume - volume);

        while (volume < maximumVolume)
        {
            volume += (Time.deltaTime / lengthToFadeIn) * multiplier;
            mixer.SetFloat(MUSIC_VOLUME_KEY, volume);
            yield return null;
        }
    }

    public void FadeOutMusic()
    {
        if(routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float volume;
        
        mixer.GetFloat(MUSIC_VOLUME_KEY, out volume);

        float multiplier = Mathf.Abs(minimumVolume - volume);

        while(volume > minimumVolume)
        {
            volume -= (Time.deltaTime / lengthToFadeOut) * multiplier;
            mixer.SetFloat(MUSIC_VOLUME_KEY, volume);
            yield return null;
        }

        source.Stop();
        SetInitAudioVolume();
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        mixer.SetFloat(MUSIC_VOLUME_KEY, volume);
    }
}
