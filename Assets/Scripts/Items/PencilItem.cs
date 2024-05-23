using System;
using UnityEngine;

[Serializable]
public class PencilItem : MonoBehaviour
{
    [Header("Step-by-step upgrade price / one-time launch term")]
    public int[] cost;
    public float[] oneForSeconds;
}
