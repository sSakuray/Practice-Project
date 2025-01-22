using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PanCamera : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
    [Range(0.1f, 5f)]
    [Tooltip("How sensitive the mouse drag to camera rotation")]
    public float mouseRotateSpeed = 0.8f;
    [Range(0.01f, 100)]
    [Tooltip("How sensitive the touch drag to camera rotation")]
    public float touchRotateSpeed = 17.5f;
    [Tooltip("Smaller positive value means smoother rotation, 1 means no smooth apply")]
    public float slerpValue = 0.25f; 
    public enum RotateMethod { Mouse, Touch };
    [Tooltip("How do you like to rotate the camera")]
    public RotateMethod rotateMethod = RotateMethod.Mouse;
    [SerializeField] private float zoomSpeed = 6f; 
    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 120f; 
    [SerializeField] private float targetXRotation = 20f;


    private Vector2 swipeDirection;
    private Quaternion cameraRot; 
    private Touch touch;
    private float distanceBetweenCameraAndTarget;

    private float minXRotAngle = 20; 
    private float maxXRotAngle  = 89; 
    private float rotX;
    private float rotY; 
    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        
    }
    void Start()
    {
        distanceBetweenCameraAndTarget = Vector3.Distance(mainCamera.transform.position, target.position);
    }

    void Update()
    {
        if (rotateMethod == RotateMethod.Mouse)
        {
            if (Input.GetMouseButton(0))
            {
                rotX += -Input.GetAxis("Mouse Y") * mouseRotateSpeed; 
                rotY += Input.GetAxis("Mouse X") * mouseRotateSpeed;
            }

            if (rotX < minXRotAngle)
            {
                rotX = minXRotAngle;
            }
            else if (rotX > maxXRotAngle)
            {
                rotX = maxXRotAngle;
            }
        }
        else if (rotateMethod == RotateMethod.Touch)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {

                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    swipeDirection += touch.deltaPosition * Time.deltaTime * touchRotateSpeed;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                }
            }

            if (swipeDirection.y < minXRotAngle)
            {
                swipeDirection.y = minXRotAngle;
            }
            else if (swipeDirection.y > maxXRotAngle)
            {
                swipeDirection.y = maxXRotAngle;
            }
        }

        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (mainCamera.orthographicSize  <= minZoom)
        {
            mainCamera.orthographicSize = minZoom; 
        }
        else if (mainCamera.orthographicSize >= maxZoom)
        {
            mainCamera.orthographicSize = maxZoom; 
        }

        //Min X Degree Camera Rotation NOT WORKS

        float currentXRotation = transform.localEulerAngles.x;

        if (currentXRotation < targetXRotation)
        {
            SetLocalXRotation(targetXRotation);
        }

    }
    private void SetLocalXRotation(float xRotation) //NOT WORKS
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.x = xRotation;
        mainCamera.transform.localEulerAngles = currentRotation;
    }

    private void LateUpdate()
    {

        Vector3 dir = new Vector3(0, 0, -distanceBetweenCameraAndTarget);

        Quaternion newQ; 
        if (rotateMethod == RotateMethod.Mouse)
        {
           newQ  = Quaternion.Euler(rotX , rotY, 0); 
        }
        else
        {
            newQ = Quaternion.Euler(swipeDirection.y , -swipeDirection.x, 0);
        }
        cameraRot = Quaternion.Slerp(cameraRot, newQ, slerpValue);  
        mainCamera.transform.position = target.position + cameraRot * dir;
        mainCamera.transform.LookAt(target.position);

    }

    public void SetCamPos()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        mainCamera.transform.position = new Vector3(0, 0, -distanceBetweenCameraAndTarget);
    }

}