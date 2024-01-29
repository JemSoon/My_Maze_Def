using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Inst;
    public GameObject[] fields;
    bool[] fieldsStartActive;

    private void Awake()
    {
        Inst = this;
        Inst.SaveBeginActive();
    }

    public void SaveBeginActive()
    {
        fieldsStartActive = new bool[fields.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            //시작할때의 필드 액티브상태를 인덱스로 저장
            fieldsStartActive[i] = fields[i].gameObject.activeSelf;
            Debug.Log(fields[i].gameObject.activeSelf);
        }
    }

    public void ResetFields()
    {
        //필드의 초기상태로
        //플레이어는 플레이어에서 초기상태 저장
        //몬스터도 몬스터클래스에서 초기상태 저장 등등
        //그리고 이걸 GameManager에서 ResetGame에서 함수를 다같이 호출

        for(int i  = 0; i < fields.Length; ++i)
        {
            fields[i].gameObject.SetActive(fieldsStartActive[i]);
        }
    }
}
