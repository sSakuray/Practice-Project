using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePlacer : MonoBehaviour
{
    private bool isPlacing = false; 
    public LayerMask panelLayer;   

    private void Update()
    {
        if (isPlacing)
        {
            MoveHouseWithMouse();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceHouse();
            }
        }
    }

    public void StartPlacingHouse()
    {
        isPlacing = true; 
    }

    private void MoveHouseWithMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 35f;  
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.y -= 2f;
        transform.position = worldPosition;
    }

    private void PlaceHouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, panelLayer))
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();

            if (cell != null && !cell.isOccupied && !GridRegistry.IsOccupied(hit.collider.transform.position))
            {
                transform.position = hit.collider.transform.position;
                cell.SetOccupied(true);
                var houseComponent = GetComponent<SpawnStats>();
                if (houseComponent != null)
                {
                    houseComponent.AssociatedCell = cell;
                }

                GetComponent<SpawnStats>().PlaceHouse();
                isPlacing = false;      
                GameManager.Instance.HousePlaced();
            }
            else
            {
                Debug.LogWarning($"Невозможно разместить здание: позиция {hit.collider.transform.position} уже занята!");
            }
        }
    }
}
