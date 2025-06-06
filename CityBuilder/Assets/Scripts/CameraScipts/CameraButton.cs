using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraButton : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform rotationPoint;
    private int click = 0;
    private Vector3 targetRotation = new Vector3(45f, -45f, 0f);
    private Vector3 targetRotation2 = new Vector3(89f, 0f, 0f);
    private Vector3 targetPosition = new Vector3(30f, 0f, -29f);
    public void Click ()
    {
        if (click == 0)
        {
            click = 0;
            rotationPoint.transform.position = targetPosition;
        }

    }
}
