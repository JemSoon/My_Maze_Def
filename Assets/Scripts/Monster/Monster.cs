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
    //Animator anim;
    bool isLive = true;


    public Vector2 GetCurrentPos => this.transform.position;
    public bool isAlive => 0 < this.hp && this.gameObject.activeSelf;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }

    public void Activate_Func()
    {
        this.gameObject.SetActive(true);

        this.transform.position = portal.transform.position;
        this.waypointID = 0;
        this.isMoving = true;
        StartCoroutine(Moving());
    }

    void FixedUpdate()
    {
        //나중에 길이 오픈되었을때 한번 호출하게끔 바꾸자
        if(isArrived && !isMoving) 
        { 
            FindWayPoint(waypointID); 
        }
    }

    IEnumerator Moving()
    {
        if (!isLive) { yield break; }

        while (isMoving)
        {
            Vector2 dir = (((Vector2)targetObject.transform.position) - (Vector2)transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * moveSpeed);

            Vector2 targetPosition = targetObject.transform.position;
            Vector2 currentPosition = transform.position;

            arrivalDistanceSquared = Mathf.Pow(Vector2.Distance(targetPosition, currentPosition) + 0.9f, 2);

            //if (dir == Vector2.zero)
            if (dir.sqrMagnitude >= arrivalDistanceSquared) //오차 범위내에 도착했다면
            {
                isArrived = true;
                ++waypointID;
                FindWayPoint(waypointID);
            }

            yield return null;
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
                StartCoroutine(Moving());
                break;
            }
        }

        if(isFound==false)
        {
            isMoving = false;
            Debug.Log("길을 잃었다..목적지가 없다..");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 0)
        {
            moveSpeed = 0;
            isMoving = false;
        }
    }

    private void OnEnable()
    {
        //풀링으로 되살아나거나 새로 만들어져 액티브 됬을 때
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
}
