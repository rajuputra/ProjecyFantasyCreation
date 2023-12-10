using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clip")]
    public AudioClip menuBackground;

    private void Start()
    {
        musicSource.clip = menuBackground;
        musicSource.Play();
    }

}
