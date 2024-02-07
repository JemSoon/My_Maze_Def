using Cinemachine;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public float gameTime;

    public PoolManager poolManager;
    public Player player;
    public TextMeshProUGUI penTmp;
    public TextMeshProUGUI goldTmp;
    //public bool isGameOver;
    public GameObject resultMenu;
    public GameObject upgradeMenu;
    public GameObject makingPencilUI;
    public TextMeshProUGUI pencilCost;
    public TextMeshProUGUI pencilSeconds;

    public TextMeshProUGUI goldAmountTmp;

    private void Awake()
    {
        Inst = this;
        player.OnKeyCountChanged += UpdateKeyCountText;
        player.OnGoldCountChanged += UpdateGoldCountText;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

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
        Time.timeScale = 0f;
        player.gameObject.SetActive(false);

        PoolManager pool = FindObjectOfType<PoolManager>();
        pool.ResetPoolManager();//몬스터,총알,근접무기 전부 삭제 초기화

        makingPencilUI.SetActive(false);//연필만드는 슬라이드바 대기중엔 비활성화

        resultMenu.SetActive(true);
        upgradeMenu.SetActive(true);
        UpgradePencilButtonText();

        OutGameMoney.Inst.money += player.goldCount;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        OutGameMoney.Inst.SaveInfo();
    }

    public void ResetGame()
    {
        //대기 메뉴 UI닫기
        resultMenu.SetActive(false);
        upgradeMenu.SetActive(false);

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
        if(OutGameMoney.Inst.level + 1< OutGameMoney.Inst.pencilItem.cost.Length)
        {
            pencilCost.text = "다음 단계 비용 : " + OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.level + 1].ToString();
            pencilSeconds.text = "1개당 : " + OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.level + 1].ToString() + "초";
        }
        else
        {
            pencilCost.text = "최대 레벨";
            pencilSeconds.text = "최대 레벨";
        }
        
    }

    public void UpgradePencilButtonClick()
    {
        //현재 설정한 맥시멈 레벨보다 높은 정보를 가져오려 하면 리턴
        if(OutGameMoney.Inst.level + 1 >= OutGameMoney.Inst.pencilItem.cost.Length) 
        { return; }

        //다음 레벨 비용보다 돈이 적으면 리턴
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.level + 1]) 
        { return; }

        else
        {
            OutGameMoney.Inst.money -= OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.level + 1];
            OutGameMoney.Inst.level++;
            OutGameMoney.Inst.pencilCoolTime = OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.level];
            goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

            OutGameMoney.Inst.SaveInfo();

            UpgradePencilButtonText();
        }
    }
}
