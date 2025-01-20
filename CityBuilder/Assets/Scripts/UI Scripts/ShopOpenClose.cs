using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOpenClose : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Button OpenShopButton;
    [SerializeField] private Button CloseShopButton;
    [SerializeField] private GameObject ShopPanel;
    private bool isShopOpen = false;

    private void Start()
    {
        OpenShopButton.onClick.AddListener(ToggleMenu);
        CloseShopButton.onClick.AddListener(ExitMenu);
        ShopPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        isShopOpen = !isShopOpen;
        ShopPanel.SetActive(true);
        anim.SetBool("ShopOpen", isShopOpen);
        OpenShopButton.gameObject.SetActive(false);
    }
    
    public void ExitMenu()
    {
        isShopOpen = false;
        anim.SetBool("ShopOpen", isShopOpen);
        OpenShopButton.gameObject.SetActive(true);
    }

}
