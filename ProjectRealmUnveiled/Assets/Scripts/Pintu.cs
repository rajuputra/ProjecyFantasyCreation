using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pintu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void Unactive()
    {
        gameObject.SetActive(false);
    }
}
