using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    int spawnCount;//총 소환할 마리수
    int beginSpawnCount; //초기 설정 소환 마리수
    int nowCount = 0; //현재 소환한 마리수
    int index; //레벨에 따른 몬스터 종류
    float timer;

    int Zpos; //몬스터 나올때마다 Z값 올려주기용
    //[Header("몇초마다 단계를 올립니까?")]
    //public float seconds;

    [Header("Spawn Order What you want")]
    public MonsterData[] monsterDatas;

    private void Awake()
    {
        spawnCount = monsterDatas.Length;
        //기본 소환 마리수 저장해두기
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
        monster.transform.position = spawnPos;//몬스터 위치 스폰 포탈 위치로 초기화 + Z값으로 몬스터 오브젝트 소팅
                                              //이렇게 하지 않으면 몬스터가 뒤에 있어도 글자가 앞에 튀어나와 앞 몬스터와 겹침
        ++Zpos;
        if (Zpos >= 100) 
        {
            Zpos = 0; 
        } //숫자 넘 커지면 혹시 모를 안전용 초기화

        if (index < monsterDatas.Length - 1)
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