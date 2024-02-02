using System;
using UnityEngine;

[Serializable]
public class OutGameMoney : MonoBehaviour
{
    public static OutGameMoney Inst;

    [Header("�ƿ����� ��ȭ")]
    public float pencilCoolTime;
    public int money;
    public int level;
   

    private void Awake()
    {
        Inst = this;
        Inst.pencilCoolTime = 2.0f;
    }
}
