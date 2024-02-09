using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonTextPos : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject[] gameobj;
    public Vector2 pressedTextOffset;

    public List<Vector2> originalTextPosition;

    private bool isPressed = false;

    private void Start()
    {
        // 텍스트의 원래 위치 기록
        for(int i = 0; i < gameobj.Length; i++)
        {
            originalTextPosition.Add(gameobj[i].GetComponent<RectTransform>().anchoredPosition);
        }
        // 버튼의 Pressed 이벤트에 함수 연결
        this.GetComponent<Button>().onClick.AddListener(AdjustTextPosition);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        AdjustTextPosition();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        AdjustTextPosition();
    }

    private void AdjustTextPosition()
    {
        for(int i =0; i<gameobj.Length; i++)
        {
            if (isPressed)
            {
                gameobj[i].GetComponent<RectTransform>().anchoredPosition = originalTextPosition[i] + pressedTextOffset;
            }
            else
            {
                gameobj[i].GetComponent<RectTransform>().anchoredPosition = originalTextPosition[i];
            }
        }
    }
}
