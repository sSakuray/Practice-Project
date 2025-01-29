using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [System.Serializable]
    public class HouseData
    {
        public GameObject housePrefab;
        public string houseName;
        public int citizens;
        public int energy;   
        public int income;  
    }
    [System.Serializable]
    public class HouseRequirement
    {
        public GameObject housePrefab;
        public int requiredCitizens; 
        public int requiredEnergy;   
    }
    [SerializeField] private List<HouseData> houses;
    [SerializeField] private List<HouseRequirement> requirements;
    public HouseData GetHouseData(GameObject housePrefab)
    {
        foreach (var house in houses)
        {
            if (house.housePrefab == housePrefab)
            {
                return house;
            }
        }
        return null;
    }
    public HouseRequirement GetHouseRequirement(GameObject housePrefab)
    {
        foreach (var requirement in requirements)
        {
            if (requirement.housePrefab == housePrefab)
            {
                return requirement;
            }
        }
        return null;
    }

    public bool CanPurchaseBuilding(GameObject housePrefab)
    {
        var requirement = GetHouseRequirement(housePrefab);
        if (requirement == null) return false;

        return GameManager.Instance.totalCitizens >= requirement.requiredCitizens &&
               GameManager.Instance.totalEnergy >= requirement.requiredEnergy;
    }

    public bool CanSellBuilding(GameObject housePrefab)
    {
        var houseData = GetHouseData(housePrefab);
        if (houseData == null) return false;


        return GameManager.Instance.totalCitizens - houseData.citizens >= 0 &&
               GameManager.Instance.totalEnergy - houseData.energy >= 0;
    }

    public bool CanUpgradeBuilding(GameObject currentPrefab, GameObject upgradedPrefab)
    {
        var upgradedRequirement = requirements.Find(req => req.housePrefab == upgradedPrefab);
        var upgradedData = GetHouseData(upgradedPrefab);
        if (upgradedRequirement == null || upgradedData == null) return false;

        if (GameManager.Instance.totalCitizens >= upgradedRequirement.requiredCitizens &&
            GameManager.Instance.totalEnergy >= upgradedRequirement.requiredEnergy)
        {
            GameManager.Instance.totalEnergy -= upgradedRequirement.requiredEnergy;
            return true;
        }

        return false;
    }
}
