using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum ColType { Down, Top, Left, Right }
public class WallChecker : MonoBehaviour
{
    public BoxCollider2D colDown;
    public BoxCollider2D colTop;
    public BoxCollider2D colLeft;
    public BoxCollider2D colRight;
    public Monster monster;

    public ParticleSystem particle;

    public ColType myColType;
    private void Awake()
    {
        //colDown = GetComponent<BoxCollider2D>();
        monster = GetComponentInParent<Monster>();
        particle = GetComponent<ParticleSystem>();
        
        particle.Stop();
        MakeMyColType();
    }

    private void OnEnable()
    {
        particle.Stop();
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
                
                if(isTouching)
                { particle.Play(); }
                else
                { particle.Stop(); }

                break;
            case ColType.Top:
                Debug.Log("�� �浹 �ν� : " + isTouching);
                monster.isTopWall = isTouching;

                if (isTouching)
                { particle.Play(); }
                else
                { particle.Stop(); }

                break;
            case ColType.Left:
                Debug.Log("���� �浹 �ν� : " + isTouching);
                monster.isLeftWall = isTouching;

                if (isTouching)
                { particle.Play(); }
                else
                { particle.Stop(); }

                break;
            case ColType.Right:
                Debug.Log("������ �浹 �ν� : " + isTouching);
                monster.isRightWall = isTouching;

                if (isTouching)
                { particle.Play(); }
                else
                { particle.Stop(); }

                break;

            default:
                Debug.Log("�ش��ϴ� ��Ÿ���� ����");
                break;
        }
    }
}