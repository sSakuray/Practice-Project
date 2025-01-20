using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class HouseData
    {
        public GameObject housePrefab; // Префаб дома
        public Button buyButton; // Кнопка покупки
        public float price; // Цена дома
    }

    [SerializeField] private List<HouseData> houses; // Список всех домов

    private void Start()
    {
        // Настраиваем каждую кнопку
        foreach (var houseData in houses)
        {
            houseData.buyButton.onClick.AddListener(() => OnBuyClicked(houseData));
        }
    }

    private void OnBuyClicked(HouseData houseData)
    {
        if (GameManager.Instance.money < houseData.price)
        {
            StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.red));
            Debug.Log($"Не хватает денег на дом {houseData.housePrefab?.name}. Цена: {houseData.price}, Баланс: {GameManager.Instance.money}");
            return;
        }

        if (GameManager.Instance.isHouseBeingPlaced)
        {
            Debug.LogWarning("Нельзя купить новый дом, пока текущий не размещен!");
            return;
        }

        StartCoroutine(MakeButtonFlash(houseData.buyButton, Color.green));
        GameManager.Instance.money -= houseData.price;
        GameManager.Instance.SpawnHouse(houseData.housePrefab);
        Debug.Log($"Дом {houseData.housePrefab.name} куплен!");
    }

    private IEnumerator MakeButtonFlash(Button button, Color flashColor)
    {
        button.targetGraphic.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        button.targetGraphic.color = Color.white;
    }
}