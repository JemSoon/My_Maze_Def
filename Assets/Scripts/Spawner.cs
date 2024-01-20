using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    int level;
    float timer;
    [Header("���ʸ��� �ܰ踦 �ø��ϱ�?")]
    public float seconds;
    //public Transform[] spanwPoint;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //SpawnData�� �迭�� �����ʰ� ����
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Inst.gameTime / seconds),spawnData.Length-1);

        if(timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Inst.poolManager.Get(0);
        }
    }

    void Spawn()
    {
        GameObject monster = GameManager.Inst.poolManager.Get(0);
        //monster.transform.position = spawnPoint[]
        monster.GetComponent<Monster>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int hp;
    public float speed;
}
