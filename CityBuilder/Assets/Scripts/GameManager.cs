using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    public float money = 1000f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void SpawnHouse(GameObject housePrefab)
    {
        GameObject house = Instantiate(housePrefab);
        house.AddComponent<HousePlacer>();
        house.GetComponent<HousePlacer>().StartPlacingHouse();
    }

    public void AddHouse(GameObject house)
    {
        house.SetActive(true);
    }
}
