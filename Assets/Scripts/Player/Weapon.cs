using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count; //���Ÿ����� ������ ����
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
            LevelUp(10, 1); //������, �����
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
            case 0: //���� ���� ȸ���ӵ�
                speed = 300;
                Place();

                break;

            default: //����Ʈ���� ����ӵ�
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

        //���� ���� �ٶ󺸴� ����
        Vector3 targetPos = Player.Inst.scanner.nearestTarget.position;//��ġ
        Vector3 dir = targetPos - transform.position;//����(ũ������)
        dir = dir.normalized;//����ȭ(ũ��� 1�� ���Ͻ�ȭ)

        Transform bullet = GameManager.Inst.poolManager.Get(prefabId).transform.transform;
        bullet.position = transform.position;

        //��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
