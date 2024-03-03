using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    int spawnCount;//총 소환할 마리수
    int beginSpawnCount; //초기 설정 소환 마리수
    int nowCount = 0; //현재 소환한 마리수
    int index; //레벨에 따른 몬스터 종류
    float timer;
    //[Header("몇초마다 단계를 올립니까?")]
    //public float seconds;

    [Header("Spawn Order What you want")]
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
        //index = Mathf.Min(Mathf.FloorToInt(GameManager.Inst.gameTime / seconds),spawnData.Length-1);//원래 호칭은 level이였으나 용도에 맞게 index로 변경

        if (timer > spawnData[(int)monsterTypes[index]].spawnTime && nowCount<spawnCount)
        {
            timer = 0;
            Spawn();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ++spawnCount;
        //    ++nowCount;
        //    Spawn();
        //}
    }

    void Spawn()
    {
        GameObject monster = GameManager.Inst.poolManager.Get(0);
        //monster.transform.position = spawnPoint[]

        monster.GetComponent<Monster>().Init(spawnData[(int)monsterTypes[index]]);
        monster.transform.position = this.transform.position;//몬스터 위치 스폰 포탈 위치로 초기화

        if(index < monsterTypes.Length-1)
        {
            //monsterTypes가 4종류라면 인덱스는 3번까지 생성이 되고
            //2번일때 ++해서 3번이 되고나서 ++하면 안되니 Length-1
            ++index;
        }

        ++nowCount;
    }

    public void ResetSpawnner()
    {
        spawnCount = beginSpawnCount;
        nowCount = 0;
        index = 0;
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int hp;
    public float speed;
    public int knockBackForce;

    public int giveGoldCount;
}

[System.Serializable]
public enum MonsterType
{
    red,
    purple,
    blue,
}