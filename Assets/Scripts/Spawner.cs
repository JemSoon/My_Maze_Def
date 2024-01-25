using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    public int spawnCount = 0;//총 소환할 마리수
    int nowCount = 0; //현재 소환한 마리수
    int level;
    float timer;
    [Header("몇초마다 단계를 올립니까?")]
    public float seconds;
    //public Transform[] spanwPoint;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //SpawnData의 배열을 넘지않게 제한
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Inst.gameTime / seconds),spawnData.Length-1);

        if(timer > spawnData[level].spawnTime && nowCount<spawnCount)
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
        monster.GetComponent<Monster>().Init(spawnData[level]);
        ++nowCount;
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
