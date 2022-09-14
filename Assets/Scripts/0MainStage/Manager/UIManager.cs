using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private ChoheeController m_Player;
    private NearItemCheck m_Item;
    private NearNpcCheck m_Npc;
    private AudioClip m_ButtonSound;

    public Text m_NearNameText;
    public Image m_PlayerHealth;
    public Image m_PlayerMask;
    public Image[] m_PlayerSkill;
    public Text m_SoulCount;

    public Image m_BloodScreen;
    public Image m_DeathScreen;
    public Image m_GameOverScreen;
    public GameObject[] m_GameOverButton;
    public bool m_BloodPlaying;

    public GameObject m_PausePanel;
    public GameObject m_ExitPanel;
    public GameObject m_WeaponPanel;
    public GameObject m_ArmorPanel;
    public GameObject m_GatePanel;
    public GameObject m_StationPanel;
    public RectTransform m_WeaponMenu;
    public RectTransform m_ArmorMenu;
    public RectTransform m_GateMenu;
    public RectTransform m_StationMenu;
    public Text m_WeaponPanelText;
    public Text m_ArmorPanelText;
    public Text m_GatePanelText;
    public Text m_StationPanelText;

    public GameObject m_TalkPanel;
    public RectTransform m_TalkImage;
    public Text m_TalkText;
    public Text m_TalkNpcNameText;
    public Image m_TalkNpcNameImage;
    public Sprite[] m_CurTalkNpcImage;

    public GameObject[] m_PauseMenus;
    public GameObject[] m_PauseIcon;
    public GameObject[] m_KeyHelpersMenus;
    public GameObject[] m_SettingText;
    public Image[] m_BGM;
    public Image[] m_SFX;

    int m_SettingMenuNumber;
    private Sprite m_SettingGageOn;
    private Sprite m_SettingGageOff;

    /// <summary>
    /// 0 = Life == 4,
    /// 1 = Life == 3,
    /// 2 = Life == 2,
    /// 3 = Life == 1
    /// </summary>
    [SerializeField]
    private Sprite[] m_LifeImage;

    /// <summary>
    /// 0 = null
    /// 1 = Normal
    /// 2 = Speed
    /// 3 = Fire
    /// 4 = Ice
    /// </summary>
    [SerializeField]
    private Sprite[] m_CurMask;

    /// <summary>
    /// 0 = False
    /// 1 = Ready
    /// 2 = True
    /// </summary>
    [SerializeField]
    private Sprite[] m_SkillGage;

    private static UIManager m_instance;
    // �̱���
    public static UIManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (m_instance == null)
                    Debug.Log("No Singletone Obj");
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();
        m_Item = GameObject.FindWithTag("Player").GetComponent<NearItemCheck>();
        m_Npc = GameObject.FindWithTag("Player").GetComponent<NearNpcCheck>();

        m_LifeImage[0] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_4");
        m_LifeImage[1] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_3");
        m_LifeImage[2] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_2");
        m_LifeImage[3] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_1");

        //m_CurMask[0] = Resources.Load<Sprite>("7Textures/06_05UITexture/MaskFrame");
        m_CurMask[0] = Resources.Load<Sprite>("7Textures/UI/06_08Add/Mask/NullImage");
        m_CurMask[1] = Resources.Load<Sprite>("7Textures/UI/06_08Add/Mask/NormalMask");
        m_CurMask[2] = Resources.Load<Sprite>("7Textures/UI/06_08Add/Mask/SpeedMask");
        m_CurMask[3] = Resources.Load<Sprite>("7Textures/UI/06_08Add/Mask/FireMask");
        m_CurMask[4] = Resources.Load<Sprite>("7Textures/UI/06_08Add/Mask/IceMask");

        m_SkillGage[0] = Resources.Load<Sprite>("7Textures/06_05UITexture/Skill_False");
        m_SkillGage[1] = Resources.Load<Sprite>("7Textures/06_05UITexture/Skill_Ready");
        m_SkillGage[2] = Resources.Load<Sprite>("7Textures/06_05UITexture/Skill_True");

        m_ButtonSound = Resources.Load<AudioClip>("8SoundResources/06_08SFX/volume_sound");
        m_SettingGageOn = Resources.Load<Sprite>("7Textures/UI/Settings/GageOn");
        m_SettingGageOff = Resources.Load<Sprite>("7Textures/UI/Settings/GageOff");

        SFXSoundReCheck(SoundManager.Instance.m_SFXGage);
        BGMSoundReCheck(SoundManager.Instance.m_BGMGage);
    }

    void Update()
    {
        NearInfo();
        HealthCtrl();
        MaskCtrl();
        SFXSoundCheck();
        BGMSoundCheck();
        SkillGageCtrl();
        SoulCount();
        SettingMenu();
        PauseFalse();

        if (!AllGameManager.Instance.isTalkAction && m_TalkImage.anchoredPosition.y == -1000)
            m_TalkPanel.SetActive(false);
    }

    public void ButtonSound()
    {
        SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
    }

    void NearInfo()
    {
        if (m_Item.m_NearItem)
        {
            if (m_Item.m_NearItem.name == "FireMask_Item")
                m_NearNameText.text = "���� Ż ȹ���ϱ�" + "<color=#FFE400>" + " (E)" + "</color>";
            else if (m_Item.m_NearItem.name == "IceMask_Item")
                m_NearNameText.text = "��� Ż ȹ���ϱ�" + "<color=#FFE400>" + " (E)" + "</color>";
            else if (m_Item.m_NearItem.name == "SpeedMask_Item")
                m_NearNameText.text = "�ʷ��� Ż ȹ���ϱ�" + "<color=#FFE400>" + " (E)" + "</color>";
            else if (m_Item.m_NearItem.name == "NormalMask_Item")
                m_NearNameText.text = "��� Ż ȹ���ϱ�" + "<color=#FFE400>" + " (E)" + "</color>";
        }
        else if (m_Npc.m_NearNpc)
        {
            m_NearNameText.text = m_Npc.m_NearNpc.name + "�� ��ȭ�ϱ�" + "<color=#FFE400>" + " (E)" + "</color>";
        }
        else
            m_NearNameText.text = null;
    }

    void HealthCtrl()
    {
        int CurHealth = (int)PlayerStats.Instance.Health;
        if(PlayerStats.Instance.Health <= 4 && PlayerStats.Instance.Health >= 1)
            m_PlayerHealth.sprite = m_LifeImage[CurHealth - 1];
    }

    void MaskCtrl()
    {
        if (ChoheeWeapon.Instance.MaskType == "Normal")
            m_PlayerMask.sprite = m_CurMask[1];
        else if (ChoheeWeapon.Instance.MaskType == "Speed")
            m_PlayerMask.sprite = m_CurMask[2];
        else if (ChoheeWeapon.Instance.MaskType == "Fire")
            m_PlayerMask.sprite = m_CurMask[3];
        else if (ChoheeWeapon.Instance.MaskType == "Ice")
            m_PlayerMask.sprite = m_CurMask[4];
        else
            m_PlayerMask.sprite = m_CurMask[0];
    }

    void SkillGageCtrl()
    {
        int CurSlash = (int)PlayerStats.Instance.Slash;
        if (PlayerStats.Instance.Slash == CurSlash && !m_PlayerSkill[0])
        {
            for (int i = 1; i < 5; i++)
            {
                m_PlayerSkill[i].sprite = m_SkillGage[0];

                if(i == CurSlash)
                    m_PlayerSkill[CurSlash].sprite = m_SkillGage[2];

                else if(i < CurSlash)
                    m_PlayerSkill[i].sprite = m_SkillGage[1];
            }
        }
    }

    void SoulCount()
    {
        m_SoulCount.text = PlayerStats.Instance.Soul.ToString();
        
    }

    public IEnumerator BloodScreen(float ExitTime)
    {
        m_BloodPlaying = true;

        Color BScolor = m_BloodScreen.color;

        float m_Time = 0f;

        BScolor.a = Mathf.Lerp(1, 0, m_Time);

        while(BScolor.a > 0f)
        {
            m_Time += Time.deltaTime / ExitTime;

            BScolor.a = Mathf.Lerp(1, 0, m_Time);

            m_BloodScreen.color = BScolor;

            yield return null;
        }

        m_BloodPlaying = false;
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        m_PauseIcon[0].SetActive(true);
        m_PausePanel.SetActive(true);
        AllGameManager.Instance.isPause = true;
        
    }

    void PauseFalse()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && AllGameManager.Instance.isPause)
        {
            Time.timeScale = 1f;
            m_PausePanel.SetActive(false);
            m_ExitPanel.SetActive(false);
            AllGameManager.Instance.isPause = false;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !AllGameManager.Instance.isPause)
        {
            Time.timeScale = 0f;
            m_ExitPanel.SetActive(true);
            AllGameManager.Instance.isPause = true;
        }
    }

    public void PauseChagneMenu(int MenuNumber)
    {
        SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);

        for (int i = 0; i < 3; i++)
        {
            if (i == MenuNumber)
            {
                m_PauseMenus[i].SetActive(true);
                m_PauseIcon[i].SetActive(true);
            }
            else
            {
                m_PauseMenus[i].SetActive(false);
                m_PauseIcon[i].SetActive(false);
            }
        }
    }

    void SettingMenu()
    {
        if(Input.GetKeyUp(KeyCode.UpArrow) && m_SettingMenuNumber > -1 && m_SettingMenuNumber < 2)
        {
            m_SettingMenuNumber++;
            SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);
            SettingTextCtrl(m_SettingMenuNumber);
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow) && m_SettingMenuNumber > 0 && m_SettingMenuNumber < 3)
        {
            m_SettingMenuNumber--;
            SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);
            SettingTextCtrl(m_SettingMenuNumber);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (m_SettingMenuNumber == 1 && SoundManager.Instance.m_SFXGage > -1 && SoundManager.Instance.m_SFXGage < 5)
            {
                SoundManager.Instance.m_SFXGage++;
                SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);
                SettingGageCtrl(SoundManager.Instance.m_SFXGage);
            }
            else if (m_SettingMenuNumber == 2 && SoundManager.Instance.m_BGMGage > -1 && SoundManager.Instance.m_BGMGage < 5)
            {
                SoundManager.Instance.m_BGMGage++;
                SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);
                SettingGageCtrl(SoundManager.Instance.m_BGMGage);
            }
            else
                return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (m_SettingMenuNumber == 1 && SoundManager.Instance.m_SFXGage > 0 && SoundManager.Instance.m_SFXGage < 6)
            {
                SoundManager.Instance.m_SFXGage--;
                SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);
                SettingGageCtrl(SoundManager.Instance.m_SFXGage);
            }
            else if (m_SettingMenuNumber == 2 && SoundManager.Instance.m_BGMGage > 0 && SoundManager.Instance.m_BGMGage < 6)
            {
                SoundManager.Instance.m_BGMGage--;
                SoundManager.Instance.SFXPlay("Button", AllGameManager.Instance.m_Clip[2]);
                SettingGageCtrl(SoundManager.Instance.m_BGMGage);
            }
            else
                return;
        }
    }

    void SettingTextCtrl(int Gage)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == Gage)
            {
                m_SettingText[i].SetActive(true);
            }
            else
            {
                m_SettingText[i].SetActive(false);
            }
        }
    }

    void SettingGageCtrl(int Gage)
    {
        if(!m_SFX[0] && !m_BGM[0])
        {
            for (int i = Gage; i < 6; i++)
            {
                if (m_SettingMenuNumber == 1)
                {
                    if(i == 0)
                        m_SFX[1].sprite = m_SettingGageOff;
                    else if (i == Gage)
                        m_SFX[i].sprite = m_SettingGageOn;
                    else if (i > Gage)
                        m_SFX[i].sprite = m_SettingGageOff;
                }

                else if (m_SettingMenuNumber == 2)
                {
                    if (i == 0)
                        m_BGM[1].sprite = m_SettingGageOff;
                    else if (i == Gage)
                        m_BGM[i].sprite = m_SettingGageOn;
                    else if (i > Gage)
                        m_BGM[i].sprite = m_SettingGageOff;
                }
            }
        }      
    }

    void SFXSoundReCheck(int Gage)
    {
        for (int i = Gage; i < 6; i++)
        {
            {
                if (i == 0)
                    m_SFX[1].sprite = m_SettingGageOff;
                else if (i == Gage)
                    m_SFX[i].sprite = m_SettingGageOn;
                else if (i > Gage)
                    m_SFX[i].sprite = m_SettingGageOff;
            }
        }
    }

    void BGMSoundReCheck(int Gage)
    {
        for (int i = Gage; i < 6; i++)
        {
            {
                if (i == 0)
                    m_BGM[1].sprite = m_SettingGageOff;
                else if (i == Gage)
                    m_BGM[i].sprite = m_SettingGageOn;
                else if (i > Gage)
                    m_BGM[i].sprite = m_SettingGageOff;
            }
        }
    }

    public void KeyHelpersMouse(int MouseNumber)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == MouseNumber)
                m_KeyHelpersMenus[i].SetActive(true);
            else
                m_KeyHelpersMenus[i].SetActive(false);
        }
    }

    void SFXSoundCheck()
    {
        if (SoundManager.Instance.m_SFXGage == 0)
            SoundManager.Instance.SFXSoundVolume(-80);
        else if (SoundManager.Instance.m_SFXGage == 1)
            SoundManager.Instance.SFXSoundVolume(-20);
        else if (SoundManager.Instance.m_SFXGage == 2)
            SoundManager.Instance.SFXSoundVolume(-10);
        else if (SoundManager.Instance.m_SFXGage == 3)
            SoundManager.Instance.SFXSoundVolume(-5);
        else if (SoundManager.Instance.m_SFXGage == 4)
            SoundManager.Instance.SFXSoundVolume(0);
        else if (SoundManager.Instance.m_SFXGage == 5)
            SoundManager.Instance.SFXSoundVolume(10);
    }

    void BGMSoundCheck()
    {
        if (SoundManager.Instance.m_BGMGage == 0)
            SoundManager.Instance.BGSoundVolume(-80);
        else if (SoundManager.Instance.m_BGMGage == 1)
            SoundManager.Instance.BGSoundVolume(-20);
        else if (SoundManager.Instance.m_BGMGage == 2)
            SoundManager.Instance.BGSoundVolume(-10);
        else if (SoundManager.Instance.m_BGMGage == 3)
            SoundManager.Instance.BGSoundVolume(-5);
        else if (SoundManager.Instance.m_BGMGage == 4)
            SoundManager.Instance.BGSoundVolume(0);
        else if (SoundManager.Instance.m_BGMGage == 5)
            SoundManager.Instance.BGSoundVolume(10);
    }

    public void ExitButtonCtrl(int Number)
    {
        if (Number == 0)
            Debug.Log("Game Exit");
        else if (Number == 1)
        {
            Time.timeScale = 1f;
            m_ExitPanel.SetActive(false);
            AllGameManager.Instance.isPause = false;
        }
    }

    public void SmithySystem(bool isShop, string NpcName)
    {
        if(isShop && NpcName == "WeaponNpc")
        {
            SoundManager.Instance.SFXPlay("Open Shop", AllGameManager.Instance.m_Clip[8]);

            m_WeaponPanel.SetActive(true);

            m_WeaponMenu.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }
        else if (isShop && NpcName == "ArmorNpc")
        {
            SoundManager.Instance.SFXPlay("Open Shop", AllGameManager.Instance.m_Clip[8]);

            m_ArmorPanel.SetActive(true);

            m_ArmorMenu.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }
        else if (isShop && NpcName == "Gate Keeper")
        {
            SoundManager.Instance.SFXPlay("Open Shop", AllGameManager.Instance.m_Clip[8]);

            m_GatePanel.SetActive(true);

            m_GateMenu.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }
        /*else if (isShop && NpcName == "Station Master")
        {
            m_StationPanel.SetActive(true);

            m_StationMenu.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }*/
        else if(!isShop && NpcName == "WeaponNpc")
        {
            m_WeaponMenu.anchoredPosition = Vector3.up * 1000;

            m_WeaponPanel.SetActive(false);
        }
        else if (!isShop && NpcName == "ArmorNpc")
        {
            m_ArmorMenu.anchoredPosition = Vector3.up * 1000;

            m_ArmorPanel.SetActive(false);
        }
        else if (!isShop && NpcName == "Gate Keeper")
        {
            m_GateMenu.anchoredPosition = Vector3.up * 1000;

            m_GatePanel.SetActive(false);
        }
        /*else if (!isShop && NpcName == "Station Master")
        {
            m_StationMenu.anchoredPosition = Vector3.up * 1000;

            m_StationPanel.SetActive(false);
        }*/
    }

    public void SmithyShopFalse(string Npc)
    {
        SmithySystem(false, Npc);
        AllGameManager.Instance.isWeaponShop = false;
    }

    public IEnumerator DeathScreen()
    {
        //m_BloodPlaying = true;
        Color BScolor = m_DeathScreen.color;

        float m_Time = 0f;

        BScolor.a = Mathf.Lerp(0, 0.8f, m_Time);

        while (BScolor.a < 0.8f)
        {
            m_Time += Time.deltaTime / 2;

            BScolor.a = Mathf.Lerp(0, 0.8f, m_Time);

            m_DeathScreen.color = BScolor;

            yield return null;
        }

        AllGameManager.Instance.isGameOver = true;
        StartCoroutine(GameOver());
        //m_BloodPlaying = false;
    }

    IEnumerator GameOver()
    {
        m_DeathScreen.raycastTarget = true;

        Color GameOvercolor = m_GameOverScreen.color;

        float m_Time = 0f;

        while (GameOvercolor.a < 1)
        {
            m_Time += Time.deltaTime / 1;

            GameOvercolor.a = Mathf.Lerp(0, 1f, m_Time);

            m_GameOverScreen.color = GameOvercolor;

            yield return null;
        }

        m_GameOverButton[0].SetActive(true);
        m_GameOverButton[1].SetActive(true);
    }
}
