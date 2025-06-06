using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStats : MonoBehaviour
{
    [SerializeField] private GameObject statPanelPrefab; 
    [SerializeField] private Animator statsPanelAnim;    
    [SerializeField] private GameObject originalPrefab;
    [SerializeField] private GameObject upgradedFromPrefab; 
    public static GameObject currentActiveStatPanel; 
    private GameObject spawnedStatPanel;             
    private bool isPlaced = false;                   
    public bool IsPlaced => isPlaced;
    private int currentHousePrice;                  
    public GridCell AssociatedCell { get; set; }       
    public GameObject OriginalPrefab => originalPrefab;
    public GameObject UpgradedFromPrefab => upgradedFromPrefab;
    public bool IsUpgraded => upgradedFromPrefab != null;

    public void Initialize(GameObject prefab, GameObject upgradedFrom = null)
    {
        originalPrefab = prefab;
        upgradedFromPrefab = upgradedFrom;
        ShopManager shopManager = FindObjectOfType<ShopManager>();
        if (shopManager != null)
        {
            currentHousePrice = shopManager.GetHousePrice(originalPrefab);
        }
    }
    private void OnMouseDown()
    {
        if (!isPlaced)
        {
            return; 
        }
        if (currentActiveStatPanel != null && currentActiveStatPanel == spawnedStatPanel)
        {
            Destroy(currentActiveStatPanel); 
            currentActiveStatPanel = null;  
            if (HouseArrowClick.currentArrow != null)
            {
                Destroy(HouseArrowClick.currentArrow);
                HouseArrowClick.currentArrow = null;
            }
            return; 
        }
        if (currentActiveStatPanel != null)
        {
            Destroy(currentActiveStatPanel);
            currentActiveStatPanel = null;
        }
        SpawnStatPanel();
    }
    public void PlaceHouse()
    {
        isPlaced = true; 
        
        if (!IsUpgraded)
        {
            GameManager.Instance.UpdateStatsOnHousePlaced(originalPrefab);
        }
        else
        {
            HouseManager houseManager = FindObjectOfType<HouseManager>();
            if (houseManager != null)
            {
                var houseData = houseManager.GetHouseData(originalPrefab);
                if (houseData != null)
                {
                    GameManager.Instance.totalCitizens += houseData.citizens;
                    if (houseData.energy > 0)
                    {
                        GameManager.Instance.totalEnergy += houseData.energy;
                    }
                }
            }
        }
    }
    public void SpawnStatPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>(); 
        if (canvas == null)
        {
            return;
        }
        spawnedStatPanel = Instantiate(statPanelPrefab, canvas.transform);
        currentActiveStatPanel = spawnedStatPanel;

        HouseActions houseActions = spawnedStatPanel.GetComponent<HouseActions>();
        if (houseActions != null)
        {
            houseActions.SetCurrentHouse(gameObject, currentHousePrice);
            houseActions.statPanel = spawnedStatPanel;
        }
        RectTransform rectTransform = spawnedStatPanel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0, 0.4f);
            rectTransform.anchorMax = new Vector2(0, 0.4f);
            rectTransform.pivot = new Vector2(0, 0.4f);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
        UpdateStatPanel();
    }
    public void UpdateStatPanel()
    {
        HouseManager houseManager = FindObjectOfType<HouseManager>();
        HouseManager.HouseData houseData = houseManager.GetHouseData(originalPrefab);
        StatsCard statsCard = spawnedStatPanel.GetComponent<StatsCard>();
        if (statsCard != null)
        {
            statsCard.UpdateStats(houseData.houseName, houseData.citizens, houseData.energy, houseData.income);
        }
    }
    public void UpdateHousePrice(int newPrice)
    {
        currentHousePrice = newPrice;
    }
    private void OnDestroy()
    {
        if (AssociatedCell != null)
        {
            AssociatedCell.isOccupied = false;
        }
        if (spawnedStatPanel != null && spawnedStatPanel == currentActiveStatPanel)
        {
            Destroy(currentActiveStatPanel);
            currentActiveStatPanel = null;
        }
        // Уничтожить стрелку, если она есть
        if (HouseArrowClick.currentArrow != null)
        {
            Destroy(HouseArrowClick.currentArrow);
            HouseArrowClick.currentArrow = null;
        }
    }
}
