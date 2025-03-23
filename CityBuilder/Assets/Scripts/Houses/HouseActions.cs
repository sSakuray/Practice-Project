using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseActions : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;     
    [SerializeField] private Button rotateButton;      
    [SerializeField] private Button deleteButton;       
    [SerializeField] public GameObject statPanel;      
    [SerializeField] private TextMeshProUGUI priceText;  
    
    [System.Serializable]
    public class UpgradeOption
    {
        public GameObject currentPrefab;
        public GameObject upgradedPrefab;
        public int upgradeCost;   
    }

    [SerializeField] private List<UpgradeOption> upgradeOptions;
    private GameObject currentHouse;   
    private int houseCost;   
    private int totalSpentOnUpgrades = 0;  
    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeHouse);
        rotateButton.onClick.AddListener(RotateHouse);
        deleteButton.onClick.AddListener(DeleteHouse);
    }

    public void SetCurrentHouse(GameObject house, int cost)
    {
        currentHouse = house;
        houseCost = cost;
        UpdateUpgradePrice();
    }
    private void UpdateUpgradePrice()
    {
        if (priceText == null || currentHouse == null)
        {
            return;
        }

        foreach (var option in upgradeOptions)
        {
            if (option.currentPrefab.name == currentHouse.name.Replace("(Clone)", "").Trim())
            {
                priceText.text = $"{option.upgradeCost} $";
                return;
            }
        }

        priceText.text = "N/A";
    }
    public void UpgradeHouse()
    {
        foreach (var option in upgradeOptions)
        {
            if (option.currentPrefab.name == currentHouse.name.Replace("(Clone)", "").Trim())
            {
                var houseManager = FindObjectOfType<HouseManager>();
                if (houseManager == null) return;

                // Проверяем наличие ресурсов
                if (!houseManager.CanUpgradeBuilding(currentHouse, option.upgradedPrefab))
                {
                    Debug.LogWarning("Нехватает ресурсов для апгрейда!");
                    return;
                }
                
                if (GameManager.Instance.money >= option.upgradeCost)
                {
                    Vector3 position = currentHouse.transform.position;
                    Quaternion rotation = currentHouse.transform.rotation;

                    SpawnStats oldSpawnStats = currentHouse.GetComponent<SpawnStats>();
                    GameObject originalPrefab = null;
                    GameObject upgradedFromPrefab = null;
                    GridCell associatedCell = null; 
                    
                    if (oldSpawnStats != null)
                    {
                        originalPrefab = option.upgradedPrefab;
                        upgradedFromPrefab = option.currentPrefab;
                        associatedCell = oldSpawnStats.AssociatedCell; 
                        
                        if (associatedCell != null)
                        {
                            GridRegistry.SetOccupied(associatedCell.transform.position, true);
                            associatedCell.SetOccupied(true);
                            Debug.Log($"Апгрейд здания: позиция {associatedCell.transform.position} помечена как занятая");
                        }
                    }
                    
                    var upgradedRequirement = houseManager.GetHouseRequirement(option.upgradedPrefab);
                    GameManager.Instance.totalCitizens -= upgradedRequirement.requiredCitizens;
                    GameManager.Instance.totalEnergy -= upgradedRequirement.requiredEnergy;
                    GameManager.Instance.money -= option.upgradeCost;
                    totalSpentOnUpgrades += option.upgradeCost;
                    
                    GameManager.Instance.UpdateIncomeOnHouseUpgraded(option.currentPrefab, option.upgradedPrefab);
                    
                    Vector3 cellPosition = associatedCell != null ? associatedCell.transform.position : position;
                    
                    Destroy(currentHouse);

                    GameObject upgradedHouse = Instantiate(option.upgradedPrefab, position, rotation);
                    currentHouse = upgradedHouse;

                    UpdateUpgradePrice();

                    SpawnStats spawnStats = upgradedHouse.GetComponent<SpawnStats>();
                    if (spawnStats != null)
                    {
                        spawnStats.Initialize(originalPrefab, upgradedFromPrefab);
                        
                        spawnStats.AssociatedCell = associatedCell;
                        if (associatedCell != null)
                        {
                            GridRegistry.SetOccupied(cellPosition, true);
                            associatedCell.SetOccupied(true);
                            Debug.Log($"После апгрейда: позиция {cellPosition} снова помечена как занятая");
                        }
                          
                        spawnStats.PlaceHouse();  

                        spawnStats.UpdateHousePrice(houseCost + totalSpentOnUpgrades);
                        Destroy(statPanel);
                        spawnStats.SpawnStatPanel();
                    }
                    return;
                }
                else
                {
                    Debug.LogWarning("Недостаточно денег для апгрейда!");
                }
            }
        }
    }

    public void RotateHouse()
    {
        currentHouse.transform.RotateAround(currentHouse.transform.position, Vector3.up, 90);
    }
    public void DeleteHouse()
    {
        var houseManager = FindObjectOfType<HouseManager>();
        if (houseManager == null) return;

        SpawnStats spawnStats = currentHouse.GetComponent<SpawnStats>();
        if (spawnStats == null || !houseManager.CanSellBuilding(spawnStats.OriginalPrefab))
        {
            Debug.LogWarning("Нельзя удалить здание - это приведет к отрицательным значениям ресурсов!");
            return;
        }

        GridCell associatedCell = spawnStats.AssociatedCell;
        if (associatedCell != null)
        {
            associatedCell.SetOccupied(false);
        }
        else
        {
            GridRegistry.SetOccupied(currentHouse.transform.position, false);
        }

        GameManager.Instance.UpdateStatsOnHouseRemoved(spawnStats.OriginalPrefab);
        GameManager.Instance.money += (int)(houseCost * 0.7);
        Destroy(statPanel);
        Destroy(currentHouse);
        currentHouse = null;
    }
}
