using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class HouseData
    {
        public GameObject housePrefab; 
        public Button buyButton;
        public int price; 
    }
    [SerializeField] public List<HouseData> houses; 
    public void Start()
    {
        foreach (var houseData in houses)
        {
            houseData.buyButton.onClick.AddListener(() => OnBuyClicked(houseData));
        }
    }
    public int GetHousePrice(GameObject housePrefab)
    {
        foreach (var houseData in houses)
        {
            if (houseData.housePrefab == housePrefab)
            {
                return houseData.price;
            }
        }
        return 0;
    }
    private void OnBuyClicked(HouseData houseData)
    {
        if (GameManager.Instance.isHouseBeingPlaced)
        {
            return;
        }

        var houseManager = FindObjectOfType<HouseManager>();
        if (houseManager == null) return;

        if (houseManager.CanPurchaseBuilding(houseData.housePrefab))
        {
            StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.green));
            GameManager.Instance.money -= houseData.price;

            var requirement = houseManager.GetHouseRequirement(houseData.housePrefab);
            GameManager.Instance.totalCitizens -= requirement.requiredCitizens;
            GameManager.Instance.totalEnergy -= requirement.requiredEnergy;

            GameManager.Instance.SpawnHouse(houseData.housePrefab);
        }
        else
        {
            StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.red));
        }
    }
    private IEnumerator MakeButtonFlash(Button button, Color flashColor)
    {
        button.targetGraphic.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        button.targetGraphic.color = Color.white;
    }
}