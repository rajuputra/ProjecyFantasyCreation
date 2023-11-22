using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;

    public Vector2 platformingRespawnPoint;
    public Vector2 respawnPoint;
    [SerializeField] SetSpawnPoint setspawn;

    public GameObject shade;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        setspawn = FindObjectOfType<SetSpawnPoint>();
    }

    public void RespawnPlayer()
    {
        if(setspawn != null)
        {
            if (setspawn.interacted)
            {
                respawnPoint = setspawn.transform.position;
            }
            else
            {
                respawnPoint = platformingRespawnPoint;
            }
        }
        else
        {
            respawnPoint = platformingRespawnPoint;
        }

        
        Player.Instance.transform.position = respawnPoint;
        StartCoroutine(UIManager.Instance.DeactiveDeathScene());
        Player.Instance.Respawned();
    }

}
