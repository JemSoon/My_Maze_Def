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
    public TextMeshProUGUI stageTmp;

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

    public GameObject upgradeBulletLevelMenu;
    public TextMeshProUGUI bulletCost;
    public TextMeshProUGUI bulletDamage;
    public Button bulletUpgradeButton;

    public TextMeshProUGUI goldAmountTmp;
    public GameObject floatingJoystick;

    public GameObject resultMenu;
    public TextMeshProUGUI resultCount;

    public bool isStageClear;
    const int maxStage = 2; //마지막 스테이지일시 다음 스테이지 불러오기 막기용(씬 인덱스가 아니라 실제 스테이지 네임 기반)

    public AD_MOB admob;
   
    private void Awake()
    {
        Debug.Log("게임 매니저 어웨이크 호출");
        Inst = this;

        player.OnKeyCountChanged += UpdateKeyCountText;
        player.OnGoldCountChanged += UpdateGoldCountText;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();
        stageTmp.text = SceneManager.GetActiveScene().name;
        isStageClear = false;

        //저장된 스테이지가 맨처음게 아니면 로드
        if (OutGameMoney.Inst.stageLevel != 0 && OutGameMoney.Inst.isSceneLoaded == false)
        {
            OutGameMoney.Inst.SetAsyncLoad(SceneManager.LoadSceneAsync(OutGameMoney.Inst.stageLevel));
            OutGameMoney.Inst.isSceneLoaded = true;
            StartCoroutine(NewStageStart());
        }
        else
        {
            Inst.isGameOver = true;
            GameEnd();
        }

        admob = OutGameMoney.Inst.admob;
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

        UpgradePencilButtonText();
        UpgradeFireButtonText();
        UpgrageBulletLevelText();

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
        upgradeBulletLevelMenu.SetActive(false);

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
            pencilCost.text = OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1].ToString();              //소수점2자리
            pencilSeconds.text = (1 / OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.pencilLevel + 1]).ToString("F2") + "/sec";
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
           fireSeconds.text = (1 / OutGameMoney.Inst.fireRateItem.oneForSeconds[OutGameMoney.Inst.fireLevel + 1]).ToString("F2") + "/sec";
        }
        else
        {
            fireCost.text = "Max";
            fireSeconds.text = "Max";
        }
    }

    public void UpgrageBulletLevelText()
    {
        if(OutGameMoney.Inst.bulletLevel + 1 <OutGameMoney.Inst.bulletItem.cost.Length)
        {
            bulletCost.text = OutGameMoney.Inst.bulletItem.cost[OutGameMoney.Inst.bulletLevel+1].ToString();
            bulletDamage.text = OutGameMoney.Inst.bulletItem.damage[OutGameMoney.Inst.bulletLevel + 1].ToString("F1");
        }
        else
        {
            bulletCost.text = "Max";
            bulletDamage.text = "Max";
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

    public void UpgrageBulletDamageButtonClick()
    {
        //현재 설정한 맥시멈 레벨보다 높은 정보를 가져오려 하면 리턴
        if (OutGameMoney.Inst.bulletLevel + 1 >= OutGameMoney.Inst.bulletItem.cost.Length)
        { return; }

        //다음 레벨 비용보다 돈이 적으면 리턴
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.bulletItem.cost[OutGameMoney.Inst.bulletLevel + 1])
        { return; }

        else
        {
            OutGameMoney.Inst.money -= OutGameMoney.Inst.bulletItem.cost[OutGameMoney.Inst.bulletLevel + 1];
            OutGameMoney.Inst.bulletLevel++;
            
            goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

            OutGameMoney.Inst.SaveInfo();

            UpgrageBulletLevelText();
            SetButtonSprite();
        }
    }

    public void SetButtonSprite()
    {
        if (OutGameMoney.Inst.pencilLevel + 1 < OutGameMoney.Inst.pencilItem.cost.Length && OutGameMoney.Inst.money < OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1])
        {
            pencilCost.color = Color.red;
            pencilUpgradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            pencilCost.color = Color.black;
            pencilUpgradeButton.GetComponent<Button>().interactable = true;
        }


        if (OutGameMoney.Inst.fireLevel + 1 < OutGameMoney.Inst.fireRateItem.cost.Length && OutGameMoney.Inst.money < OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1])
        {
            fireCost.color = Color.red;
            fireUpgradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            fireCost.color = Color.black;
            fireUpgradeButton.GetComponent<Button>().interactable = true;
        }


        if (OutGameMoney.Inst.bulletLevel + 1 < OutGameMoney.Inst.bulletItem.cost.Length && OutGameMoney.Inst.money < OutGameMoney.Inst.bulletItem.cost[OutGameMoney.Inst.bulletLevel + 1])
        {
            bulletCost.color = Color.red;
            bulletUpgradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            bulletCost.color = Color.black;
            bulletUpgradeButton.GetComponent<Button>().interactable = true;
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

        ++OutGameMoney.Inst.showInterstitialAdCount;

        if(OutGameMoney.Inst.showInterstitialAdCount>=3)
        {
            admob.ShowInterstitialAd();
            OutGameMoney.Inst.showInterstitialAdCount = 0;
        }
        else
        {
            //세번째 아니면 걍 닫고 돈정산
            StartCoroutine(goldCount(player.goldCount));
        }
    }

    public IEnumerator goldCount(int currentGet)
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

        yield return null;

        //나중에 재화 획득 UI창이 뜨고 닫기버튼누르면 씬 로드로 변경
        if (isStageClear)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int stageNumber = int.Parse(sceneName.Split(' ')[1]);//"Stage 1"을 띄어쓰기 기준 "Stage"와 "1"로 나눔 그중 두번째 인덱스인[1]("1")을 가져오
            
            if(stageNumber + 1 <= maxStage)
            {
                //마지막 스테이지가 아니라면 다음 스테이지 불러오기
                SceneManager.LoadScene("Stage " + (stageNumber + 1));
                //그리고 저장
                OutGameMoney.Inst.SaveStage(stageNumber);
            }
            else
            {
                //최종 스테이지라면 그냥 현 스테이지 다시 불러오기
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.buildIndex);
            }
            //추가적으로 스테이지 단계 저장해야함
            isStageClear = false;

        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }

    public IEnumerator gold2Count(int currentGet)
    {
        float duration = 0.01f;
        float timer = 0.0f;

        while (currentGet > 0)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= duration)
            {
                --currentGet;
                OutGameMoney.Inst.money+=(1*2);//2배 보상!
                goldAmountTmp.text = (OutGameMoney.Inst.money).ToString();
                timer = 0.0f;

            }
            yield return null;
        }

        OutGameMoney.Inst.SaveInfo();

        yield return null;

        //나중에 재화 획득 UI창이 뜨고 닫기버튼누르면 씬 로드로 변경
        if (isStageClear)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int stageNumber = int.Parse(sceneName.Split(' ')[1]);//"Stage 1"을 띄어쓰기 기준 "Stage"와 "1"로 나눔 그중 두번째 인덱스인[1]("1")을 가져오

            if (stageNumber + 1 <= maxStage)
            {
                //마지막 스테이지가 아니라면 다음 스테이지 불러오기
                SceneManager.LoadScene("Stage " + (stageNumber + 1));
                //그리고 저장
                OutGameMoney.Inst.SaveStage(stageNumber);
            }
            else
            {
                //최종 스테이지라면 그냥 현 스테이지 다시 불러오기
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.buildIndex);
            }
            //추가적으로 스테이지 단계 저장해야함
            isStageClear = false;

        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }

    IEnumerator NewStageStart()
    {
        // 씬 로딩이 완료될 때까지 대기
        while (!OutGameMoney.Inst.asyncLoad.isDone)
        {
            yield return null;
        }

        // 씬 로딩이 완료된 후에 수행할 작업
        Inst.isGameOver = true;
        GameEnd();
        OutGameMoney.Inst.asyncLoad = null;
    }
    
    public void ShowRewardAD()
    {
        OutGameMoney.Inst.ADButtonClick();
    }

    //몬스터 HP 디버깅용
    public void DebugMonsterHPshow()
    {
        OutGameMoney.Inst.monHPshow = !OutGameMoney.Inst.monHPshow;

        //현재 소환되있는 애들도 ui상태 변경
        Monster[] monsters = FindObjectsOfType<Monster>();
        // 모든 Monster 컴포넌트를 순회하며 monHP를 0으로 설정한다.
        foreach (Monster monster in monsters)
        {
            monster.tmp.gameObject.SetActive(OutGameMoney.Inst.monHPshow);
        }
    }
}
