using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    public BoxCollider2D col;
    public TextMeshProUGUI text;
    public WaitForSeconds time = new WaitForSeconds(waitTime);//�� �ٲٸ� �ؿ� DOMoveTween()�� �ٲ����

    [Header("���� �ݳ� ������ �ð�")]
    public static float waitTime = 0.15f;

    [Header("�ʿ��� ����Ʈ ���� ���� ��")]
    public int needKey;
    public int beginNeedKey;
    bool isGiveKey;
    public SpriteRenderer pencil;

    [Header("���� �ʵ�")]
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

        //���� �ݸ����� �ٽ� Ű��
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
        // �÷��̾��� ���� ��ġ���� ��ǥ �������� �̵��ϴ� Tween�� ����
        pencil.transform.position = GameManager.Inst.player.transform.position + new Vector3(0, 2, 0);
        pencil.gameObject.SetActive(true);

        // �����ݳ� DOTween
        ////pecnilTween�� ������ �޾ƾ��ϳ�? ������ �޾Ƽ� �ؾ� �� �� ����� �Ǵ°Ű���? �ȱ׷� ���� ������
        //pencilTween = pencil.transform.DOMove(transform.position/* + new Vector3(0,-3,0)*/, 0.1f)
        //    .SetEase(Ease.InOutQuad)
        //    .OnComplete(DOMoveTween); // Tween�� �Ϸ�� ������ DOMoveTween �Լ��� ��������� ȣ���Ͽ� �ݺ� ����

        // ������ �ݳ� DOTween
        pencilTween = pencil.transform.DOJump(transform.position, 2.0f, 1, waitTime)
            .SetEase(Ease.InOutQuad);
    }
}
