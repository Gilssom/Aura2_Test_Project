using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChoheeWeapon : MonoBehaviour
{
    private float m_NorDmg;
    public float m_CurDmg;
    private float m_FireDmg;
    public float m_ChargeDmg;
    public float m_SlashDmg;

    public Material[] material;
    public int m_Number;
    Renderer rend;

    public GameObject[] m_WeaponEff;
    /// <summary>
    /// 0 = Normal
    /// 1 = Speed
    /// 2 = Fire
    /// 3 = Ice
    /// </summary>
    public GameObject[] m_Mask;

    private static ChoheeWeapon m_instance;
    // �̱���
    public static ChoheeWeapon Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(ChoheeWeapon)) as ChoheeWeapon;

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
    }

    void Start()
    {
        m_NorDmg = 100;
        m_CurDmg = m_NorDmg;

        m_FireDmg = m_CurDmg * 1.5f;
        m_ChargeDmg = m_CurDmg * 2;
        m_SlashDmg = m_CurDmg;

        m_Number = 0;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[m_Number];
    }

    void Update()
    {
        rend.sharedMaterial = material[m_Number];

        //WeaponTypeChange();
    }

    public void WeaponTypeChange(string MaskName)
    {
        if (MaskName == "NormalMask_Item")
        {
            m_Number = 0;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[1].SetActive(false);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(true);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(false);
        }
        else if (MaskName == "SpeedMask_Item")
        {
            m_Number = 0;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[1].SetActive(false);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(true);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(false);
        }
        else if (MaskName == "FireMask_Item")
        {
            m_Number = 1;
            m_CurDmg = m_FireDmg;
            m_WeaponEff[0].SetActive(true);
            m_WeaponEff[1].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(true);
            m_Mask[3].SetActive(false);
        }
        else if(MaskName == "IceMask_Item")
        {
            m_Number = 2;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[1].SetActive(true);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(true);
        }
        else
        {
            m_Number = 0;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[1].SetActive(false);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(false);
        }
    }
}
