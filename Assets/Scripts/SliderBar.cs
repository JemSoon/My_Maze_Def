using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    public float coolTimePer;
    Slider slider;
    Player player;

    private void Start()
    {
        player = GameManager.Inst.player;
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (player != null)
        {
            slider.value = player.checkCoolTime / OutGameMoney.Inst.pencilCoolTime;
        }
    }
}
