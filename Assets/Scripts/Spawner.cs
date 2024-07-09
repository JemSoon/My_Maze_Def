using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    int spawnCount;//�� ��ȯ�� ������
    int beginSpawnCount; //�ʱ� ���� ��ȯ ������
    int nowCount = 0; //���� ��ȯ�� ������
    int index; //������ ���� ���� ����
    float timer;

    int Zpos; //���� ���ö����� Z�� �÷��ֱ��
    //[Header("���ʸ��� �ܰ踦 �ø��ϱ�?")]
    //public float seconds;

    [Header("Spawn Order What you want")]
    public MonsterData[] monsterDatas;

    private void Awake()
    {
        spawnCount = monsterDatas.Length;
        //�⺻ ��ȯ ������ �����صα�
        beginSpawnCount = spawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (GameManager.Inst.gameTime >= monsterDatas[index].spawnTime && nowCount < spawnCount && !monsterDatas[index].isSpawn)
        {
            monsterDatas[index].isSpawn = true;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject monster = GameManager.Inst.poolManager.Get(0);

        //monster.GetComponent<Monster>().Init(spawnData[(int)monsterTypes[index]]);
        monster.GetComponent<Monster>().Init(spawnData[(int)monsterDatas[index].monsterType]);

        //UnityEngine.Vector3 spawnPos = new UnityEngine.Vector3(this.transform.position.x, this.transform.position.y, Zpos);
        Vector3 spawnPos = this.transform.position;
        monster.GetComponent<Monster>().sprite.sortingOrder = 2 + Zpos;
        monster.GetComponent<Monster>().canvas.sortingOrder = 2 + Zpos;
        monster.transform.position = spawnPos;//���� ��ġ ���� ��Ż ��ġ�� �ʱ�ȭ + Z������ ���� ������Ʈ ����
                                              //�̷��� ���� ������ ���Ͱ� �ڿ� �־ ���ڰ� �տ� Ƣ��� �� ���Ϳ� ��ħ
        ++Zpos;
        if (Zpos >= 100) 
        {
            Zpos = 0; 
        } //���� �� Ŀ���� Ȥ�� �� ������ �ʱ�ȭ

        if (index < monsterDatas.Length - 1)
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
    //public float spawnTime;
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
    green,
}

[System.Serializable]
public class MonsterData
{
    public MonsterType monsterType;
    public bool isSpawn;
    public float spawnTime;
}