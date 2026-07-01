using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject purchaseFailedMessage;

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
        {
            Debug.Log("Product: " + product.definition);
            if (product.definition.id == FULL_GAME_CODE)
            {
                sbpm.price = product.metadata.localizedPriceString;
                sbpm.UpdateButtonGFX();
            }
        }

        storeController.FetchPurchases();
    }

    private void OnProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.Log("OnProductsFetchFailed");
        sbpm.UpdateButtonGFX(); 
    }

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
        if (isDemo)
        {
            StartCoroutine(FetchPurchasesWithDelay());
        }
    }

    IEnumerator FetchPurchasesWithDelay()
    {
        yield return new WaitForSeconds(1);
        storeController.FetchPurchases();
    }

    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
    {
        Debug.Log("OnPurchasesFetchFailed");
        sbpm.UpdateButtonGFX(); 
    }


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
        // here I skip the checking on the store to see if the product is there, since when the game boots check for the product with the full game id
        storeController.ConfirmPurchase(order);
    }

    void OnPurchaseConfirmed(Order order)
    {
        Debug.Log("OnPurchaseConfirmed");
        isPurchasePending = false;
        if (order is FailedOrder failedOrder)
        {
            purchaseFailedMessage.SetActive(true);
            Debug.Log("Faild order: " + failedOrder.FailureReason.ToString());
            return;
        }

        Debug.Log("Purchase Confirmed: " + order.CartOrdered.Items().FirstOrDefault().ToString());
        UnlockFullGame();
    }

    void OnPurchaseFailed(FailedOrder failed)
    {
        Debug.Log("OnPurchaseFailed: " + failed.FailureReason.ToString());
        isPurchasePending = false;
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
