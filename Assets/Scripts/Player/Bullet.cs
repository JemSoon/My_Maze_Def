using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; //몇 마리 관통?
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
            rigid.velocity = dir * 15f;//원하는 속력 커스텀
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
            //mon.StartCoroutine(mon.KnockBack());
            mon.KnockBackStart = true;
            if (mon.hp>0)
            { mon.anim.SetTrigger("Hit"); }
            else
            {
                mon.Dead();
            }
        }

        per--;

        if(per == -1) 
        {
            //근접무기라면
            rigid.velocity =Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void Dead()
    {
        //일정 거리이상 멀어지면 불렛 소멸
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
            //lifeTime시간이 지나면 Dead함수 실행
            Invoke(nameof(Dead), lifeTime);
        }
    }
    private void OnDisable()
    {
        if(IsInvoking(nameof(Dead)))
        {
            //비활성화되면 Dead이벤트 함수 제거
            CancelInvoke(nameof(Dead));
        }
    }

    private void FixedUpdate()
    {
        float rotationSpeed = 1000f; //총알 임시 회전 속도
        float rotationAmount = rotationSpeed * Time.deltaTime;
        gameObject.transform.Rotate(0,0,rotationAmount);
    }
}
