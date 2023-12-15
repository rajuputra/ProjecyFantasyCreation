using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveStuff : MonoBehaviour
{
    private Animator anim;
    public bool interacted;
    

    public Transform detectionPoint;
    private const float detectionRadius = 1f;
    public LayerMask detectionLayer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    

    


    // Update is called once per frame
    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                Enter();
            }
        }
        else if (!DetectObject())
        {
            Keluar();
        }
    }

    bool InteractInput()
    {
        return Input.GetButtonDown("Interact");
    }

    bool DetectObject()
    {
        return Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
    }

    /*private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            anim.SetBool("Interacted", true);
            interacted = true;
            

            SaveData.Instance.saveStuffSceneName = SceneManager.GetActiveScene().name;
            SaveData.Instance.saveStuffPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            SaveData.Instance.SaveSaveStuff();
            SaveData.Instance.SavePlayerData();
            

        }
    }*/

    public void Enter()
    {
        anim.SetBool("Interacted", true);
        interacted = true;


        SaveData.Instance.saveStuffSceneName = SceneManager.GetActiveScene().name;
        SaveData.Instance.saveStuffPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        SaveData.Instance.SaveSaveStuff();
        SaveData.Instance.SavePlayerData();
    }

    public void Keluar()
    {
        anim.SetBool("Interacted", false);
        interacted = false;
    }

    /*private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            anim.SetBool("Interacted", false);
            interacted = false;
            
        }
    }*/
}
