using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    int spawnCount;//�� ��ȯ�� ������
    int beginSpawnCount; //�ʱ� ���� ��ȯ ������
    int nowCount = 0; //���� ��ȯ�� ������
    int index; //������ ���� ���� ����
    float timer;
    //[Header("���ʸ��� �ܰ踦 �ø��ϱ�?")]
    //public float seconds;

    [Header("Spawn Order What you want")]
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
        //index = Mathf.Min(Mathf.FloorToInt(GameManager.Inst.gameTime / seconds),spawnData.Length-1);//���� ȣĪ�� level�̿����� �뵵�� �°� index�� ����

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
        monster.transform.position = this.transform.position;//���� ��ġ ���� ��Ż ��ġ�� �ʱ�ȭ

        if(index < monsterTypes.Length-1)
        {
            //monsterTypes�� 4������� �ε����� 3������ ������ �ǰ�
            //2���϶� ++�ؼ� 3���� �ǰ��� ++�ϸ� �ȵǴ� Length-1
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