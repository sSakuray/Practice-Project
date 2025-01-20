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
                Debug.LogError("HouseManager not found!");
                return;
            }

            HouseManager.HouseData houseData = houseManager.GetHouseData(originalPrefab);
            if (houseData !=null)
            {

                Debug.Log($"Полученные данные дома: Жители = {houseData.citizens}, Энергия = {houseData.energy}, Доход = {houseData.income}");
                StatsCard statsCard = spawnedStatPanel.GetComponent<StatsCard>();
                if (statsCard != null)
                {
                    statsCard.UpdateStats(houseData.citizens, houseData.energy, houseData.income);
                }
                else
                {
                    Debug.LogError("StatsCard not found!");
                }
            
            }
            else
            {
                Debug.LogWarning($"Данные для дома {gameObject.name} не найдены!");
            }
        }
    }
}