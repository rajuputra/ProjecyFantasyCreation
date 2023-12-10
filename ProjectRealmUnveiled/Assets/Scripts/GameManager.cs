using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;

    public Vector2 platformingRespawnPoint;
    public Vector2 respawnPoint;
    [SerializeField] SaveStuff saveStuff;

    public GameObject shade;



    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        SaveData.Instance.Initialize();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if(Player.Instance != null)
        {
            if (Player.Instance.halfMana)
            {
                SaveData.Instance.LoadShadeData();
                if(SaveData.Instance.sceneWithShade == SceneManager.GetActiveScene().name || SaveData.Instance.sceneWithShade == "")
                {
                    Instantiate(shade, SaveData.Instance.shadePos, SaveData.Instance.shadeRot);
                }
            }
        }
        SaveScene();
        DontDestroyOnLoad(gameObject);
        saveStuff = FindObjectOfType<SaveStuff>();
    }

    public void SaveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SaveData.Instance.sceneNames.Add(currentSceneName);
    }

    public void RespawnPlayer()
    {
        
        SaveData.Instance.LoadSaveStuff();

        if (SaveData.Instance.saveStuffSceneName != null ) // load the saveStuff's scene if it exists.
        {
            SceneManager.LoadScene(SaveData.Instance.saveStuffSceneName);
        }

        if(SaveData.Instance.saveStuffPos != null) // set the respawn point to save stuff position
        {
            respawnPoint = SaveData.Instance.saveStuffPos;
        }
        else
        {
            respawnPoint = platformingRespawnPoint;
        }

        
        Player.Instance.transform.position = respawnPoint;
        StartCoroutine(UIManager.Instance.DeactiveDeathScene());

        if (Player.Instance.deathCauseSpike)
        {
            Player.Instance.deathCauseSpike = false;
            Player.Instance.RespawnedFromSpike();
            
        }
        else
        {
            Player.Instance.Respawned();
        }
        
        StartCoroutine(SaveAfterDeath());
        
    }

    IEnumerator SaveAfterDeath()
    {
        yield return new WaitForSeconds(1);
        SaveData.Instance.SavePlayerData();
    }

}
