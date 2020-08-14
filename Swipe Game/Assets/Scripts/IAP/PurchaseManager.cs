using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    private int currentProductIndex;

    [Tooltip("Не многоразовые товары. Больше подходит для отключения рекламы и т.п.")]
    public string[] NC_PRODUCTS;
    [Tooltip("Многоразовые товары. Больше подходит для покупки игровой валюты и т.п.")]
    public string[] C_PRODUCTS;

    /// <summary>
    /// Событие, которое запускается при удачной покупке многоразового товара.
    /// </summary>
    public static event OnSuccessConsumable OnPurchaseConsumable;
    /// <summary>
    /// Событие, которое запускается при удачной покупке не многоразового товара.
    /// </summary>
    public static event OnSuccessNonConsumable OnPurchaseNonConsumable;
    /// <summary>
    /// Событие, которое запускается при неудачной покупке какого-либо товара.
    /// </summary>
    public static event OnFailedPurchase PurchaseFailed;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializePurchasing();
    }

    /// <summary>
    /// Проверить, куплен ли товар.
    /// </summary>
    /// <param name="id">Индекс товара в списке.</param>
    /// <returns></returns>
    public static bool CheckBuyState(string id)
    {
        Product product = m_StoreController.products.WithID(id);
        if (product.hasReceipt) { return true; }
        else { return false; }
    }

    public void InitializePurchasing()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (string s in C_PRODUCTS) builder.AddProduct(s, ProductType.Consumable);
        foreach (string s in NC_PRODUCTS) builder.AddProduct(s, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(C_PRODUCTS[index]);
    }

    public void BuyNonConsumable(int index)
    {
        currentProductIndex = index;
        BuyProductID(NC_PRODUCTS[index]);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Log.WriteLog((string.Format("Purchasing: '{0}'.", product.definition.id)), Log.LevelsOfLogs.INFO, "PurchaseManager");
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Log.WriteLog("BuyProductID: FAIL.", Log.LevelsOfLogs.ERROR, "PurchaseManager");
                OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
            }
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Log.WriteLog("OnInitialized: PASS.", Log.LevelsOfLogs.INFO, "PurchaseManager");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed:" + error);
        Log.WriteLog("OnInitializeFailed: " + error + ".", Log.LevelsOfLogs.ERROR, "PurchaseManager");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (C_PRODUCTS.Length > 0 && String.Equals(args.purchasedProduct.definition.id, C_PRODUCTS[currentProductIndex], StringComparison.Ordinal))
            OnSuccessC(args);
        else if (NC_PRODUCTS.Length > 0 && String.Equals(args.purchasedProduct.definition.id, NC_PRODUCTS[currentProductIndex], StringComparison.Ordinal))
            OnSuccessNC(args);
        else Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        return PurchaseProcessingResult.Complete;
    }

    public delegate void OnSuccessConsumable(PurchaseEventArgs args);
    protected virtual void OnSuccessC(PurchaseEventArgs args)
    {
        OnPurchaseConsumable?.Invoke(args);
        Debug.Log(C_PRODUCTS[currentProductIndex] + " Buyed!");
        Log.WriteLog(C_PRODUCTS[currentProductIndex] + " Buyed.", Log.LevelsOfLogs.INFO, "PurchaseManager");
    }
    public delegate void OnSuccessNonConsumable(PurchaseEventArgs args);
    protected virtual void OnSuccessNC(PurchaseEventArgs args)
    {
        OnPurchaseNonConsumable?.Invoke(args);
        Debug.Log(NC_PRODUCTS[currentProductIndex] + " Buyed!");
    }
    public delegate void OnFailedPurchase(Product product, PurchaseFailureReason failureReason);
    protected virtual void OnFailedP(Product product, PurchaseFailureReason failureReason)
    {
        PurchaseFailed?.Invoke(product, failureReason);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        OnFailedP(product, failureReason);
    }

    public static string GetLocalPrice(string productId)
    {
        return m_StoreController.products.WithID(productId).metadata.localizedPriceString;
    }
}