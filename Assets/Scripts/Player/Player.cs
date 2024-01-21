using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Inst { get; private set; }

    public Rigidbody2D rigid = null;
    public PlayerCollider[] collidersArr = null;

    public FloatingJoystick joystick = null;

    public float speed;
    Vector2 moveVec;
    //readonly private List<Monster> InRangeMonsterList; //공격 범위 안에 들어온 몬스터
    public Scanner scanner;

    private void Awake()
    {
        Inst = this;
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponent<Scanner>();
    }

    private void FixedUpdate()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        moveVec = new Vector2(x, y) * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec);

        if(moveVec.sqrMagnitude == 0) { return; }


    }

    void Update()
    {
        
    }
}
