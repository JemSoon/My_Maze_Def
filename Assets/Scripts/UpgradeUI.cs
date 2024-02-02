using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    Vector3 pos;
    private void Start()
    {
        pos = this.GetComponent<RectTransform>().anchoredPosition;
    }
    public void CloseUpgradeUI()
    {
        GameManager.Inst.checkUpgrade = true;
        this.gameObject.SetActive(false);
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
