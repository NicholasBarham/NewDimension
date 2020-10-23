using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;

    [SerializeField]
    private AudioClip jumpAudio = null;

    [SerializeField]
    private AudioClip landAudio = null;

    [SerializeField]
    private AudioClip deathAudio = null;

    [SerializeField]
    private AudioClip[] stepAudio = null;

    private void Awake()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        audioSource.pitch = GetRandomPitch();
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(stepAudio[GetRandomSource(stepAudio.Length)]);
    }

    public void PlayJump()
    {
        audioSource.pitch = GetRandomPitch();
        audioSource.volume = 0.326f;
        audioSource.PlayOneShot(jumpAudio);
    }

    public void PlayDeath()
    {
        audioSource.pitch = GetRandomPitch();
        audioSource.volume = 0.326f;
        audioSource.PlayOneShot(deathAudio);
    }

    public void PlayLanding()
    {
        audioSource.pitch = GetRandomPitch();
        audioSource.volume = 0.326f;
        audioSource.PlayOneShot(landAudio);
    }

    private float GetRandomPitch() => Random.Range(0.85f, 1.15f);

    private int GetRandomSource(int arrayLength) => Random.Range(0, arrayLength - 1);
}
