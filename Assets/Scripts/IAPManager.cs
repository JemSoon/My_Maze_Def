using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour, IStoreListener
{
    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] GameObject noAdsButton;
    private IStoreController storeController;

    private string noADs = "noads_package_001";

    void Start()
    {
        InitIAP();
    }

    private void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(noADs, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        CheckNonConsumalbe(noADs);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화 실패 : " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message) 
    {
        Debug.Log("초기화 실패 : " + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("구매 실패");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //구매 성공 시 할 일
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("구매 성공 : " + product.definition.id);

        if(product.definition.id == noADs)
        {
            stateText.text = "광고 제거 구매 성공";
            noAdsButton.SetActive(true);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productID)
    {
        //버튼 이벤트 연결
        storeController.InitiatePurchase(productID);
    }

    private void CheckNonConsumalbe(string id)
    {
        //구매 영수증 확인
        var product = storeController.products.WithID(id);

        if(product != null)
        {
            //영수증이 있나요?
            bool isCheck = product.hasReceipt;
            //구매 여부에 따라 광고 제거 아이콘 활성/비활성
            noAdsButton.SetActive(isCheck);
        }
    }
}
