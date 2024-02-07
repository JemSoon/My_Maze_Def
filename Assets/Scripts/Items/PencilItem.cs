using System;
using UnityEngine;

[Serializable]
public class PencilItem : MonoBehaviour
{
    [Header("단계별 업글 가격 / 한개당 생산 속도")]
    public int[] cost;
    public float[] oneForSeconds;
}
