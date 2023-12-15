using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    public GameObject hoverButton;
    // Start is called before the first frame update
    void Start()
    {
        hoverButton.SetActive(false);
    }

    public void Show()
    {
        hoverButton.SetActive(true);
    }
    public void Hide()
    {
        hoverButton.SetActive(false);
    }
}
