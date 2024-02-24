using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    public BoxCollider2D col;
    public TextMeshProUGUI text;
    public WaitForSeconds time = new WaitForSeconds(waitTime);//값 바꾸면 밑에 DOMoveTween()도 바꿔야함

    [Header("연필 반납 딜레이 시간")]
    public static float waitTime = 0.15f;

    [Header("필요한 게이트 오픈 열쇠 수")]
    public int needKey;
    public int beginNeedKey;
    bool isGiveKey;
    public SpriteRenderer pencil;

    [Header("다음 필드")]
    public GameObject nextField;
    public GameObject[] offCollision;

    private Tween pencilTween;
    private int initialKeyCount = 0;

    private void Awake()
    {
        beginNeedKey = needKey;
        text.SetText(needKey.ToString());
    }

    private void OnEnable()
    {
        needKey = beginNeedKey;
        text.SetText(needKey.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (GameManager.Inst.player.keyCount == 0) { return; }

        if (collision.gameObject.CompareTag("Player"))
        {
            isGiveKey = true;
            initialKeyCount = GameManager.Inst.player.keyCount;
            StartCoroutine(DecreaseKey());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        initialKeyCount = GameManager.Inst.player.keyCount;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isGiveKey = false;
            pencil.gameObject.SetActive(false);
            pencil.transform.DOKill();
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
            if (initialKeyCount > 0)
            {
                Debug.Log(initialKeyCount);
                DOMoveTween();
            }
            else
            {
                pencil.transform.DOKill();
                pencil.gameObject.SetActive(false);
            }

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

    private void DOMoveTween()
    {
        // 플레이어의 현재 위치에서 목표 지점까지 이동하는 Tween을 생성
        pencil.transform.position = GameManager.Inst.player.transform.position + new Vector3(0, 2, 0);
        pencil.gameObject.SetActive(true);

        // 직선반납 DOTween
        ////pecnilTween을 변수로 받아야하나? 싶지만 받아서 해야 좀 더 깔끔히 되는거같음? 안그럼 가끔 버벅임
        //pencilTween = pencil.transform.DOMove(transform.position/* + new Vector3(0,-3,0)*/, 0.1f)
        //    .SetEase(Ease.InOutQuad)
        //    .OnComplete(DOMoveTween); // Tween이 완료될 때마다 DOMoveTween 함수를 재귀적으로 호출하여 반복 실행

        // 포물선 반납 DOTween
        pencilTween = pencil.transform.DOJump(transform.position, 2.0f, 1, waitTime)
            .SetEase(Ease.InOutQuad);
    }
}
