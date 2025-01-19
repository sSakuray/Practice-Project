using UnityEngine;

public class DragCamera : MonoBehaviour
{
    private Vector3 dragOrigin;
    private bool isDragging = false;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform rotationObject;

    void Update()
    {
        // Check if the middle mouse button is pressed
        if (Input.GetMouseButtonDown(2)) // 2 is the middle mouse button
        {
            isDragging = true;
            dragOrigin = Input.mousePosition; // Store the initial mouse position
        }

        // Check if the middle mouse button is released
        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }

        // If dragging, update the camera's position based on mouse movement
        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - dragOrigin; // Calculate the mouse movement
            dragOrigin = Input.mousePosition; // Update the drag origin for the next frame

            // Convert mouse movement to world space movement
            Vector3 move = new Vector3(-mouseDelta.x * 0.1f, -mouseDelta.y * 0.1f, 0); // Adjust sensitivity as needed
            transform.position += transform.TransformDirection(move); // Move the camera
        }

        rotationObject.rotation = mainCamera.transform.rotation;
    }
}
