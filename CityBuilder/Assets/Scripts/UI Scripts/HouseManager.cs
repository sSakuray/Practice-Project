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

    [SerializeField] private List<HouseData> houses;
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
}
