using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            StartCoroutine(RespawnPoint());
        }
    }

    IEnumerator RespawnPoint()
    {
        Player.Instance.aState.cutscene = true;
        Player.Instance.aState. invincible = true;
        Player.Instance.rb.velocity = Vector2.zero;
        Time.timeScale = 0;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        Player.Instance.TakeDamage(1);
        yield return new WaitForSeconds(1);
        Player.Instance.transform.position = GameManager.Instance.alexRespawnPoint;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        yield return new WaitForSeconds(UIManager.Instance.sceneFader.fadeTime);
        Player.Instance.aState.cutscene = false;
        Player.Instance.aState.invincible = false;
        Time.timeScale = 1;
    }
}