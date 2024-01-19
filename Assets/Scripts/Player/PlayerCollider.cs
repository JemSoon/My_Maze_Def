using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ColliderType
{
    None = 0,

    Attack,
    Wall,
}
public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider = null;
    [SerializeField] private ColliderType colliderType = ColliderType.None;

    


}
