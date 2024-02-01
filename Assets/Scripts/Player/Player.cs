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
    Vector3 beginPos;//ó�� ���� ��ġ ����
    public SpriteRenderer sprite;

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

    public Animator anim;

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
        Inst.beginPos = this.gameObject.transform.position;
        anim = GetComponent<Animator>();
        sprite= GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        moveVec = new Vector2(x, y) * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + moveVec);
        sprite.flipX = moveVec.x > 0;
        if (moveVec.sqrMagnitude == 0) 
        {
            anim.SetBool("isMoving", false);
            return; 
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("���Ϳ� �浹");
            GameManager.Inst.GameEnd();
        }
    }

    public void ResetPlayerPos()
    {
        //ó�� ���� ������ġ��
        Inst.transform.position = Inst.beginPos;
        Inst._keyCount = 0;
        Inst.keyCount = 0;
    }
}
