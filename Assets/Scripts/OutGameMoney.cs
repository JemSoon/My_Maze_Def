using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OutGameMoney : MonoBehaviour
{
    public static OutGameMoney Inst { get; private set; }

    [Header("아웃게임 재화")]
    public float pencilCoolTime;
    public int money;
    public int pencilLevel;
    public int fireLevel;

    private const string MoneyKey = "Money";

    private const string PencilCoolTimeKey = "PencilCoolTime";
    private const string PencilLevelKey = "PencilLevel";

    private const string FireRateTimeKey = "FireRateTime";
    private const string FireRateLevelKey = "FireRateLevel";

    public PencilItem pencilItem;
    public FireRateItem fireRateItem;

    private void Awake()
    {
        Inst = this;
        Inst.pencilItem = GetComponent<PencilItem>();
        Inst.fireRateItem = GetComponent<FireRateItem>();

        if (!PlayerPrefs.HasKey(PencilCoolTimeKey) && !PlayerPrefs.HasKey(MoneyKey) && !PlayerPrefs.HasKey(PencilLevelKey) && !PlayerPrefs.HasKey(FireRateTimeKey) && !PlayerPrefs.HasKey(FireRateLevelKey))
        {
            //맨 처음 세이브 없으면 기본값으로 초기화
            Inst.money = 0;
            Inst.pencilCoolTime = pencilItem.oneForSeconds[0];
            Inst.pencilLevel = 0;
            Inst.fireLevel = 0;
        }

        else
        {
            Inst.pencilCoolTime = pencilItem.oneForSeconds[PlayerPrefs.GetInt(PencilLevelKey, pencilLevel)];
            Inst.money = PlayerPrefs.GetInt(MoneyKey, money);
            Inst.pencilLevel = PlayerPrefs.GetInt(PencilLevelKey, pencilLevel);
            Inst.fireLevel = PlayerPrefs.GetInt(FireRateLevelKey);
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
}
