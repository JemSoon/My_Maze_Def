using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public float gameTime;
    public bool isGameOver;

    public PoolManager poolManager;
    public Player player;
    public TextMeshProUGUI penTmp;
    public TextMeshProUGUI goldTmp;

    public GameObject startMenu;
    public GameObject upgradePencilMenu;
    public GameObject makingPencilUI;
    public TextMeshProUGUI pencilCost;
    public TextMeshProUGUI pencilSeconds;
    public Button pencilUpgradeButton;

    public GameObject upgradeFireMenu;
    public TextMeshProUGUI fireCost;
    public TextMeshProUGUI fireSeconds;
    public Button fireUpgradeButton;

    public TextMeshProUGUI goldAmountTmp;
    public GameObject floatingJoystick;

    public GameObject resultMenu;
    public TextMeshProUGUI resultCount;

    private void Awake()
    {
        Inst = this;
        player.OnKeyCountChanged += UpdateKeyCountText;
        player.OnGoldCountChanged += UpdateGoldCountText;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        Inst.isGameOver = true;
        GameEnd();
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        //tmp.text = "열쇠 : "+player.keyCount.ToString();
    }

    private void UpdateKeyCountText(int keyCount)
    {
        // keyCountText의 text 속성을 업데이트
        penTmp.text = keyCount.ToString();
    }
    private void UpdateGoldCountText(int goldCount)
    {
        // goldCountText의 text 속성을 업데이트
        goldTmp.text = "+ "+ goldCount.ToString();
    }

    public void GameEnd()
    {
        SetJoystickColor(floatingJoystick.transform, Color.clear);

        Time.timeScale = 0f;
        player.gameObject.SetActive(false);

        PoolManager pool = FindObjectOfType<PoolManager>();
        pool.ResetPoolManager();//몬스터,총알,근접무기 전부 삭제 초기화

        makingPencilUI.SetActive(false);//연필만드는 슬라이드바 대기중엔 비활성화

        //startMenu.SetActive(true);
        //upgradeMenu.SetActive(true);
        UpgradePencilButtonText();
        UpgradeFireButtonText();

        //OutGameMoney.Inst.money += player.goldCount;
        //goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        //OutGameMoney.Inst.SaveInfo();

        SetButtonSprite();//돈 정산 후 버튼 정보 활성화
    }

    public void ResetGame()
    {
        Inst.isGameOver = false;
        SetJoystickColor(floatingJoystick.transform, Color.white);

        //대기 메뉴 UI닫기
        startMenu.SetActive(false);
        upgradePencilMenu.SetActive(false);
        upgradeFireMenu.SetActive(false);

        //리스타트
        player.gameObject.SetActive(true);
        Time.timeScale = 1f;
        gameTime = 0f;
        player.ResetPlayerPos();
        UpdateKeyCountText(player.keyCount);//UI열쇠개수도 초기화
        FieldManager.Inst.ResetFields();
        makingPencilUI.SetActive(true);//연필 생산 UI활성화

        //★★★몬스터는 풀 매니저쪽에서 삭제중★★★

        //게이트 조건 초기화 + 액티브
        Gate[] gates = FindObjectsOfType<Gate>();
        foreach (Gate gate in gates)
        {
            gate.ResetNeedKey();
        }

        Spawner[] spawners = FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            spawner.ResetSpawnner();
        }

        //PoolManager pool = FindObjectOfType<PoolManager>();
        //pool.ResetPoolManager();//몬스터,총알,근접무기 전부 삭제 초기화
    }

    public void DATAResetButton()
    {
        //데이터 삭제 버튼 누른 후 껏다키면 데이터 삭제됨
        OutGameMoney.Inst.DeleteInfo();
    }

    public void UpgradePencilButtonText()
    {
        if(OutGameMoney.Inst.pencilLevel + 1< OutGameMoney.Inst.pencilItem.cost.Length)
        {
            pencilCost.text = OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1].ToString();
            pencilSeconds.text = OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.pencilLevel + 1].ToString() + "sec";
        }
        else
        {
            pencilCost.text = "Max";
            pencilSeconds.text = "Max";
        }
    }

    public void UpgradeFireButtonText()
    {
        if (OutGameMoney.Inst.fireLevel + 1 < OutGameMoney.Inst.fireRateItem.cost.Length)
        {
           fireCost.text = OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1].ToString();
           fireSeconds.text = OutGameMoney.Inst.fireRateItem.oneForSeconds[OutGameMoney.Inst.fireLevel + 1].ToString() + "sec";
        }
        else
        {
            fireCost.text = "Max";
            fireSeconds.text = "Max";
        }
    }

    public void UpgradePencilButtonClick()
    {
        //현재 설정한 맥시멈 레벨보다 높은 정보를 가져오려 하면 리턴
        if(OutGameMoney.Inst.pencilLevel + 1 >= OutGameMoney.Inst.pencilItem.cost.Length) 
        { return; }

        //다음 레벨 비용보다 돈이 적으면 리턴
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1]) 
        { return; }

        else
        {
            OutGameMoney.Inst.money -= OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1];
            OutGameMoney.Inst.pencilLevel++;
            OutGameMoney.Inst.pencilCoolTime = OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.pencilLevel];
            goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

            OutGameMoney.Inst.SaveInfo();

            UpgradePencilButtonText();
            SetButtonSprite();
        }
    }

    public void UpgradeFireRateButtonClick()
    {
        //현재 설정한 맥시멈 레벨보다 높은 정보를 가져오려 하면 리턴
        if (OutGameMoney.Inst.fireLevel + 1 >= OutGameMoney.Inst.fireRateItem.cost.Length)
        { return; }

        //다음 레벨 비용보다 돈이 적으면 리턴
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1])
        { return; }

        else
        {
            OutGameMoney.Inst.money -= OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1];
            OutGameMoney.Inst.fireLevel++;
            //총 쿨타임은 인게임 들어가 적용되서 이게 필요없다
            //OutGameMoney.Inst.pencilCoolTime = OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.pencilLevel];
            goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

            OutGameMoney.Inst.SaveInfo();

            UpgradeFireButtonText();
            SetButtonSprite();
        }
    }

    public void SetButtonSprite()
    {
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1])
        {
            pencilCost.color = Color.red;
            pencilUpgradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            pencilCost.color = Color.black;
            pencilUpgradeButton.GetComponent<Button>().interactable = true;
        }

        if (OutGameMoney.Inst.money < OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1])
        {
            fireCost.color = Color.red;
            fireUpgradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            fireCost.color = Color.black;
            fireUpgradeButton.GetComponent<Button>().interactable = true;
        }
    }

    public void SetJoystickColor(Transform parent, Color color)
    {
        // 부모의 자식 수만큼 반복
        foreach (Transform child in parent)
        {
            // 자식 오브젝트의 Image 컴포넌트 가져오기
            Image image = child.GetComponent<Image>();

            // Image 컴포넌트가 있으면
            if (image != null)
            {
                // 컬러를 Color.Clear로 설정
                image.color = color;
            }

            // 재귀 호출하여 자식의 자식에 대해서도 동일한 작업 수행
            SetJoystickColor(child, color);
        }
    }
    
    public void CloseResultMenu()
    {
        resultMenu.SetActive(false);

        StartCoroutine(goldCount(player.goldCount));
    }

    IEnumerator goldCount(int currentGet)
    {
        float duration = 0.01f;
        float timer = 0.0f;

        while (currentGet>0)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= duration)
            {
                --currentGet;
                ++OutGameMoney.Inst.money;
                goldAmountTmp.text = (OutGameMoney.Inst.money).ToString();
                timer = 0.0f;
                
            }
            yield return null;
        }

        OutGameMoney.Inst.SaveInfo();

        //나중에 재화 획득 UI창이 뜨고 닫기버튼누르면 씬 로드로 변경
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
