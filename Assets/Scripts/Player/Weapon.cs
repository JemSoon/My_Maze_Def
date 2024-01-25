using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count; //원거리에선 관통할 개수
    public float speed;

    float timer;

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;
                
                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //Test LevelUp
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LevelUp(10, 1); //데미지, 관통수
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id ==0)
        {
            Place();
        }
    }

    public void Init()
    {
        switch(id)
        {
            case 0: //근접 무기 회전속도
                speed = 300;
                Place();

                break;

            default: //디폴트에선 연사속도
                speed = 1.0f;
                break;
        }
    }

    void Place()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;
            
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Inst.poolManager.Get(prefabId).transform;
                bullet.parent = transform;
            }            

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);

        }
    }

    void Fire()
    {
        if(!Player.Inst.scanner.nearestTarget)
        { return;}

        //적을 향해 바라보는 방향
        Vector3 targetPos = Player.Inst.scanner.nearestTarget.position;//위치
        Vector3 dir = targetPos - transform.position;//방향(크기포함)
        dir = dir.normalized;//정규화(크기는 1로 동일시화)

        Transform bullet = GameManager.Inst.poolManager.Get(prefabId).transform.transform;
        bullet.position = transform.position;

        //목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
