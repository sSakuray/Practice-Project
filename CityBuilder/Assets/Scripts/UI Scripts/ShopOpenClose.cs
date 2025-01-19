using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpenClose : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator anim;

    private bool isShopOpen = false;

    public void ToggleMenu()
    {
        isShopOpen = !isShopOpen;
        anim.SetBool("ShopOpen", isShopOpen);
    }

}
