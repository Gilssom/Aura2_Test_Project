using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnforceManager : MonoBehaviour
{
    private ChoheeController m_Player;

    [SerializeField]
    float m_DamageStep = 0;
    [SerializeField]
    float m_ChargeStep = 0;
    [SerializeField]
    float m_DressStep = 0;
    [SerializeField]
    float m_BootsStep = 0;

    public bool isWeapon;

    public Animator m_Anim;
    public Text m_PauseDamageText;
    public Text m_PauseChargeText;
    public Text m_PauseChargeText_2;

    public Text m_PauseCollTimeText;
    public Text m_PauseSpeedText;

    public Image m_DamageUpGradeImage;
    public Image m_SpeedUpGradeImage;
    public Image m_DistanceUpGradeImage;
    public Image m_DressUpGradeImage;
    public Image m_BootsUpGradeImage;

    /// <summary>
    /// 0 = 예리하게 강화
    /// 1 = 재빠르게 강화
    /// 2 = 커다랗게 강화
    /// 3 = 더 깃털 같은 옷 강화
    /// 4 = 더 깃털 같은 신발 강화
    /// </summary>
    public Text[] m_SoulText;

    /// <summary>
    /// 0 , 1 = 날카롭게
    /// 2 , 3 = 재빠르게
    /// 4 , 5 = 커다랗게
    /// </summary>
    public Sprite[] m_UpGradeImage;
    public GameObject[] m_UpGradeButton;

    public Text m_SoulCount;

    private void Awake()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();
    }

    void Start()
    {
        if (isWeapon)
        {
            m_PauseDamageText.text = "100%";
            m_PauseChargeText.text = "100%";
        }
        else if(!isWeapon)
        {
            m_PauseCollTimeText.text = "100%";
            m_PauseSpeedText.text = "100%";
        }
    }

    void Update()
    {
        if(isWeapon)
            m_PauseChargeText_2.text = m_PauseChargeText.text;

        m_SoulCount.text = UIManager.Instance.m_SoulCount.text;

        if(!m_Anim)
            m_Anim = m_Player.GetComponent<Animator>();
    }

    public void DamageEnforce()
    {
        float Cost = 100 + 100 * m_DamageStep;

        if (m_DamageStep < 3 && PlayerStats.Instance.Soul >= Cost)
        {
            StartCoroutine(m_Player.EnforceEff(0));
            m_DamageStep++;
            PlayerStats.Instance.UseSoul(Cost);
            m_SoulText[0].text = "" + (100 + 100 * m_DamageStep) + "";

            if (m_DamageStep == 1)
            {
                ChoheeWeapon.Instance.m_NorDmg = 120;
                m_PauseDamageText.text = "120%";
                m_DamageUpGradeImage.sprite = m_UpGradeImage[0];
            }
            else if (m_DamageStep == 2)
            {
                ChoheeWeapon.Instance.m_NorDmg = 150;
                m_PauseDamageText.text = "150%";
                m_DamageUpGradeImage.sprite = m_UpGradeImage[1];
            }
            else if (m_DamageStep == 3)
            {
                ChoheeWeapon.Instance.m_NorDmg = 200;
                m_PauseDamageText.text = "200%";
                m_SoulText[0].gameObject.SetActive(false);
                m_UpGradeButton[0].SetActive(false);
                m_DamageUpGradeImage.sprite = m_UpGradeImage[2];
            }
            ChoheeWeapon.Instance.WeaponDamageUpdate();
            SoundManager.Instance.SFXPlay("WeaponUpSound", AllGameManager.Instance.m_Clip[10]);
        }
        else
            Debug.Log("강화 할 수 없습니다.");
    }

    public void ChargeEnforce()
    {
        float Cost = 100 + 100 * m_ChargeStep;

        if (m_ChargeStep < 3 && PlayerStats.Instance.Soul >= Cost)
        {
            StartCoroutine(m_Player.EnforceEff(0));
            m_ChargeStep++;
            PlayerStats.Instance.UseSoul(Cost);
            m_SoulText[1].text = "" + (100 + 100 * m_ChargeStep) + "";

            if (m_ChargeStep == 1)
            {
                m_Anim.SetFloat("ChargeSpeed", 1.15f);
                m_PauseChargeText.text = "85%";
                m_SpeedUpGradeImage.sprite = m_UpGradeImage[3];
            }
            else if (m_ChargeStep == 2)
            {
                m_Anim.SetFloat("ChargeSpeed", 1.4f);
                m_PauseChargeText.text = "60%";
                m_SpeedUpGradeImage.sprite = m_UpGradeImage[4];
            }
            else if (m_ChargeStep == 3)
            {
                m_Anim.SetFloat("ChargeSpeed", 1.55f);
                m_PauseChargeText.text = "45%";
                m_SoulText[1].gameObject.SetActive(false);
                m_UpGradeButton[1].SetActive(false);
                m_SpeedUpGradeImage.sprite = m_UpGradeImage[5];
            }
            SoundManager.Instance.SFXPlay("WeaponUpSound", AllGameManager.Instance.m_Clip[10]);
        }
        else
            Debug.Log("강화 할 수 없습니다.");
    }

    public void DressEnforce()
    {
        float Cost = 100 + 100 * m_DressStep;

        if (m_DressStep < 3 && PlayerStats.Instance.Soul >= Cost)
        {
            StartCoroutine(m_Player.EnforceEff(1));
            m_DressStep++;
            PlayerStats.Instance.UseSoul(Cost);
            m_SoulText[3].text = "" + (100 + 100 * m_DressStep) + "";

            if (m_DressStep == 1)
            {
                m_PauseCollTimeText.text = "85%";
                m_DressUpGradeImage.sprite = m_UpGradeImage[6];
            }
            else if (m_DressStep == 2)
            {
                m_PauseCollTimeText.text = "60%";
                m_DressUpGradeImage.sprite = m_UpGradeImage[7];
            }
            else if (m_DressStep == 3)
            {
                m_PauseCollTimeText.text = "45%";
                m_SoulText[3].gameObject.SetActive(false);
                m_UpGradeButton[2].SetActive(false);
                m_DressUpGradeImage.sprite = m_UpGradeImage[8];
            }
            SoundManager.Instance.SFXPlay("WeaponUpSound", AllGameManager.Instance.m_Clip[11]);
        }
        else
            Debug.Log("강화 할 수 없습니다.");
    }

    public void BootsEnforce()
    {
        float Cost = 100 + 100 * m_BootsStep;

        if (m_BootsStep < 3 && PlayerStats.Instance.Soul >= Cost)
        {
            StartCoroutine(m_Player.EnforceEff(1));
            m_BootsStep++;
            PlayerStats.Instance.UseSoul(Cost);
            m_SoulText[4].text = "" + (100 + 100 * m_BootsStep) + "";

            if (m_BootsStep == 1)
            {
                m_Player.Speed = 5.5f;
                m_PauseSpeedText.text = "110%";
                m_BootsUpGradeImage.sprite = m_UpGradeImage[9];
            }
            else if (m_BootsStep == 2)
            {
                m_Player.Speed = 6;
                m_PauseSpeedText.text = "120%";
                m_BootsUpGradeImage.sprite = m_UpGradeImage[10];
            }
            else if (m_BootsStep == 3)
            {
                m_Player.Speed = 6.5f;
                m_PauseSpeedText.text = "130%";
                m_SoulText[4].gameObject.SetActive(false);
                m_UpGradeButton[3].SetActive(false);
                m_BootsUpGradeImage.sprite = m_UpGradeImage[11];
            }
            SoundManager.Instance.SFXPlay("WeaponUpSound", AllGameManager.Instance.m_Clip[11]);
        }
        else
            Debug.Log("강화 할 수 없습니다.");
    }
}
