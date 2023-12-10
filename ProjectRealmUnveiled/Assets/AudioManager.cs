using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource1;

    [Header("Audio Clip")]
    public AudioClip inGameBackground;

    private void Start()
    {
        musicSource1.clip = inGameBackground;
        musicSource1.Play();
    }
}

