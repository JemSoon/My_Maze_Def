using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public float gameTime;

    public PoolManager poolManager;
    public Player player;
    public TextMeshProUGUI penTmp;
    public TextMeshProUGUI goldTmp;
    //public bool isGameOver;
    public GameObject resultMenu;
    public GameObject upgradeMenu;
    public bool checkUpgrade; //���׷��̵� �޴� Ȯ���߽��ϱ�?

    public TextMeshProUGUI goldAmountTmp;

    private void Awake()
    {
        Inst = this;
        player.OnKeyCountChanged += UpdateKeyCountText;
        player.OnGoldCountChanged += UpdateGoldCountText;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        //tmp.text = "���� : "+player.keyCount.ToString();
    }

    private void UpdateKeyCountText(int keyCount)
    {
        // keyCountText�� text �Ӽ��� ������Ʈ
        penTmp.text = keyCount.ToString();
    }
    private void UpdateGoldCountText(int goldCount)
    {
        // goldCountText�� text �Ӽ��� ������Ʈ
        goldTmp.text = "+ "+ goldCount.ToString();
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;
        resultMenu.SetActive(true);
        upgradeMenu.SetActive(true);
        RectTransform canvasRectTransform = upgradeMenu.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        upgradeMenu.transform.DOMove(canvasRectTransform.anchoredPosition, 2.0f).SetEase(Ease.OutBounce).SetUpdate(true);

        OutGameMoney.Inst.money += player.goldCount;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        OutGameMoney.Inst.SaveInfo();
    }

    public void ResetGame()
    {
        if(checkUpgrade)
        {
            resultMenu.SetActive(false);
            //����ŸƮ
            Time.timeScale = 1f;
            gameTime = 0f;
            player.ResetPlayerPos();
            UpdateKeyCountText(player.keyCount);//UI���谳���� �ʱ�ȭ
            FieldManager.Inst.ResetFields();

            //�ڡڡڸ��ʹ� Ǯ �Ŵ����ʿ��� �����ߡڡڡ�

            //����Ʈ ���� �ʱ�ȭ + ��Ƽ��
            Gate[] gates = FindObjectsOfType<Gate>();
            foreach (Gate gate in gates)
            {
                gate.ResetNeedKey();
            }

            Spawner[] spawners = FindObjectsOfType<Spawner>();
            foreach (Spawner spawner in spawners)
            {
                spawner.ResetSpawnner();
            }

            PoolManager pool = FindObjectOfType<PoolManager>();
            pool.ResetPoolManager();//����,�Ѿ�,�������� ���� ���� �ʱ�ȭ
            checkUpgrade = false;
        }
    }

    public void DATAResetButton()
    {
        //������ ���� ��ư ���� �� ����Ű�� ������ ������
        OutGameMoney.Inst.DeleteInfo();
    }
}
