using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class IAPStoreManager : MonoBehaviour
{
    StoreController storeController;
    private static string FULL_GAME_CODE = "full_game";
    private bool isPurchasePending;

    public DemoVersionInitialization dvi;
    public StoreButtonPurchaseManager sbpm;

    void Start()
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("No Android NOR iPhone");
            sbpm.HideButton();
            gameObject.SetActive(false);
            return;
        }

        InitialiseStore();
    }

    async void InitialiseStore()
    {

        if (PlayerPrefs.GetInt("FullVersion", 0) == 0)
        {
            dvi.isDemo = true;
            dvi.UpdateDemoAndFullVersionFlags();
        }

        storeController = UnityIAPServices.StoreController();

        storeController.OnStoreDisconnected += OnStoreDisconnected;

        storeController.OnProductsFetched += OnProductsFetched;
        storeController.OnProductsFetchFailed += OnProductsFetchFailed;

        storeController.OnPurchasesFetched += OnPurchasesFetched;
        storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;

        storeController.OnPurchasePending += OnPurchasePending;
        storeController.OnPurchaseFailed += OnPurchaseFailed;
        storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        storeController.OnPurchaseDeferred += OnPurchaseDeferred;

        var catalogProvider = new CatalogProvider();
        catalogProvider.AddProduct("100_gold_coins", ProductType.Consumable);
        catalogProvider.AddProduct(FULL_GAME_CODE, ProductType.NonConsumable);

        try
        {
            await storeController.Connect();
            Debug.Log("Connected to store");
            catalogProvider.FetchProducts(list => storeController.FetchProducts(list)); 
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            sbpm.UpdateButtonGFX(); 
        }

    }

    private void OnStoreDisconnected(StoreConnectionFailureDescription failure)
    {
        Debug.Log("OnStoreDisconnected");
        sbpm.UpdateButtonGFX(); 
    }

    private void OnProductsFetched(List<Product> products)
    {
        Debug.Log("OnProductsFetched");
        foreach (Product product in products)
           Debug.Log("Product: " + product.definition);

        storeController.FetchPurchases();
    }

    // Invoked when an attempt to fetch products has failed or when a subset of products failed to be fetched.
    private void OnProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.Log("OnProductsFetchFailed");
        sbpm.UpdateButtonGFX(); 
    }

    /// Invoked when previous purchases are fetched. // this at te beginning to check of the FULL_GAME was purchased
    private void OnPurchasesFetched(Orders orders)
    {
        bool isDemo = true;
        Debug.Log("OnPurchasesFetched");
        foreach(ConfirmedOrder itm in orders.ConfirmedOrders)
        {
            Debug.Log("ConfirmedOrders:" + itm.Info);
            foreach(IPurchasedProductInfo ippi in itm.Info.PurchasedProductInfo)
            {
                if (ippi.productId == FULL_GAME_CODE)
                {
                    Debug.Log("Confirmed Order with product " + FULL_GAME_CODE + " found!");
                    isDemo = false;
                }
            }
        }
        foreach(DeferredOrder itm in orders.DeferredOrders)
            Debug.Log("DeferredOrder:" + itm.Info);
        foreach(PendingOrder itm in orders.PendingOrders)
            Debug.Log("PendingOrder:" + itm.Info);
        
        dvi.isDemo = isDemo;
        dvi.UpdateDemoAndFullVersionFlags();
        sbpm.UpdateButtonGFX(); 
    }

    /// Invoked when an attempt to fetch previous purchases has failed.
    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
    {
        Debug.Log("OnPurchasesFetchFailed");
        sbpm.UpdateButtonGFX(); 
    }


/*
    void CheckPurchase()
    {
        var product = storeController.products.WithID(FULL_GAME_CODE);

        if (product != null && product.hasReceipt)
        {
            //UnlockFullGame();
        }
    }
*/

    public void BuyFullGame()
    {
        if (isPurchasePending)
        {
            Debug.Log("Purchase pending");
            return;
        }
        isPurchasePending = true;
        storeController.PurchaseProduct(FULL_GAME_CODE);
    }

    async void OnPurchasePending(PendingOrder order)
    {
        Debug.Log("OnPurchasePending");
        // actually there are some check to do just copy from 6:58
        // confirm the purchase with:
        storeController.ConfirmPurchase(order);
    }

    void OnPurchaseConfirmed(Order order)
    {
        Debug.Log("OnPurchaseConfirmed");
        isPurchasePending = false;
        if (order is FailedOrder failedOrder) // double ceck?
        {
            // write something about why the order failed in failedOrder.FailedReason
            return;
        }

        // some messagge on the product you managed to pourchase? 7:56
        UnlockFullGame();
    }

    void OnPurchaseFailed(FailedOrder failed)
    {
        Debug.Log("OnPurchaseFailed");
        isPurchasePending = false;
        // say something, it's the same as when I close the store, so it's quite normal
    }

    void OnPurchaseDeferred(DeferredOrder deferredOrder)
    {
        Debug.Log("DeferredOrder");
    }

    private void UnlockFullGame()
    {
        dvi.isDemo = false;
        dvi.UpdateDemoAndFullVersionFlags();
        SceneManager.LoadScene("MainMenu");
    }

}
