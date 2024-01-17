using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider = null;
    [SerializeField] private ColliderType colliderType = ColliderType.None;

    public enum ColliderType
    {
        None = 0,

        Attack,
        Wall,
    }


}
