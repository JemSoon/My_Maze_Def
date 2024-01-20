using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public float gameTime;

    public PoolManager poolManager;
    public Player player;

    private void Awake()
    {
        Inst = this;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }
}
