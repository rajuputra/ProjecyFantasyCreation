using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThemeSong : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip defaultMusic;
    [SerializeField] AudioClip bossMusic;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        defaultMusic = audioSource.clip;
    }

    // Update is called once per frame
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }*/

    public void PlayBossMusic()
    {
        audioSource.clip = bossMusic;
        audioSource.Play();
    }

    public void PlayDefaultMusic()
    {
        audioSource.clip = defaultMusic;
        audioSource.Play();
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            audioSource.clip = defaultMusic;
            audioSource.Play();
        }
    }*/
}
