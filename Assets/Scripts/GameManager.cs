using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    public PoolManager poolManager;
    public Player player;

    private void Awake()
    {
        Inst = this;
    }
}
