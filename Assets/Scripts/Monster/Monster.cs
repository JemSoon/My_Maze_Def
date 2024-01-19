using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp;
    public GameObject targetObject;
    public float moveSpeed;
    public GameObject portal;
    public int waypointID = 0;
    public CapsuleCollider2D monsterCollider;
    public ColliderType monsterColliderTyep;
    public bool isMoving = false;
    float arrivalDistanceSquared; //목적지 도착 오차범위


    public Vector2 GetCurrentPos => this.transform.position;
    public bool isAlive => 0 < this.hp && this.gameObject.activeSelf;

    private void Start()
    {
        Activate_Func();
    }

    public void Activate_Func()
    {
        this.gameObject.SetActive(true);

        this.transform.position = portal.transform.position;
        this.waypointID = 0;
        this.hp = 5;
        this.isMoving = true;
        StartCoroutine(Moving());
    }

    void Update()
    {

    }

    IEnumerator Moving()
    {
        while (isMoving)
        {
            Vector2 dir = (((Vector2)targetObject.transform.position) - (Vector2)transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * moveSpeed);

            Vector2 targetPosition = targetObject.transform.position;
            Vector2 currentPosition = transform.position;

            arrivalDistanceSquared = Mathf.Pow(Vector2.Distance(targetPosition, currentPosition) +0.9f, 2);

            //if (dir == Vector2.zero)
            if (dir.sqrMagnitude >= arrivalDistanceSquared) //오차 범위내에 도착했다면
            {
                ++waypointID;
                FindWayPoint(waypointID);
            }

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 0)
        {
            moveSpeed = 0;
            isMoving = false;
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
                targetObject = point.gameObject;
                isFound = true;
                break;
            }
        }

        if(isFound==false)
        {
            isMoving = false;
            Debug.Log("길을 잃었다..목적지가 없다..");
        }
    }
}
