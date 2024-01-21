using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� �ֺ��� ���� �׷��� ���� �����ϴ� ��ĳ��
public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //ĳ������ ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;

        //�Ÿ�(�� �ݰ溸�� ������ ������)
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; //�÷��̾� ��ġ
            Vector3 targetPos = target.transform.position; //���� ��ġ
            float curDiff = Vector3.Distance(myPos, targetPos);//���� ������ �Ÿ� ��

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
