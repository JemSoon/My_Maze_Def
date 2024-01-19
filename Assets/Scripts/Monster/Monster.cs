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

            if (dir == Vector2.zero)
            {
                isMoving = false;
            }

            yield return null;
        }
    }

    void Move()
    {
        Vector2 dir = (((Vector2)targetObject.transform.position) - (Vector2)transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * moveSpeed);

        if (dir == Vector2.zero)
        {
            isMoving = false;
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
}
