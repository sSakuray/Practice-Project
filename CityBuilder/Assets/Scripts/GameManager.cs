using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float money = 1000f;
    public LayerMask panelLayer; // Слой панелей травы

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SpawnHouse(GameObject housePrefab)
    {
        GameObject house = Instantiate(housePrefab);

        // Добавляем HousePlacer и настраиваем слой панелей
        HousePlacer housePlacer = house.AddComponent<HousePlacer>();
        housePlacer.panelLayer = panelLayer;
        housePlacer.StartPlacingHouse();
    }

    public void AddHouse(GameObject house)
    {
        house.SetActive(true);
    }
}
