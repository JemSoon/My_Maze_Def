using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public float gameTime;

    public PoolManager poolManager;
    public Player player;
    public TextMeshProUGUI tmp;

    private void Awake()
    {
        Inst = this;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        tmp.text = "¿­¼è : "+player.keyCount.ToString();
    }
}
