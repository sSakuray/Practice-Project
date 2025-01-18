using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePlacer : MonoBehaviour
{
    private bool isPlacing = false;

    private void Update()
    {
        if (isPlacing)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
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

    private void PlaceHouse()
    {
        isPlacing = false;
        GameManager.Instance.AddHouse(transform.gameObject);
    }
}
