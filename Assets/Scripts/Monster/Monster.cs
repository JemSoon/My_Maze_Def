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
    Animator anim;
    SpriteRenderer sprite;
    bool isLive = true;


    public Vector2 GetCurrentPos => this.transform.position;
    public bool isAlive => 0 < this.hp && this.gameObject.activeSelf;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite= GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Activate_Func()
    {
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

        if (!isLive) { return; }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            moveSpeed = 0;
            isMoving = false;
        }
    }

    private void OnEnable()
    {
        //Ǯ������ �ǻ�Ƴ��ų� ���� ������� ��Ƽ�� ���� ��
        isLive = true;
        hp = maxHp;
        Activate_Func();
    }

    public void Init(SpawnData data)
    {
        //anim.runtimeAnimatorController = animCon[data.spriteType];
        moveSpeed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet"))
        { return; }

        hp -= collision.GetComponent<Bullet>().damage;

        if(hp > 0)
        {
            //��Ʈ�׼�
        }
        else
        {
            //���
            Dead();
        }

    }

    void Dead()
    {
        isLive=false;
        gameObject.SetActive(false);
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
