using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //public int hp;
    public GameObject targetObject;
    public float moveSpeed;
    public GameObject portal;
    public int waypointID = 0;
    public CapsuleCollider2D monsterCollider;
    public ColliderType monsterColliderTyep;
    public bool isMoving = false;
    public bool isArrived = false;
    float arrivalDistanceSquared; //목적지 도착 오차범위

    [Header("골드메탈 센세")]
    public RuntimeAnimatorController[] animCon;
    public float hp;
    public float maxHp;
    Rigidbody2D rigid;
    public Animator anim;
    SpriteRenderer sprite;
    bool isLive = true;
    WaitForFixedUpdate wait;

    [Header("벽 충돌 체크")]
    public bool isDownWall = false;
    public bool isTopWall = false;
    public bool isRightWall = false;
    public bool isLeftWall = false;

    public bool isKnockBacking = false;

    [Header("몬스터가 주는 재화")]
    public SpriteRenderer coinSprite;
    public int giveGoldCount;

    public WallChecker[] wallCheckers;

    public Vector2 GetCurrentPos => this.transform.position;
    public bool isAlive => 0 < this.hp && this.gameObject.activeSelf;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite= GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        monsterCollider = GetComponent<CapsuleCollider2D>();
    }

    public void Activate_Func()
    {
        SetActiveMonster(true);
        this.gameObject.SetActive(true);

        this.transform.position = portal.transform.position;
        this.waypointID = 0;
        this.isMoving = true;
        //StartCoroutine(Moving());
    }

    void FixedUpdate()
    {
        #region 코루틴 무빙
        //나중에 길이 오픈되었을때 한번 호출하게끔 바꾸자
        //if(isArrived && !isMoving) 
        //{ 
        //    FindWayPoint(waypointID); 
        //}
        #endregion

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Monster_Hit")) //그래프에 그려진 이름..
        {
            isKnockBacking = true;
            //죽었거나 맞는 상태라면 행동 스탑
            return; 
        }

        Move();
    }

    void LateUpdate()
    {
        if (!isLive) { return; }

        sprite.flipX = targetObject.transform.position.x < rigid.position.x;
        anim.SetBool("isMoving", isMoving);
    }

    public void Move()
    {
        if(isKnockBacking)
        {
            foreach (var item in wallCheckers)
            {
                item.CheckMyColType(item.myColType, item.isTouching);
            }
            isKnockBacking = false;
        }
        

        if (isMoving)
        {
            Vector2 dirVec = (Vector2)targetObject.transform.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;
            rigid.velocity = Vector2.zero; //플레이어 밀리지 않게

            float arrivalDistance = 0.01f; //도착 거리 오차 범위

            if (dirVec.magnitude <= arrivalDistance)
            {
                //오차 범위 내에 들어오면
                Debug.Log("목표 도착");

                ++waypointID;
                isMoving = false;
                FindWayPoint(waypointID); //이미 다음 목적지가 열려있을수도 있으니
            }
            else
            {
                rigid.MovePosition(rigid.position + nextVec);
            }
        }
        else
        {
            //움직이지 않고 있다면 계속 목표지점 생겼는지 검색
            FindWayPoint(waypointID);
        }
    }

    public void FindWayPoint(int num)
    {
        bool isFound = false;

        WayPoint[] wayPoints = FindObjectsOfType<WayPoint>();

        foreach(WayPoint point in wayPoints)
        {
            if(point.pointNum == num)
            {
                Debug.Log("다음 포인트 확인 고고고");
                isMoving = true;
                isArrived = false;
                targetObject = point.gameObject;
                isFound = true;
                //StartCoroutine(Moving());
                break;
            }
        }

        if(isFound==false)
        {
            isMoving = false;
            Debug.Log("길을 잃었다..목적지가 없다..");
        }
    }

    private void OnEnable()
    {
        //풀링으로 되살아나거나 새로 만들어져 액티브 됬을 때
        waypointID = 0;
        isLive = true;
        hp = maxHp;
        Activate_Func();
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        moveSpeed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
        giveGoldCount = data.giveGoldCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet"))
        { return; }

        hp -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(hp > 0)
        {
            //히트액션
            anim.SetTrigger("Hit");
        }
        else
        {
            //쥬금
            Dead();
        }

    }

    IEnumerator KnockBack()
    {
        yield return wait; //다음 하나의 물리 프레임 딜레이

        foreach(var item in wallCheckers)
        {
            item.particle.Stop();
        }

        //플레이어의 위치
        Vector3 playerPos = GameManager.Inst.player.transform.position;
        //플레이어의 반대방향으로 가기(플레이어의 위치를 빼서)
        Vector3 dirVec = transform.position - playerPos;

        if (isDownWall && dirVec.y < 0f)
        {
            dirVec.y = 0;
        }
        if(isTopWall && dirVec.y > 0f)
        { 
            dirVec.y = 0;
        }
        if(isLeftWall && dirVec.x < 0f)
        {
            dirVec.x = 0;
        }
        if(isRightWall && dirVec.x > 0f)
        {
            dirVec.x = 0;
        }

        //노멀라이즈화 된(크기빼고 방향만 가진) dirVec에 밀리는 힘(3), 포스모드
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
    }

    void Dead()
    {
        SetActiveMonster(false);
    }

    void SetActiveMonster(bool active)
    {
        isMoving = active;
        sprite.enabled = active;
        coinSprite.gameObject.SetActive(!active);
        waypointID = 0;
        isLive = active;
        monsterCollider.enabled = active;

        if(active==false)
        {
            //키를 줄게 아니라 골드를 줘야함
            GameManager.Inst.player.goldCount += giveGoldCount;

            rigid.MovePosition(Vector3.zero);
            coinSprite.gameObject.transform.DOMove(transform.position + new Vector3(0, 2, 0), 1.0f).SetEase(Ease.OutBack).OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            //코인 원위치로
            coinSprite.gameObject.transform.position = transform.position;
        }
    }

    #region 코루틴 무빙
    //IEnumerator Moving()
    //{
    //    if (!isLive) { yield break; }

    //    while (isMoving)
    //    {
    //        Vector2 dir = (((Vector2)targetObject.transform.position) - (Vector2)transform.position).normalized;
    //        transform.Translate(dir * Time.deltaTime * moveSpeed);

    //        Vector2 targetPosition = targetObject.transform.position;
    //        Vector2 currentPosition = transform.position;

    //        arrivalDistanceSquared = Mathf.Pow(Vector2.Distance(targetPosition, currentPosition) + 0.9f, 2);

    //        //if (dir == Vector2.zero)
    //        if (dir.sqrMagnitude >= arrivalDistanceSquared) //오차 범위내에 도착했다면
    //        {
    //            isArrived = true;
    //            ++waypointID;
    //            FindWayPoint(waypointID);
    //        }

    //        yield return null;
    //    }
    //}
    #endregion
}
