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
    public int level;

    private const string PencilCoolTimeKey = "PencilCoolTime";
    private const string MoneyKey = "Money";
    private const string LevelKey = "Level";

    public PencilItem pencilItem;

    private void Awake()
    {
        Inst = this;
        Inst.pencilItem = GetComponent<PencilItem>();

        if (!PlayerPrefs.HasKey(PencilCoolTimeKey) && !PlayerPrefs.HasKey(MoneyKey) && !PlayerPrefs.HasKey(LevelKey))
        {
            //맨 처음 세이브 없으면 기본값으로 초기화
            Inst.pencilCoolTime = pencilItem.oneForSeconds[0];
            Inst.money = 0;
            Inst.level = 0;
        }

        else
        {
            Inst.pencilCoolTime = pencilItem.oneForSeconds[PlayerPrefs.GetInt(LevelKey, level)];
            Inst.money = PlayerPrefs.GetInt(MoneyKey, money);
            Inst.level = PlayerPrefs.GetInt(LevelKey, level);
        }
        //Inst.pencilCoolTime = 2.0f;
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetFloat(PencilCoolTimeKey, pencilCoolTime);
        PlayerPrefs.SetInt(MoneyKey, money);
        PlayerPrefs.SetInt(LevelKey, level);

        PlayerPrefs.Save();
    }

    public void DeleteInfo()
    {
        PlayerPrefs.DeleteAll();
    }
}
