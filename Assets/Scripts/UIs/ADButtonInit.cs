using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADButtonInit : MonoBehaviour
{
    Button adButton;
    // Start is called before the first frame update
    void Start()
    {
        adButton = GetComponent<Button>();
        adButton.onClick.AddListener(OutGameMoney.Inst.ADButtonClick);
    }
}
