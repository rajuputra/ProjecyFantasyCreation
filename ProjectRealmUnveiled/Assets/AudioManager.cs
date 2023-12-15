using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Audio Clip")]
    public AudioClip inGameBackground;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = inGameBackground;
        audioSource.Play();
    }
}

