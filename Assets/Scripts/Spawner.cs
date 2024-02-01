using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    public int spawnCount = 0;//총 소환할 마리수
    int beginSpawnCount; //초기 설정 소환 마리수
    int nowCount = 0; //현재 소환한 마리수
    int level; //레벨에 따른 몬스터 종류
    float timer;
    [Header("몇초마다 단계를 올립니까?")]
    public float seconds;
    //public Transform[] spanwPoint;

    [Header("당신이 원하는 소환 순서")]
    public MonsterType[] monsterTypes;
    private void Awake()
    {
        spawnCount = monsterTypes.Length;
        //기본 소환 마리수 저장해두기
        beginSpawnCount = spawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //SpawnData의 배열을 넘지않게 제한
        //level = Mathf.Min(Mathf.FloorToInt(GameManager.Inst.gameTime / seconds),spawnData.Length-1);

        if (timer > spawnData[(int)monsterTypes[level]].spawnTime && nowCount<spawnCount)
        {
            timer = 0;
            Spawn();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ++spawnCount;
            ++nowCount;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject monster = GameManager.Inst.poolManager.Get(0);
        //monster.transform.position = spawnPoint[]

        monster.GetComponent<Monster>().Init(spawnData[(int)monsterTypes[level]]);
        
        if(level<monsterTypes.Length-1)
        {
            ++level;
        }

        ++nowCount;
    }

    public void ResetSpawnner()
    {
        spawnCount = beginSpawnCount;
        nowCount = 0;
        level = 0;
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int hp;
    public float speed;

    public int giveKeyCount;
}

[System.Serializable]
public enum MonsterType
{
    red,
    purple,
    blue,
}