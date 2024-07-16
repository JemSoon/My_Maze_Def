using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Inst;
    [SerializeField] GameObject purchasedAlready;
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    public GameObject purchaseUI;

    private string noADs = "mazedefense_no_ads_package";

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Inst != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        //if(storeController == null)
        {
            InitIAP();
        }
        //else
        {
            CheckPurchaseStatus();
        }
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void InitIAP()
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
            //stateText.text = "���� ���� ���� ����";
            purchasedAlready.SetActive(true);
            //������ ���� ����
            OutGameMoney.Inst.isPurchased = true;
            //������ ��� �ı�
            OutGameMoney.Inst.admob.DestroyBannerView();
            //���ʽ� ���� +100
            OutGameMoney.Inst.money += 100;
            GameManager.Inst.goldAmountTmp.text = (OutGameMoney.Inst.money).ToString();
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
        Product product = storeController.products.WithID(id);

        if(product != null)
        {
            //�������� �ֳ���?
            bool isPurchased = product.hasReceipt;
            //���� ���ο� ���� ���� ���� ������ Ȱ��/��Ȱ��
            purchasedAlready.SetActive(isPurchased);
            //�ش� bool���� ���� ���� ���� ����
            OutGameMoney.Inst.isPurchased = isPurchased;
        }
    }

    public void ClosePurchaseUI()
    {
        //if(Inst.isGameOver)
        {
            GameManager.Inst.startMenu.SetActive(true);
            GameManager.Inst.upgradePencilMenu.SetActive(true);
            GameManager.Inst.upgradeFireMenu.SetActive(true);
            GameManager.Inst.upgradeBulletLevelMenu.SetActive(true);
        }
        //else
        //{
        //    startMenu.SetActive(false);
        //    upgradePencilMenu.SetActive(false);
        //    upgradeFireMenu.SetActive(false);
        //    upgradeBulletLevelMenu.SetActive(false);
        //}

        IAPManager.Inst.purchaseUI.SetActive(false);
    }

    private void CheckPurchaseStatus()
    {
        if (storeController != null)
        {
            Product product = storeController.products.WithID(noADs);
            if (product != null && product.hasReceipt)
            {
                OutGameMoney.Inst.isPurchased = true;
            }
            else
            {
                OutGameMoney.Inst.isPurchased = false;
            }
        }
    }
}
