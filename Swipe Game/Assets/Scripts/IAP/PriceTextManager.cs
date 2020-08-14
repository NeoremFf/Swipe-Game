using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceTextManager : MonoBehaviour
{
    [SerializeField] PurchaseManager items;
    [SerializeField] private bool isConsumable;
    [SerializeField] private int id;

    private string priceText;

    private void Start()
    {
        if (isConsumable)
        {
            priceText = PurchaseManager.GetLocalPrice(items.C_PRODUCTS[id]);
        }
        else
        {
            priceText = PurchaseManager.GetLocalPrice(items.NC_PRODUCTS[id]);
        }

        GetComponent<Text>().text = priceText;
    }
}

