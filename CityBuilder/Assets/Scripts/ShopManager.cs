using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private Button inShopButton;
    [SerializeField] private Button ExitShopButton;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] public GameObject housePrefab;
    [SerializeField] public Button buyButton;        
    [SerializeField] public Image buttonImage;        
    [SerializeField] public TextMeshProUGUI priceText;        
    [SerializeField] public float price;              
    void Start()
    {
        shopPanel.SetActive(false);
        inShopButton.onClick.AddListener(OnShop);
        ExitShopButton.onClick.AddListener(OnExitShop);
        buyButton.onClick.AddListener(OnBuyClicked);
        priceText.text = "$" + price.ToString(); 
    }

    private void OnShop()
    {
        shopPanel.SetActive(true);
    }

    private void OnExitShop()
    {
        shopPanel.SetActive(false);
    }

    private void OnBuyClicked()
    {
        if (GameManager.Instance.money >= price)
        {
            StartCoroutine(MakeButtonFlash(Color.green));
            GameManager.Instance.money -= price; 
            GameManager.Instance.SpawnHouse(housePrefab);
        }
        else
        {
            StartCoroutine(MakeButtonFlash(Color.red));
        }
    }

    private IEnumerator MakeButtonFlash(Color flashColor)
    {
        buttonImage.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        buttonImage.color = Color.white;
    }

}
