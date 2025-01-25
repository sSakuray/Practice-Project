using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int money;
    public int totalCitizens;
    public int totalEnergy;
    public LayerMask panelLayer;
    public bool isHouseBeingPlaced = false;
    [SerializeField] private TextMeshProUGUI quantityMoneyText;
    [SerializeField] private TextMeshProUGUI citizensText;
    [SerializeField] private TextMeshProUGUI energyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        UpdateGameStats();
    }

    private void UpdateGameStats()
    {
        quantityMoneyText.text = $"{money}";
        if (citizensText != null) citizensText.text = $"{totalCitizens}";
        if (energyText != null) energyText.text = $"{totalEnergy}";
    }

    public void SpawnHouse(GameObject housePrefab)
    {
        if (isHouseBeingPlaced)
        {
            return;
        }
        GameObject house = Instantiate(housePrefab);

        HousePlacer housePlacer = house.AddComponent<HousePlacer>();
        housePlacer.panelLayer = panelLayer;
        housePlacer.StartPlacingHouse();

        isHouseBeingPlaced = true;
        SpawnStats spawnStats = house.GetComponent<SpawnStats>();
        spawnStats.Initialize(housePrefab);
    }

    public void HousePlaced()
    {
        isHouseBeingPlaced = false;
    }

    public void AddHouse(GameObject house)
    {
        house.SetActive(true);
    }

    public void UpdateStatsOnHousePlaced(GameObject housePrefab)
    {
        HouseManager houseManager = FindObjectOfType<HouseManager>();
        if (houseManager != null)
        {
            var houseData = houseManager.GetHouseData(housePrefab);
            if (houseData != null)
            {
                totalCitizens += houseData.citizens;
                totalEnergy += houseData.energy;
            }
        }
    }

    public void UpdateStatsOnHouseRemoved(GameObject housePrefab)
    {
        HouseManager houseManager = FindObjectOfType<HouseManager>();
        if (houseManager != null)
        {
            var houseData = houseManager.GetHouseData(housePrefab);
            if (houseData != null)
            {
                totalCitizens -= houseData.citizens;
                totalEnergy -= houseData.energy;
            }
        }
    }
}
