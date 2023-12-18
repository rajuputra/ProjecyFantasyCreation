using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryTrigger : MonoBehaviour
{
    
    
    private void Start()
    {
        
    }

    private void Update()
    {
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.tag == "Player")
        {
            GameObject[] canvasObjects = GameObject.FindGameObjectsWithTag("Canvas");
            foreach (GameObject obj in canvasObjects)
            {
                Destroy(obj);
            }
            SceneManager.LoadScene("StoryTerakhir");
        }
    }
}
