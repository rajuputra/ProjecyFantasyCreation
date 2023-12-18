using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowAnimasi()
    {
        animator.Play("show");
    }

    public void HideAnimasi()
    {
        animator.Play("hide");
    }
}
