using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float money = 1000f;
    public LayerMask panelLayer;
    
    public bool isHouseBeingPlaced = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
}
