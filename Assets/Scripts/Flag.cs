using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Flag : MonoBehaviour
{
    BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Inst.resultMenu.SetActive(true);
            GameManager.Inst.resultCount.text = GameManager.Inst.player.goldCount.ToString();
            GameManager.Inst.GameEnd();

            string sceneName = SceneManager.GetActiveScene().name;
            int stageNumber = int.Parse(sceneName.Split(' ')[1]);//"Stage 1"을 "Stage"와 "1"로 나눔
            Debug.Log("현재 스테이지 "+stageNumber);

            GameManager.Inst.isStageClear = true;
            //SceneManager.LoadScene("Stage " + (stageNumber + 1));
        }
    }
}
