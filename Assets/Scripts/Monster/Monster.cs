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
    float arrivalDistanceSquared; //������ ���� ��������

    [Header("����Ż ����")]
    public RuntimeAnimatorController[] animCon;
    public float hp;
    public float maxHp;
    Rigidbody2D rigid;
    public Animator anim;
    SpriteRenderer sprite;
    bool isLive = true;
    WaitForFixedUpdate wait;

    [Header("�� �浹 üũ")]
    public bool isDownWall = false;
    public bool isTopWall = false;
    public bool isRightWall = false;
    public bool isLeftWall = false;

    public bool isKnockBacking = false;

    [Header("���Ͱ� �ִ� ��ȭ")]
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
        #region �ڷ�ƾ ����
        //���߿� ���� ���µǾ����� �ѹ� ȣ���ϰԲ� �ٲ���
        //if(isArrived && !isMoving) 
        //{ 
        //    FindWayPoint(waypointID); 
        //}
        #endregion

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Monster_Hit")) //�׷����� �׷��� �̸�..
        {
            isKnockBacking = true;
            //�׾��ų� �´� ���¶�� �ൿ ��ž
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
            rigid.velocity = Vector2.zero; //�÷��̾� �и��� �ʰ�

            float arrivalDistance = 0.01f; //���� �Ÿ� ���� ����

            if (dirVec.magnitude <= arrivalDistance)
            {
                //���� ���� ���� ������
                Debug.Log("��ǥ ����");

                ++waypointID;
                isMoving = false;
                FindWayPoint(waypointID); //�̹� ���� �������� ������������ ������
            }
            else
            {
                rigid.MovePosition(rigid.position + nextVec);
            }
        }
        else
        {
            //�������� �ʰ� �ִٸ� ��� ��ǥ���� ������� �˻�
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
                Debug.Log("���� ����Ʈ Ȯ�� ����");
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
            Debug.Log("���� �Ҿ���..�������� ����..");
        }
    }

    private void OnEnable()
    {
        //Ǯ������ �ǻ�Ƴ��ų� ���� ������� ��Ƽ�� ���� ��
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
            //��Ʈ�׼�
            anim.SetTrigger("Hit");
        }
        else
        {
            //���
            Dead();
        }

    }

    IEnumerator KnockBack()
    {
        yield return wait; //���� �ϳ��� ���� ������ ������

        foreach(var item in wallCheckers)
        {
            item.particle.Stop();
        }

        //�÷��̾��� ��ġ
        Vector3 playerPos = GameManager.Inst.player.transform.position;
        //�÷��̾��� �ݴ�������� ����(�÷��̾��� ��ġ�� ����)
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

        //��ֶ�����ȭ ��(ũ�⻩�� ���⸸ ����) dirVec�� �и��� ��(3), �������
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
            //Ű�� �ٰ� �ƴ϶� ��带 �����
            GameManager.Inst.player.goldCount += giveGoldCount;

            rigid.MovePosition(Vector3.zero);
            coinSprite.gameObject.transform.DOMove(transform.position + new Vector3(0, 2, 0), 1.0f).SetEase(Ease.OutBack).OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            //���� ����ġ��
            coinSprite.gameObject.transform.position = transform.position;
        }
    }

    #region �ڷ�ƾ ����
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
    //        if (dir.sqrMagnitude >= arrivalDistanceSquared) //���� �������� �����ߴٸ�
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
