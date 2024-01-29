using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public BoxCollider2D col;
    public TextMeshProUGUI text;
    public WaitForSeconds time = new WaitForSeconds(1);
    public int needKey;
    int beginNeedKey;

    [Header("다음 필드")]
    public GameObject nextField;
    public GameObject[] offCollision;

    private void Awake()
    {
        beginNeedKey = needKey;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Inst.player.keyCount == 0) { return; }

        if (collision.gameObject.CompareTag("Player"))
        {
            //0.1초마다 플레이어의 열쇠 호로록
            GameManager.Inst.player.keyCount--;
            needKey--;
            text.SetText(needKey.ToString());
            if(needKey == 0)
            {
                OpenField(nextField);
            }
        }
    }

    public void OpenField(GameObject field)
    {
        nextField.SetActive(true);
        
        foreach(GameObject offCol in offCollision) 
        {
            offCol.SetActive(false);
        }

        this.gameObject.SetActive(false);
    }

    public void ResetNeedKey()
    {
        needKey = beginNeedKey;
        text.SetText(needKey.ToString());
        this.gameObject.SetActive(true);
    }
}
