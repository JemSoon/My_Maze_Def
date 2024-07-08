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
    const int maxStage = 2; //������ ���������Ͻ� ���� �������� �ҷ����� �����(�� �ε����� �ƴ϶� ���� �������� ���� ���)

    public AD_MOB admob;
   
    private void Awake()
    {
        Debug.Log("���� �Ŵ��� �����ũ ȣ��");
        Inst = this;

        player.OnKeyCountChanged += UpdateKeyCountText;
        player.OnGoldCountChanged += UpdateGoldCountText;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();
        stageTmp.text = SceneManager.GetActiveScene().name;
        isStageClear = false;

        //����� ���������� ��ó���� �ƴϸ� �ε�
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
        //tmp.text = "���� : "+player.keyCount.ToString();
    }

    private void UpdateKeyCountText(int keyCount)
    {
        // keyCountText�� text �Ӽ��� ������Ʈ
        penTmp.text = keyCount.ToString();
    }
    private void UpdateGoldCountText(int goldCount)
    {
        // goldCountText�� text �Ӽ��� ������Ʈ
        goldTmp.text = "+ "+ goldCount.ToString();
    }

    public void GameEnd()
    {
        SetJoystickColor(floatingJoystick.transform, Color.clear);

        Time.timeScale = 0f;
        player.gameObject.SetActive(false);

        PoolManager pool = FindObjectOfType<PoolManager>();
        pool.ResetPoolManager();//����,�Ѿ�,�������� ���� ���� �ʱ�ȭ

        makingPencilUI.SetActive(false);//���ʸ���� �����̵�� ����߿� ��Ȱ��ȭ

        UpgradePencilButtonText();
        UpgradeFireButtonText();
        UpgrageBulletLevelText();

        SetButtonSprite();//�� ���� �� ��ư ���� Ȱ��ȭ
    }

    public void ResetGame()
    {
        Inst.isGameOver = false;
        SetJoystickColor(floatingJoystick.transform, Color.white);

        //��� �޴� UI�ݱ�
        startMenu.SetActive(false);
        upgradePencilMenu.SetActive(false);
        upgradeFireMenu.SetActive(false);
        upgradeBulletLevelMenu.SetActive(false);

        //����ŸƮ
        player.gameObject.SetActive(true);
        Time.timeScale = 1f;
        gameTime = 0f;
        player.ResetPlayerPos();
        UpdateKeyCountText(player.keyCount);//UI���谳���� �ʱ�ȭ
        FieldManager.Inst.ResetFields();
        makingPencilUI.SetActive(true);//���� ���� UIȰ��ȭ

        //�ڡڡڸ��ʹ� Ǯ �Ŵ����ʿ��� �����ߡڡڡ�

        //����Ʈ ���� �ʱ�ȭ + ��Ƽ��
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
        //pool.ResetPoolManager();//����,�Ѿ�,�������� ���� ���� �ʱ�ȭ
    }

    public void DATAResetButton()
    {
        //������ ���� ��ư ���� �� ����Ű�� ������ ������
        OutGameMoney.Inst.DeleteInfo();
    }

    public void UpgradePencilButtonText()
    {
        if(OutGameMoney.Inst.pencilLevel + 1< OutGameMoney.Inst.pencilItem.cost.Length)
        {
            pencilCost.text = OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.pencilLevel + 1].ToString();              //�Ҽ���2�ڸ�
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
        //���� ������ �ƽø� �������� ���� ������ �������� �ϸ� ����
        if(OutGameMoney.Inst.pencilLevel + 1 >= OutGameMoney.Inst.pencilItem.cost.Length) 
        { return; }

        //���� ���� ��뺸�� ���� ������ ����
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
        //���� ������ �ƽø� �������� ���� ������ �������� �ϸ� ����
        if (OutGameMoney.Inst.fireLevel + 1 >= OutGameMoney.Inst.fireRateItem.cost.Length)
        { return; }

        //���� ���� ��뺸�� ���� ������ ����
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1])
        { return; }

        else
        {
            OutGameMoney.Inst.money -= OutGameMoney.Inst.fireRateItem.cost[OutGameMoney.Inst.fireLevel + 1];
            OutGameMoney.Inst.fireLevel++;
            //�� ��Ÿ���� �ΰ��� �� ����Ǽ� �̰� �ʿ����
            //OutGameMoney.Inst.pencilCoolTime = OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.pencilLevel];
            goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

            OutGameMoney.Inst.SaveInfo();

            UpgradeFireButtonText();
            SetButtonSprite();
        }
    }

    public void UpgrageBulletDamageButtonClick()
    {
        //���� ������ �ƽø� �������� ���� ������ �������� �ϸ� ����
        if (OutGameMoney.Inst.bulletLevel + 1 >= OutGameMoney.Inst.bulletItem.cost.Length)
        { return; }

        //���� ���� ��뺸�� ���� ������ ����
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
        // �θ��� �ڽ� ����ŭ �ݺ�
        foreach (Transform child in parent)
        {
            // �ڽ� ������Ʈ�� Image ������Ʈ ��������
            Image image = child.GetComponent<Image>();

            // Image ������Ʈ�� ������
            if (image != null)
            {
                // �÷��� Color.Clear�� ����
                image.color = color;
            }

            // ��� ȣ���Ͽ� �ڽ��� �ڽĿ� ���ؼ��� ������ �۾� ����
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
            //����° �ƴϸ� �� �ݰ� ������
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

        //���߿� ��ȭ ȹ�� UIâ�� �߰� �ݱ��ư������ �� �ε�� ����
        if (isStageClear)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int stageNumber = int.Parse(sceneName.Split(' ')[1]);//"Stage 1"�� ���� ���� "Stage"�� "1"�� ���� ���� �ι�° �ε�����[1]("1")�� ������
            
            if(stageNumber + 1 <= maxStage)
            {
                //������ ���������� �ƴ϶�� ���� �������� �ҷ�����
                SceneManager.LoadScene("Stage " + (stageNumber + 1));
                //�׸��� ����
                OutGameMoney.Inst.SaveStage(stageNumber);
            }
            else
            {
                //���� ����������� �׳� �� �������� �ٽ� �ҷ�����
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.buildIndex);
            }
            //�߰������� �������� �ܰ� �����ؾ���
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
                OutGameMoney.Inst.money+=(1*2);//2�� ����!
                goldAmountTmp.text = (OutGameMoney.Inst.money).ToString();
                timer = 0.0f;

            }
            yield return null;
        }

        OutGameMoney.Inst.SaveInfo();

        yield return null;

        //���߿� ��ȭ ȹ�� UIâ�� �߰� �ݱ��ư������ �� �ε�� ����
        if (isStageClear)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int stageNumber = int.Parse(sceneName.Split(' ')[1]);//"Stage 1"�� ���� ���� "Stage"�� "1"�� ���� ���� �ι�° �ε�����[1]("1")�� ������

            if (stageNumber + 1 <= maxStage)
            {
                //������ ���������� �ƴ϶�� ���� �������� �ҷ�����
                SceneManager.LoadScene("Stage " + (stageNumber + 1));
                //�׸��� ����
                OutGameMoney.Inst.SaveStage(stageNumber);
            }
            else
            {
                //���� ����������� �׳� �� �������� �ٽ� �ҷ�����
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.buildIndex);
            }
            //�߰������� �������� �ܰ� �����ؾ���
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
        // �� �ε��� �Ϸ�� ������ ���
        while (!OutGameMoney.Inst.asyncLoad.isDone)
        {
            yield return null;
        }

        // �� �ε��� �Ϸ�� �Ŀ� ������ �۾�
        Inst.isGameOver = true;
        GameEnd();
        OutGameMoney.Inst.asyncLoad = null;
    }
    
    public void ShowRewardAD()
    {
        OutGameMoney.Inst.ADButtonClick();
    }

    //���� HP ������
    public void DebugMonsterHPshow()
    {
        OutGameMoney.Inst.monHPshow = !OutGameMoney.Inst.monHPshow;

        //���� ��ȯ���ִ� �ֵ鵵 ui���� ����
        Monster[] monsters = FindObjectsOfType<Monster>();
        // ��� Monster ������Ʈ�� ��ȸ�ϸ� monHP�� 0���� �����Ѵ�.
        foreach (Monster monster in monsters)
        {
            monster.tmp.gameObject.SetActive(OutGameMoney.Inst.monHPshow);
        }
    }
}
