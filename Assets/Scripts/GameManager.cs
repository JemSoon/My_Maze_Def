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
    public GameObject makingPencilUI;

    public TextMeshProUGUI goldAmountTmp;

    private void Awake()
    {
        Inst = this;
        player.OnKeyCountChanged += UpdateKeyCountText;
        player.OnGoldCountChanged += UpdateGoldCountText;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        GameEnd();
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
        player.gameObject.SetActive(false);

        PoolManager pool = FindObjectOfType<PoolManager>();
        pool.ResetPoolManager();//����,�Ѿ�,�������� ���� ���� �ʱ�ȭ

        makingPencilUI.SetActive(false);//���ʸ���� �����̵�� ����߿� ��Ȱ��ȭ

        resultMenu.SetActive(true);
        upgradeMenu.SetActive(true);

        OutGameMoney.Inst.money += player.goldCount;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        OutGameMoney.Inst.SaveInfo();
    }

    public void ResetGame()
    {
        //��� �޴� UI�ݱ�
        resultMenu.SetActive(false);
        upgradeMenu.SetActive(false);

        //����ŸƮ
        player.gameObject.SetActive(true);
        Time.timeScale = 1f;
        gameTime = 0f;
        player.ResetPlayerPos();
        UpdateKeyCountText(player.keyCount);//UI���谳���� �ʱ�ȭ
        FieldManager.Inst.ResetFields();
        makingPencilUI.SetActive(true);//���� ���� UIȰ��ȭ

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

        //PoolManager pool = FindObjectOfType<PoolManager>();
        //pool.ResetPoolManager();//����,�Ѿ�,�������� ���� ���� �ʱ�ȭ
    }

    public void DATAResetButton()
    {
        //������ ���� ��ư ���� �� ����Ű�� ������ ������
        OutGameMoney.Inst.DeleteInfo();
    }
}
