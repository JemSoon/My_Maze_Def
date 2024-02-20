using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; //�� ���� ����?
    public float lifeTime;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.velocity = dir * 15f;//���ϴ� �ӷ� Ŀ����
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Monster") || per == -100)
        { return; }

        if(per > -1)
        {
            Monster mon = collision.GetComponent<Monster>();
            mon.hp -= damage;
            mon.StartCoroutine(mon.KnockBack());

            if(mon.hp>0)
            { mon.anim.SetTrigger("Hit"); }
            else
            {
                mon.Dead();
            }
        }

        per--;

        if(per == -1) 
        {
            //����������
            rigid.velocity =Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void Dead()
    {
        //���� �Ÿ��̻� �־����� �ҷ� �Ҹ�
        //Transform target = GameManager.Inst.player.transform;
        //Vector3 targetPos = target.position;
        //float dir = Vector3.Distance(targetPos, transform.position);
        
        //if (dir > 20f)
        { this.gameObject.SetActive(false); }
    }

    private void OnEnable()
    {
        if(lifeTime != -1)
        {
            //lifeTime�ð��� ������ Dead�Լ� ����
            Invoke(nameof(Dead), lifeTime);
        }
    }
    private void OnDisable()
    {
        if(IsInvoking(nameof(Dead)))
        {
            //��Ȱ��ȭ�Ǹ� Dead�̺�Ʈ �Լ� ����
            CancelInvoke(nameof(Dead));
        }
    }

    private void FixedUpdate()
    {
        float rotationSpeed = 1000f; //�Ѿ� �ӽ� ȸ�� �ӵ�
        float rotationAmount = rotationSpeed * Time.deltaTime;
        gameObject.transform.Rotate(0,0,rotationAmount);
    }
}
