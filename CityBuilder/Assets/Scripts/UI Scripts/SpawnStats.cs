using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStats : MonoBehaviour
{
    [SerializeField] private GameObject statPanelPrefab; 
    [SerializeField] private Animator statsPanelAnim;
    [SerializeField] private GameObject originalPrefab;
    private GameObject spawnedStatPanel; 
    private bool isPlaced = false; 
    private bool isPanelOpen = false;

    public void Initialize(GameObject prefab)
    {
        originalPrefab = prefab;
    }

    private void OnMouseDown()
    {
        if (!isPlaced)
        {
            return; 
        }

        if (spawnedStatPanel != null)
        {
            Destroy(spawnedStatPanel);
            spawnedStatPanel = null;
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
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            return;
        }

        spawnedStatPanel = Instantiate(statPanelPrefab, canvas.transform);
        HouseActions houseActions = spawnedStatPanel.GetComponent<HouseActions>();
        if (houseActions != null)
        {
            ShopManager shopManager = FindObjectOfType<ShopManager>();
            if (shopManager != null)
            {
                float housePrice = shopManager.GetHousePrice(originalPrefab);
                houseActions.SetCurrentHouse(gameObject, housePrice);
                houseActions.statPanel = spawnedStatPanel;
            }
        }
        


        RectTransform rectTransform = spawnedStatPanel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0, 0.4f); 
            rectTransform.anchorMax = new Vector2(0, 0.4f);
            rectTransform.pivot = new Vector2(0, 0.4f); 
            rectTransform.anchoredPosition = new Vector2(0, 0); 

            HouseManager houseManager = FindObjectOfType<HouseManager>();
            if (houseManager == null)
            {
                return;
            }

            HouseManager.HouseData houseData = houseManager.GetHouseData(originalPrefab);
            if (houseData !=null)
            {
                StatsCard statsCard = spawnedStatPanel.GetComponent<StatsCard>();
                if (statsCard != null)
                {
                    statsCard.UpdateStats(houseData.houseName, houseData.citizens, houseData.energy, houseData.income);
                }
            
            }
        }
    }
    public GridCell AssociatedCell { get; set; }

    private void OnDestroy()
    {
        if (AssociatedCell != null)
        {
            AssociatedCell.isOccupied = false;
        }
    }
}
