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
        Debug.Log("�ʱ�ȭ ���� : " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message) 
    {
        Debug.Log("�ʱ�ȭ ���� : " + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("���� ����");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //���� ���� �� �� ��
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("���� ���� : " + product.definition.id);

        if(product.definition.id == noADs)
        {
            stateText.text = "���� ���� ���� ����";
            noAdsButton.SetActive(true);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productID)
    {
        //��ư �̺�Ʈ ����
        storeController.InitiatePurchase(productID);
    }

    private void CheckNonConsumalbe(string id)
    {
        //���� ������ Ȯ��
        var product = storeController.products.WithID(id);

        if(product != null)
        {
            //�������� �ֳ���?
            bool isCheck = product.hasReceipt;
            //���� ���ο� ���� ���� ���� ������ Ȱ��/��Ȱ��
            noAdsButton.SetActive(isCheck);
        }
    }
}
