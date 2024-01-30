using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public BoxCollider2D col;
    public TextMeshProUGUI text;
    public WaitForSeconds time = new WaitForSeconds(0.1f);
    public int needKey;
    int beginNeedKey;
    bool isGiveKey;
    public SpriteRenderer pencil;

    [Header("다음 필드")]
    public GameObject nextField;
    public GameObject[] offCollision;

    private void Awake()
    {
        beginNeedKey = needKey;
        text.SetText(needKey.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Inst.player.keyCount == 0) { return; }

        if (collision.gameObject.CompareTag("Player"))
        {
            isGiveKey = true;
            StartCoroutine(DecreaseKey());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Inst.player.keyCount == 0) { return; }

        if (collision.gameObject.CompareTag("Player"))
        {
            //GameManager.Inst.player.keyCount--;
            //needKey--;
            //text.SetText(needKey.ToString());
            //if(needKey == 0)
            //{
            //    OpenField(nextField);
            //}
            //StartCoroutine(DecreaseKey());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGiveKey = false;
            pencil.gameObject.SetActive(false);
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

        //꺼진 콜리전도 다시 키기
        foreach (GameObject offCol in offCollision)
        {
            offCol.SetActive(true);
        }
    }

    IEnumerator DecreaseKey()
    {
        while(isGiveKey)
        {
            pencil.gameObject.SetActive(true);
            
            yield return time;

            if (GameManager.Inst.player.keyCount > 0)
            {
                GameManager.Inst.player.keyCount--;
                needKey--;
                text.SetText(needKey.ToString());
                if (needKey == 0)
                {
                    OpenField(nextField);
                }
            }
        }
    }
}
