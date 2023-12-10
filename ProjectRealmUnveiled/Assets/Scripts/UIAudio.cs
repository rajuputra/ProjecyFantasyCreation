using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] AudioClip click;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SoundOnClick()
    {
        audioSource.PlayOneShot(click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
