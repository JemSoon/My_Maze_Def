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
        //tmp.text = "열쇠 : "+player.keyCount.ToString();
    }

    private void UpdateKeyCountText(int keyCount)
    {
        // keyCountText의 text 속성을 업데이트
        tmp.text = "열쇠 : " + keyCount;
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;
        resultMenu.SetActive(true);
    }

    public void ResetGame()
    {
        resultMenu.SetActive(false);
        //리스타트
        Time.timeScale = 1f;
        gameTime = 0f;
        player.ResetPlayerPos();
        UpdateKeyCountText(player.keyCount);//UI열쇠개수도 초기화
        FieldManager.Inst.ResetFields();

        //★★★몬스터는 풀 매니저쪽에서 삭제중★★★

        //게이트 조건 초기화 + 액티브
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
        pool.ResetPoolManager();//몬스터,총알,근접무기 전부 삭제 초기화
    }
}
