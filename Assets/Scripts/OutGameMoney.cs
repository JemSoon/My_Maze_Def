using System;
using UnityEngine;

[Serializable]
public class OutGameMoney : MonoBehaviour
{
    public static OutGameMoney Inst { get; private set; }

    [Header("Out Game Goods")]
    public float pencilCoolTime;
    public int money;
    public int pencilLevel;
    public int fireLevel;
    [Header("Stage Scene Index")]
    public int stageLevel;

    private const string MoneyKey = "Money";

    private const string PencilCoolTimeKey = "PencilCoolTime";
    private const string PencilLevelKey = "PencilLevel";

    private const string FireRateLevelKey = "FireRateLevel";

    private const string StageLevelKey = "StageLevel";

    public PencilItem pencilItem;
    public FireRateItem fireRateItem;

    public bool isSceneLoaded;
    public AsyncOperation asyncLoad;
    public MAX_AD MAX_AD;

    private void Awake()
    {
        //Debug.Log("어웨이크 호출");
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

        Inst.pencilItem = Inst.GetComponent<PencilItem>();
        Inst.fireRateItem = Inst.GetComponent<FireRateItem>();

        if (!PlayerPrefs.HasKey(PencilCoolTimeKey) && !PlayerPrefs.HasKey(MoneyKey) && !PlayerPrefs.HasKey(PencilLevelKey) && !PlayerPrefs.HasKey(FireRateLevelKey) && !PlayerPrefs.HasKey(StageLevelKey))
        {
            //맨 처음 세이브 없으면 기본값으로 초기화
            Inst.money = 0;
            Inst.pencilCoolTime = Inst.pencilItem.oneForSeconds[0];
            Inst.pencilLevel = 0;
            Inst.fireLevel = 0;
            Inst.stageLevel = 0;
        }

        else
        {
            Inst.pencilCoolTime = Inst.pencilItem.oneForSeconds[PlayerPrefs.GetInt(PencilLevelKey, pencilLevel)];
            Inst.money = PlayerPrefs.GetInt(MoneyKey, money);
            Inst.pencilLevel = PlayerPrefs.GetInt(PencilLevelKey, pencilLevel);
            Inst.fireLevel = PlayerPrefs.GetInt(FireRateLevelKey);
            Inst.stageLevel = PlayerPrefs.GetInt(StageLevelKey);
        }
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetFloat(PencilCoolTimeKey, pencilCoolTime);
        PlayerPrefs.SetInt(MoneyKey, money);
        PlayerPrefs.SetInt(PencilLevelKey, pencilLevel);
        PlayerPrefs.SetInt(FireRateLevelKey, fireLevel);

        PlayerPrefs.Save();
    }

    public void DeleteInfo()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SaveStage(int SceneIndex)
    {
        Debug.Log("저장될 씬 인덱스 : " + SceneIndex);
        stageLevel = SceneIndex;
        PlayerPrefs.SetInt(StageLevelKey, stageLevel);
    }

    public void SetAsyncLoad(AsyncOperation async)
    {
        asyncLoad = async;
    }

    void HandleRewardedAdResult(bool isSuccess)
    {
        //다 봤을때 true가 되야하는데
        //그렇다면 얘를 어떻게 해야하지?
        //일단 true로 해서 광고 띄우기는 성공
        //isSuccess = true;
    }

    public void ADButtonClick()
    {
        MAX_AD.ShowRewardedAd(success =>
        {
            if (success)
            {
                StartCoroutine(GameManager.Inst.gold2Count(Player.Inst.goldCount));//코인 두배 보상!
            }
            else
            {
                Debug.Log("Rewarded ad was not successfully watched.");
            }
        });

        
    }
}
