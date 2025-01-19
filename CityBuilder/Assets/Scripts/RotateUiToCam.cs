using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUiToCam : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    
    private void Update() {
        transform.rotation = mainCamera.transform.rotation;
    }
}
