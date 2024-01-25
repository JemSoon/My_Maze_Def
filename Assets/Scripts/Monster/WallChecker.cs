using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public BoxCollider2D col;
    public Monster monster;
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        monster = GetComponentInParent<Monster>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� Collider�� ���ϴ� ���̾����� Ȯ��
        if (other.gameObject.layer == 6)
        {
            Debug.Log("�浹��");
            monster.isDownWall = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log("�浹����");
            monster.isDownWall = false;
        }
    }
}
