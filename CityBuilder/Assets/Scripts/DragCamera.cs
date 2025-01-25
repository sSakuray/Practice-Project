using UnityEngine;

public class DragCamera : MonoBehaviour
{
    private Vector3 dragOrigin;
    private bool isDragging = false;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform rotationObject;
    void Update()
    {
        if (Input.GetMouseButtonDown(2)) 
        {
            isDragging = true;
            dragOrigin = Input.mousePosition; 
        }
        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }
        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - dragOrigin; 
            dragOrigin = Input.mousePosition; 

            Vector3 move = new Vector3(-mouseDelta.x * 0.1f, -mouseDelta.y * 0.1f, 0); 
            transform.position += transform.TransformDirection(move); 
        }
        rotationObject.rotation = mainCamera.transform.rotation;
    }
}
