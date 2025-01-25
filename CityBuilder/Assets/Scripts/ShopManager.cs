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
        if (GameManager.Instance.money < houseData.price)
        {
            StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.red));
            return;
        }

        if (GameManager.Instance.isHouseBeingPlaced)
        {
            return;
        }

        StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.green));
        GameManager.Instance.money -= houseData.price;
        GameManager.Instance.SpawnHouse(houseData.housePrefab);
    }
    private IEnumerator MakeButtonFlash(Button button, Color flashColor)
    {
        button.targetGraphic.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        button.targetGraphic.color = Color.white;
    }
}