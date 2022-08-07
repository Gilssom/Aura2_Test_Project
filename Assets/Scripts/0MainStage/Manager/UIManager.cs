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

    public Text m_NearNameText;
    public Image m_PlayerHealth;
    public Image m_PlayerMask;
    public Image[] m_PlayerSkill;
    public Text m_SoulCount;
    public Image m_BloodScreen;
    public Image m_DeathScreen;
    public bool m_BloodPlaying;
    public GameObject m_PausePanel;
    public GameObject m_ExitPanel;
    public GameObject m_WeaponPanel;
    public GameObject m_ArmorPanel;
    public RectTransform m_WeaponMenu;
    public RectTransform m_ArmorMenu;

    public GameObject[] m_PauseMenus;
    public GameObject[] m_PauseIcon;
    public GameObject[] m_KeyHelpersMenus;
    public GameObject[] m_SettingText;
    public Image[] m_BGM;
    public Image[] m_SFX;

    int m_SettingMenuNumber;
    int m_SFXGage;
    int m_BGMGage;
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
    // ΩÃ±€≈Ê
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

        m_SettingGageOn = Resources.Load<Sprite>("7Textures/UI/Settings/GageOn");
        m_SettingGageOff = Resources.Load<Sprite>("7Textures/UI/Settings/GageOff");

        m_SFXGage = 4;
        m_BGMGage = 4;
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
    }

    void NearInfo()
    {
        if (m_Item.m_NearItem)
        {
            if (m_Item.m_NearItem.name == "FireMask_Item")
                m_NearNameText.text = "∏Ò¡ﬂ ≈ª »πµÊ«œ±‚" + "<color=#FFE400>" + " (E)" + "</color>";
            else if (m_Item.m_NearItem.name == "IceMask_Item")
                m_NearNameText.text = "∫Ò∫Ò ≈ª »πµÊ«œ±‚" + "<color=#FFE400>" + " (E)" + "</color>";
            else if (m_Item.m_NearItem.name == "SpeedMask_Item")
                m_NearNameText.text = "√ ∑©¿Ã ≈ª »πµÊ«œ±‚" + "<color=#FFE400>" + " (E)" + "</color>";
            else if (m_Item.m_NearItem.name == "NormalMask_Item")
                m_NearNameText.text = "æÁπ› ≈ª »πµÊ«œ±‚" + "<color=#FFE400>" + " (E)" + "</color>";
        }
        else if (m_Npc.m_NearNpc)
        {
            m_NearNameText.text = m_Npc.m_NearNpc.name + "øÕ ¥Î»≠«œ±‚" + "<color=#FFE400>" + " (E)" + "</color>";
        }
        else
            m_NearNameText.text = null;
    }

    void HealthCtrl()
    {
        int CurHealth = (int)PlayerStats.Instance.Health;
        if(PlayerStats.Instance.Health < 4)
            m_PlayerHealth.sprite = m_LifeImage[CurHealth];
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
        GameManager.Instance.isPause = true;
        
    }

    void PauseFalse()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isPause)
        {
            Time.timeScale = 1f;
            m_PausePanel.SetActive(false);
            m_ExitPanel.SetActive(false);
            GameManager.Instance.isPause = false;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isPause)
        {
            Time.timeScale = 0f;
            m_ExitPanel.SetActive(true);
            GameManager.Instance.isPause = true;
        }
    }

    public void PauseChagneMenu(int MenuNumber)
    {
        SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);

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
            SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);
            SettingTextCtrl(m_SettingMenuNumber);
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow) && m_SettingMenuNumber > 0 && m_SettingMenuNumber < 3)
        {
            m_SettingMenuNumber--;
            SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);
            SettingTextCtrl(m_SettingMenuNumber);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (m_SettingMenuNumber == 1 && m_SFXGage > -1 && m_SFXGage < 5)
            {
                m_SFXGage++;
                SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);
                SettingGageCtrl(m_SFXGage);
            }
            else if (m_SettingMenuNumber == 2 && m_SFXGage > -1 && m_SFXGage < 5)
            {
                m_BGMGage++;
                SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);
                SettingGageCtrl(m_BGMGage);
            }
            else
                return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (m_SettingMenuNumber == 1 && m_SFXGage > 0 && m_SFXGage < 6)
            {
                m_SFXGage--;
                SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);
                SettingGageCtrl(m_SFXGage);
            }
            else if (m_SettingMenuNumber == 2 && m_BGMGage > 0 && m_BGMGage < 6)
            {
                m_BGMGage--;
                SoundManager.Instance.SFXPlay("Button", GameManager.Instance.m_Clip[2]);
                SettingGageCtrl(m_BGMGage);
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
        if (m_SFXGage == 0)
            SoundManager.Instance.SFXSoundVolume(-80);
        else if (m_SFXGage == 1)
            SoundManager.Instance.SFXSoundVolume(-20);
        else if (m_SFXGage == 2)
            SoundManager.Instance.SFXSoundVolume(-10);
        else if (m_SFXGage == 3)
            SoundManager.Instance.SFXSoundVolume(-5);
        else if (m_SFXGage == 4)
            SoundManager.Instance.SFXSoundVolume(0);
        else if (m_SFXGage == 5)
            SoundManager.Instance.SFXSoundVolume(10);
    }

    void BGMSoundCheck()
    {
        if (m_BGMGage == 0)
            SoundManager.Instance.BGSoundVolume(-80);
        else if (m_BGMGage == 1)
            SoundManager.Instance.BGSoundVolume(-20);
        else if (m_BGMGage == 2)
            SoundManager.Instance.BGSoundVolume(-10);
        else if (m_BGMGage == 3)
            SoundManager.Instance.BGSoundVolume(-5);
        else if (m_BGMGage == 4)
            SoundManager.Instance.BGSoundVolume(0);
        else if (m_BGMGage == 5)
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
            GameManager.Instance.isPause = false;
        }
    }

    public void SmithySystem(bool isShop, string NpcName)
    {
        if(isShop && NpcName == "WeaponNpc")
        {
            m_WeaponPanel.SetActive(true);

            m_WeaponMenu.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }
        else if (isShop && NpcName == "ArmorNpc")
        {
            m_ArmorPanel.SetActive(true);

            m_ArmorMenu.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }
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

        //m_BloodPlaying = false;
    }
}
