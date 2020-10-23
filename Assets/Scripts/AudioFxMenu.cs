using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioFxMenu : MonoBehaviour
{
    private const string FX_VOLUME_KEY = "fx_volume";

    [SerializeField]
    private AudioMixer mixer = null;

    [SerializeField]
    private float minimumVolume = -80f;

    [SerializeField]
    private float maximumVolume = 0f;

    [SerializeField]
    private UnityEvent<float> onVolumeInit = null;

    private void Awake()
    {
        SetInitAudioVolume();
    }

    private void SetInitAudioVolume()
    {
        float volume = PlayerPrefs.GetFloat(FX_VOLUME_KEY, maximumVolume);

        mixer.SetFloat(FX_VOLUME_KEY, volume);
        onVolumeInit?.Invoke(volume);
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(FX_VOLUME_KEY, volume);
        mixer.SetFloat(FX_VOLUME_KEY, volume);
    }
}
