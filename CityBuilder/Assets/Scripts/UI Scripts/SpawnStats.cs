using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStats : MonoBehaviour
{
    [SerializeField] private GameObject statPanelPrefab; 
    private GameObject spawnedStatPanel; 
    private bool isPlaced = false; 

    private void OnMouseDown()
    {
        if (!isPlaced)
        {
            return; 
        }

        if (spawnedStatPanel != null)
        {
            Destroy(spawnedStatPanel);
        }
        else
        {
            SpawnStatPanel();
        }
    }

    public void PlaceHouse()
    {
        isPlaced = true;
    }

    private void SpawnStatPanel()
    {
        spawnedStatPanel = Instantiate(statPanelPrefab);
        spawnedStatPanel.transform.position = transform.position + new Vector3(0, 1, 0);
    }
}