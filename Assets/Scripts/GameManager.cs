using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    public float gameTime;
    public bool isGameOver;

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
    public Button pencilUpgradeButton;

    public TextMeshProUGUI goldAmountTmp;
    public GameObject floatingJoystick;

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

        resultMenu.SetActive(true);
        upgradeMenu.SetActive(true);
        UpgradePencilButtonText();

        OutGameMoney.Inst.money += player.goldCount;
        goldAmountTmp.text = OutGameMoney.Inst.money.ToString();

        OutGameMoney.Inst.SaveInfo();

        SetButtonSprite();//�� ���� �� ��ư ���� Ȱ��ȭ
    }

    public void ResetGame()
    {
        Inst.isGameOver = false;
        SetJoystickColor(floatingJoystick.transform, Color.white);

        //��� �޴� UI�ݱ�
        resultMenu.SetActive(false);
        upgradeMenu.SetActive(false);

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
        if(OutGameMoney.Inst.level + 1< OutGameMoney.Inst.pencilItem.cost.Length)
        {
            pencilCost.text = OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.level + 1].ToString();
            pencilSeconds.text = OutGameMoney.Inst.pencilItem.oneForSeconds[OutGameMoney.Inst.level + 1].ToString() + "Sec";
        }
        else
        {
            pencilCost.text = "Max";
            pencilSeconds.text = "Max";
        }
        
    }

    public void UpgradePencilButtonClick()
    {
        //���� ������ �ƽø� �������� ���� ������ �������� �ϸ� ����
        if(OutGameMoney.Inst.level + 1 >= OutGameMoney.Inst.pencilItem.cost.Length) 
        { return; }

        //���� ���� ��뺸�� ���� ������ ����
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
            SetButtonSprite();
        }
    }

    public void SetButtonSprite()
    {
        if (OutGameMoney.Inst.money < OutGameMoney.Inst.pencilItem.cost[OutGameMoney.Inst.level + 1])
        {
            pencilCost.color = Color.red;
            pencilUpgradeButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            pencilCost.color = Color.black;
            pencilUpgradeButton.GetComponent<Button>().interactable = true;
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
    
}
