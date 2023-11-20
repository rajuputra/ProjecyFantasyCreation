using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1CameraValue : MonoBehaviour
{
    [SerializeField] private Vector3 minValue, maxValue; 
    void Awake()
    {
        CameraConfig.minValue = minValue; // Atur nilai sesuai kebutuhan
        CameraConfig.maxValue = maxValue; // Atur nilai sesuai kebutuhan
    }
}
