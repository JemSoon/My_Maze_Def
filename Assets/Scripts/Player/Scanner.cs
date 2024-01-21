using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 주변에 원을 그려서 적을 인지하는 스캐너
public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //캐스팅의 시작 위치, 범위 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;

        //거리(이 반경보다 작으면 가져옴)
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; //플레이어 위치
            Vector3 targetPos = target.transform.position; //몬스터 위치
            float curDiff = Vector3.Distance(myPos, targetPos);//나와 몬스터의 거리 값

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
