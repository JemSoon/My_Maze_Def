using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ColType { Down, Top, Left, Right }
public class WallChecker : MonoBehaviour
{
    public BoxCollider2D colDown;
    public BoxCollider2D colTop;
    public BoxCollider2D colLeft;
    public BoxCollider2D colRight;
    public Monster monster;

    public ColType myColType;
    private void Awake()
    {
        //colDown = GetComponent<BoxCollider2D>();
        monster = GetComponentInParent<Monster>();
        MakeMyColType();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� Collider�� ���ϴ� ���̾����� Ȯ��
        if (other.gameObject.layer == 6)
        {
            CheckMyColType(myColType, true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            CheckMyColType(myColType, false);
        }
    }

    void MakeMyColType()
    {
        if(colDown != null)
        {
            myColType = ColType.Down;
        }
        else if(colTop != null)
        {
            myColType = ColType.Top;
        }
        else if(colLeft != null)
        {
            myColType= ColType.Left;
        }
        else if(colRight != null)
        {
            myColType = ColType.Right;
        }
    }

    void CheckMyColType(ColType colType, bool isTouching)
    {
        switch (colType)
        {
            case ColType.Down:
                Debug.Log("�Ʒ� �浹 �ν� : " + isTouching);
                monster.isDownWall = isTouching;
                break;
            case ColType.Top:
                Debug.Log("�� �浹 �ν� : " + isTouching);
                monster.isTopWall = isTouching;
                break;
            case ColType.Left:
                Debug.Log("���� �浹 �ν� : " + isTouching);
                monster.isLeftWall = isTouching;
                break;
            case ColType.Right:
                Debug.Log("������ �浹 �ν� : " + isTouching);
                monster.isRightWall = isTouching;
                break;

            default:
                Debug.Log("�ش��ϴ� ��Ÿ���� ����");
                break;
        }
    }
}
