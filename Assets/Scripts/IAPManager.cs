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
            //stateText.text = "광고 제거 구매 성공";
            purchasedAlready.SetActive(true);
            //구매한 상태 저장
            OutGameMoney.Inst.isPurchased = true;
            //생성된 배너 파괴
            OutGameMoney.Inst.admob.DestroyBannerView();
            //보너스 코인 +100
            OutGameMoney.Inst.money += 100;
            GameManager.Inst.goldAmountTmp.text = (OutGameMoney.Inst.money).ToString();
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
        Product product = storeController.products.WithID(id);

        if(product != null)
        {
            //영수증이 있나요?
            bool isPurchased = product.hasReceipt;
            //구매 여부에 따라 광고 제거 아이콘 활성/비활성
            purchasedAlready.SetActive(isPurchased);
            //해당 bool값을 통해 광고 게제 설정
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
