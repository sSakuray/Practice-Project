using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public GameObject housePrefab;
    [SerializeField] public GameObject statPanelPrefab;
    [SerializeField] public Button buyButtonHouse2;        
    [SerializeField] public float price;      
    void Start()
    {
        buyButtonHouse2.onClick.AddListener(OnBuyClicked);
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
        buyButtonHouse2.targetGraphic.color = flashColor;
        yield return new WaitForSeconds(0.2f);
        buyButtonHouse2.targetGraphic.color = Color.white;
    }

}
