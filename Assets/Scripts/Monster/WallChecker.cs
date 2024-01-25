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
        // 충돌한 Collider가 원하는 레이어인지 확인
        if (other.gameObject.layer == 6)
        {
            Debug.Log("충돌중");
            monster.isDownWall = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log("충돌나감");
            monster.isDownWall = false;
        }
    }
}
