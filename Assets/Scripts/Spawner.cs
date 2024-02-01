using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    public int spawnCount = 0;//�� ��ȯ�� ������
    int beginSpawnCount; //�ʱ� ���� ��ȯ ������
    int nowCount = 0; //���� ��ȯ�� ������
    int level; //������ ���� ���� ����
    float timer;
    [Header("���ʸ��� �ܰ踦 �ø��ϱ�?")]
    public float seconds;
    //public Transform[] spanwPoint;

    [Header("����� ���ϴ� ��ȯ ����")]
    public MonsterType[] monsterTypes;
    private void Awake()
    {
        spawnCount = monsterTypes.Length;
        //�⺻ ��ȯ ������ �����صα�
        beginSpawnCount = spawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //SpawnData�� �迭�� �����ʰ� ����
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