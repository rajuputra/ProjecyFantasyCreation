using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] canvasObjects = GameObject.FindGameObjectsWithTag("Canvas");
        foreach (GameObject obj in canvasObjects)
        {
            Destroy(obj);
        }
    }
}
