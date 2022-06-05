using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private ChoheeController m_Player;
    private NearItemCheck m_Item;

    public Text m_NearNameText;
    public Image m_PlayerHealth;
    public Image m_PlayerMask;
    public Image[] m_PlayerSkill;
    public Text m_SoulCount;

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

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();
        m_Item = GameObject.FindWithTag("Player").GetComponent<NearItemCheck>();

        m_LifeImage[0] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_4");
        m_LifeImage[1] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_3");
        m_LifeImage[2] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_2");
        m_LifeImage[3] = Resources.Load<Sprite>("7Textures/06_05UITexture/Life_1");

        m_CurMask[0] = Resources.Load<Sprite>("7Textures/06_05UITexture/MaskFrame");
        m_CurMask[1] = Resources.Load<Sprite>("7Textures/06_05UITexture/NormalMask");
        m_CurMask[2] = Resources.Load<Sprite>("7Textures/06_05UITexture/SpeedMask");
        m_CurMask[3] = Resources.Load<Sprite>("7Textures/06_05UITexture/FireMask");
        m_CurMask[4] = Resources.Load<Sprite>("7Textures/06_05UITexture/IceMask");

        m_SkillGage[0] = Resources.Load<Sprite>("7Textures/06_05UITexture/Skill_False");
        m_SkillGage[1] = Resources.Load<Sprite>("7Textures/06_05UITexture/Skill_Ready");
        m_SkillGage[2] = Resources.Load<Sprite>("7Textures/06_05UITexture/Skill_True");
    }

    void Update()
    {
        ItemInfo();
        HealthCtrl();
        MaskCtrl();
        SkillGageCtrl();
        SoulCount();
    }

    void ItemInfo()
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
}
