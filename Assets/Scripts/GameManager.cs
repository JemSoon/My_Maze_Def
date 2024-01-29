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
    public TextMeshProUGUI tmp;
    //public bool isGameOver;
    public GameObject resultMenu;

    private void Awake()
    {
        Inst = this;
        player.OnKeyCountChanged += UpdateKeyCountText;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        //tmp.text = "���� : "+player.keyCount.ToString();
    }

    private void UpdateKeyCountText(int keyCount)
    {
        // keyCountText�� text �Ӽ��� ������Ʈ
        tmp.text = "���� : " + keyCount;
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;
        resultMenu.SetActive(true);
    }

    public void ResetGame()
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
        foreach(Gate gate in gates)
        {
            gate.ResetNeedKey();
        }

        Spawner[] spawners = FindObjectsOfType<Spawner>();
        foreach(Spawner spawner in spawners)
        {
            spawner.ResetSpawnner();
        }

        PoolManager pool = FindObjectOfType<PoolManager>();
        pool.ResetPoolManager();//����,�Ѿ�,�������� ���� ���� �ʱ�ȭ
    }
}
