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
            //�����Ҷ��� �ʵ� ��Ƽ����¸� �ε����� ����
            fieldsStartActive[i] = fields[i].gameObject.activeSelf;
            Debug.Log(fields[i].gameObject.activeSelf);
        }
    }

    public void ResetFields()
    {
        //�ʵ��� �ʱ���·�
        //�÷��̾�� �÷��̾�� �ʱ���� ����
        //���͵� ����Ŭ�������� �ʱ���� ���� ���
        //�׸��� �̰� GameManager���� ResetGame���� �Լ��� �ٰ��� ȣ��

        for(int i  = 0; i < fields.Length; ++i)
        {
            fields[i].gameObject.SetActive(fieldsStartActive[i]);
        }
    }
}
