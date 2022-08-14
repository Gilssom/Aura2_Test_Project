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
    public Text m_EnforceUIDamageText;
    public Text m_PauseChargeText;
    public Text m_PauseChargeText_2;
    public Text m_EnforceUIChargeText;

    public Text m_PauseCollTimeText;
    public Text m_EnforceUICollTimeText;
    public Text m_PauseSpeedText;
    public Text m_EnforceUISpeedText;

    /// <summary>
    /// 0 = 예리하게 강화
    /// 1 = 재빠르게 강화
    /// 2 = 커다랗게 강화
    /// 3 = 더 깃털 같은 옷 강화
    /// 4 = 더 깃털 같은 신발 강화
    /// </summary>
    public Text[] m_SoulText;

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();

        if (isWeapon)
        {
            m_PauseDamageText.text = "100%";
            m_PauseChargeText.text = "100%";
            m_EnforceUIDamageText.text = "현재 공격력 (0단계) = 100%";
            m_EnforceUIChargeText.text = "충전시간 감소 (0단계) = 100%";
        }
        else if(!isWeapon)
        {
            m_PauseCollTimeText.text = "100%";
            m_PauseSpeedText.text = "100%";
            m_EnforceUICollTimeText.text = "현재 회피기 재사용 시간 (0단계) = 100%";
            m_EnforceUISpeedText.text = "현재 이동속도 (0단계) = 100%";
        }
    }

    void Update()
    {
        if(isWeapon)
            m_PauseChargeText_2.text = m_PauseChargeText.text;
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
                m_EnforceUIDamageText.text = "현재 공격력 (1단계) = 120%";
            }
            else if (m_DamageStep == 2)
            {
                ChoheeWeapon.Instance.m_NorDmg = 150;
                m_PauseDamageText.text = "150%";
                m_EnforceUIDamageText.text = "현재 공격력 (2단계) = 150%";
            }
            else if (m_DamageStep == 3)
            {
                ChoheeWeapon.Instance.m_NorDmg = 200;
                m_PauseDamageText.text = "200%";
                m_EnforceUIDamageText.text = "현재 공격력 (3단계) = 200% (최대)";
                m_SoulText[0].gameObject.SetActive(false);
            }
            ChoheeWeapon.Instance.WeaponDamageUpdate();
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
                m_EnforceUIChargeText.text = "충전시간 감소 (1단계) = 85%";
            }
            else if (m_ChargeStep == 2)
            {
                m_Anim.SetFloat("ChargeSpeed", 1.4f);
                m_PauseChargeText.text = "60%";
                m_EnforceUIChargeText.text = "충전시간 감소 (2단계) = 60%";
            }
            else if (m_ChargeStep == 3)
            {
                m_Anim.SetFloat("ChargeSpeed", 1.55f);
                m_PauseChargeText.text = "45%";
                m_EnforceUIChargeText.text = "충전시간 감소 (3단계) = 45% (최대)";
                m_SoulText[1].gameObject.SetActive(false);
            }
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
                m_EnforceUICollTimeText.text = "회피기 재사용 시간 감소 (1단계) = 85%";
            }
            else if (m_DressStep == 2)
            {
                m_PauseCollTimeText.text = "60%";
                m_EnforceUICollTimeText.text = "회피기 재사용 시간 감소 (2단계) = 60%";
            }
            else if (m_DressStep == 3)
            {
                m_PauseCollTimeText.text = "45%";
                m_EnforceUICollTimeText.text = "회피기 재사용 시간 감소 (3단계) = 45% (최대)";
                m_SoulText[3].gameObject.SetActive(false);
            }
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
                m_EnforceUISpeedText.text = "이동속도 증가 (1단계) = 110%";
            }
            else if (m_BootsStep == 2)
            {
                m_Player.Speed = 6;
                m_PauseSpeedText.text = "120%";
                m_EnforceUISpeedText.text = "이동속도 증가 (2단계) = 120%";
            }
            else if (m_BootsStep == 3)
            {
                m_Player.Speed = 6.5f;
                m_PauseSpeedText.text = "130%";
                m_EnforceUISpeedText.text = "이동속도 증가 (3단계) = 130% (최대)";
                m_SoulText[4].gameObject.SetActive(false);
            }
        }
        else
            Debug.Log("강화 할 수 없습니다.");
    }
}
