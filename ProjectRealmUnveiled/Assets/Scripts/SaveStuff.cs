using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveStuff : MonoBehaviour
{
    public bool interacted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            interacted = true;

            SaveData.Instance.saveStuffSceneName = SceneManager.GetActiveScene().name;
            SaveData.Instance.saveStuffPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            SaveData.Instance.SaveSaveStuff();
            SaveData.Instance.SavePlayerData();
            

        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            interacted = false;
        }
    }
}
