using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;  // Added line to explicitly assign the camera object
    [SerializeField] private float relativeMovement = 0.3f;
    [SerializeField] private bool LockY = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;  // Assign the camera object in Start
    }

    void Update()
    {
        if (LockY)
        {
            transform.position = new Vector2(cameraTransform.position.x * relativeMovement, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(cameraTransform.position.x * relativeMovement, cameraTransform.position.y * relativeMovement);
        }
    }
}
