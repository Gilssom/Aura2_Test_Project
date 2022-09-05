using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChoheeWeapon : MonoBehaviour
{
    [SerializeField]
    private ChoheeController m_Player;

    public float m_NorDmg;
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
    [SerializeField]
    private string m_MaskType;
    public string MaskType { get { return m_MaskType; } }

    private static ChoheeWeapon m_instance;
    // ΩÃ±€≈Ê
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
        m_Player = GetComponentInParent<ChoheeController>();

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
        m_MaskType = "null";
        m_NorDmg = 100;
        WeaponDamageUpdate();

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

    public void WeaponDamageUpdate()
    {
        m_CurDmg = m_NorDmg;

        m_FireDmg = m_CurDmg * 1.5f;
        m_ChargeDmg = m_CurDmg * 2;
        m_SlashDmg = m_CurDmg;
    }

    public void WeaponTypeChange(string MaskName)
    {
        if (MaskName == "NormalMask_Item")
        {
            SoundManager.Instance.SFXPlay("Mask Equid", AllGameManager.Instance.m_Clip[3]);
            m_MaskType = "Normal";
            m_Player.Speed = 5;
            m_Player.m_WeaponCurType = 0;
            m_Number = 0;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[2].SetActive(false);
            m_WeaponEff[1].SetActive(false);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(true);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(false);
        }
        else if (MaskName == "SpeedMask_Item")
        {
            SoundManager.Instance.SFXPlay("Mask Equid", AllGameManager.Instance.m_Clip[3]);
            m_MaskType = "Speed";
            m_Player.Speed = 6;
            m_Player.m_WeaponCurType = 0;
            m_Number = 0;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[2].SetActive(true);
            m_WeaponEff[1].SetActive(false);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(true);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(false);
        }
        else if (MaskName == "FireMask_Item")
        {
            SoundManager.Instance.SFXPlay("Mask Equid", AllGameManager.Instance.m_Clip[3]);
            m_MaskType = "Fire";
            m_Player.Speed = 5;
            m_Player.m_WeaponCurType = 1;
            m_Number = 1;
            m_CurDmg = m_FireDmg;
            m_WeaponEff[2].SetActive(false);
            m_WeaponEff[0].SetActive(true);
            m_WeaponEff[1].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(true);
            m_Mask[3].SetActive(false);
        }
        else if(MaskName == "IceMask_Item")
        {
            SoundManager.Instance.SFXPlay("Mask Equid", AllGameManager.Instance.m_Clip[3]);
            m_MaskType = "Ice";
            m_Player.Speed = 5;
            m_Player.m_WeaponCurType = 2;
            m_Number = 2;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[2].SetActive(false);
            m_WeaponEff[1].SetActive(true);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(true);
        }
        else
        {
            m_MaskType = "Null";
            m_Player.Speed = 5;
            m_Player.m_WeaponCurType = 0;
            m_Number = 0;
            m_CurDmg = m_NorDmg;
            m_WeaponEff[2].SetActive(false);
            m_WeaponEff[1].SetActive(false);
            m_WeaponEff[0].SetActive(false);

            m_Mask[0].SetActive(false);
            m_Mask[1].SetActive(false);
            m_Mask[2].SetActive(false);
            m_Mask[3].SetActive(false);
        }
    }
}
