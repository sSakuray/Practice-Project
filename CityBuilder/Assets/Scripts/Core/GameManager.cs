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
    public int totalIncome = 100; // Base income amount
    public LayerMask panelLayer;
    public bool isHouseBeingPlaced = false;
    [SerializeField] private TextMeshProUGUI quantityMoneyText;
    [SerializeField] private TextMeshProUGUI citizensText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private int IncomeWaitInSeconds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(AccumulateMoney());
    }
    private void Update()
    {
        UpdateGameStats();
    }
    private void UpdateGameStats()
    {
        quantityMoneyText.text = ConvertMoney(money);
        if (citizensText != null) citizensText.text = $"{totalCitizens}";
        if (energyText != null) energyText.text = $"{totalEnergy}";
    }

    public string ConvertMoney(int amount)
    {
        if (amount >= 1000000) return (amount / 1000000f).ToString("0.#") + "M";
        if (amount >= 100000) return (amount / 1000f).ToString("0.#") + "k";
        if (amount >= 1000) return (amount / 1000f).ToString("0.#") + "k";
        return amount.ToString();
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
                if (houseData.energy > 0)
                {
                    totalEnergy += houseData.energy;
                }
                // Add income from the placed house
                totalIncome += houseData.income;
            }
        }
    }

    public void UpdateStatsOnHouseRemoved(GameObject housePrefab)
    {
        HouseManager houseManager = FindObjectOfType<HouseManager>();
        if (houseManager != null)
        {
            var houseData = houseManager.GetHouseData(housePrefab);
            var houseRequirement = houseManager.GetHouseRequirement(housePrefab);
            SpawnStats spawnStats = FindObjectOfType<SpawnStats>();
            bool isUpgraded = spawnStats != null && spawnStats.IsUpgraded;
            
            if (houseData != null && houseRequirement != null)
            {
                if (isUpgraded && spawnStats.UpgradedFromPrefab != null)
                {
                    var originalHouseData = houseManager.GetHouseData(spawnStats.UpgradedFromPrefab);
                    if (originalHouseData != null)
                    {
                        totalCitizens -= (originalHouseData.citizens + houseData.citizens);
                        // Remove income from both the upgraded and original house
                        totalIncome -= (originalHouseData.income + houseData.income);
                    }
                }
                else
                {
                    totalCitizens -= houseData.citizens;
                    // Remove income from the house
                    totalIncome -= houseData.income;
                }
                totalCitizens += houseRequirement.requiredCitizens;
                totalEnergy -= houseData.energy;
                totalEnergy += houseRequirement.requiredEnergy;
                if (isUpgraded && spawnStats.UpgradedFromPrefab != null)
                {
                    var upgradeRequirement = houseManager.GetHouseRequirement(spawnStats.UpgradedFromPrefab);
                    if (upgradeRequirement != null)
                    {
                        totalEnergy += upgradeRequirement.requiredEnergy;
                        totalCitizens += upgradeRequirement.requiredCitizens;
                    }
                }
            }
        }
    }

    public void UpdateIncomeOnHouseUpgraded(GameObject originalPrefab, GameObject upgradedPrefab)
    {
        HouseManager houseManager = FindObjectOfType<HouseManager>();
        if (houseManager != null)
        {
            var originalHouseData = houseManager.GetHouseData(originalPrefab);
            var upgradedHouseData = houseManager.GetHouseData(upgradedPrefab);
            
            if (originalHouseData != null && upgradedHouseData != null)
            {
                // Remove original house income and add upgraded house income
                totalIncome -= originalHouseData.income;
                totalIncome += upgradedHouseData.income;
            }
        }
    }

    private IEnumerator AccumulateMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(IncomeWaitInSeconds);
            money += totalIncome; // Now using totalIncome instead of fixed value
        }
    }
}
