using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    public BoxCollider2D col;
    public TextMeshProUGUI text;

    [Header("���� �ݳ� ������ �ð�")]
    public static float waitTime = 0.15f;
    public WaitForSeconds time = new WaitForSeconds(waitTime);//�� �ٲٸ� �ؿ� DOMoveTween()�� �ٲ����

    [Header("�ʿ��� ����Ʈ ���� ���� ��")]
    public int needKey;
    public int beginNeedKey;
    public SpriteRenderer pencil;

    [Header("���� �ʵ�")]
    public GameObject nextField;
    public GameObject[] offCollision;

    private Tween pencilTween;
    private bool once;  //������Ʈ���� �ѹ��� ȣ��Ǳ�� bool���� 
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
            //StartCoroutine(DecreaseKey());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //���ǿ� �ݵ�� 1�� �̻��϶��� �־�� �ִϸ��̼��� ���� �۵���
        //once�� Stay���¿��� �ѹ����� ȣ��Ǳ��
        //�� ���ǹ��� �ǵ����� ���ÿ�
        if (collision.gameObject.CompareTag("Player") && !once && GameManager.Inst.player.keyCount > 0)
        {
            once = true;
            StartCoroutine(DecreaseKey());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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

        if (GameManager.Inst.player.keyCount > 0 && once)
        {
            DOMoveTween();
            yield return time;
        }
        else
        {
            pencil.transform.DOKill();
            pencil.gameObject.SetActive(false);
        }

        if (GameManager.Inst.player.keyCount > 0 && once)
        {
            GameManager.Inst.player.keyCount--;
            needKey--;
            text.SetText(needKey.ToString());
            if (needKey == 0)
            {
                OpenField(nextField);
            }
        }
        once = false;
        
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
            .SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                pencil.gameObject.SetActive(false); // Tween�� �Ϸ�Ǹ� ������Ʈ�� ��Ȱ��ȭ
            });
    }
}
