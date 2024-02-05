using System;
using UnityEngine;

[Serializable]
public class OutGameMoney : MonoBehaviour
{
    public static OutGameMoney Inst { get; private set; }

    [Header("�ƿ����� ��ȭ")]
    public float pencilCoolTime;
    public int money;
    public int level;

    private const string PencilCoolTimeKey = "PencilCoolTime";
    private const string MoneyKey = "Money";
    private const string LevelKey = "Level";


    private void Awake()
    {
        Inst = this;

        if (!PlayerPrefs.HasKey(PencilCoolTimeKey) && !PlayerPrefs.HasKey(MoneyKey) && !PlayerPrefs.HasKey(LevelKey))
        {
            //�� ó�� ���̺� ������ �⺻������ �ʱ�ȭ
            Inst.pencilCoolTime = 2.0f;
            Inst.money = 0;
            Inst.level = 1;
        }

        else
        {
            Inst.pencilCoolTime = PlayerPrefs.GetFloat(PencilCoolTimeKey, pencilCoolTime);
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
}
