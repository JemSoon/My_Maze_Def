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
    //readonly private List<Monster> InRangeMonsterList; //���� ���� �ȿ� ���� ����
    public Scanner scanner;

    private int _keyCount = 0;
    public int keyCount
    {
        get { return _keyCount; }
        set
        {
            _keyCount = value;
            OnKeyCountChanged?.Invoke(_keyCount);
        }
    }
    public event System.Action<int> OnKeyCountChanged;

    public void IncrementKeyCount(int value)
    {
        keyCount += value;
    }

    private void Awake()
    {
        Inst = this;
        Inst._keyCount = 0;
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
