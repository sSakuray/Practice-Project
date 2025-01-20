using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateUiToCam : MonoBehaviour
{
    [SerializeField] private GameObject RotationObject;
    
    private void Update() 
    {
        RotationObject.transform.rotation = Camera.main.transform.rotation;
    }
    
}
