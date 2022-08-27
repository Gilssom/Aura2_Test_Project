using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int m_MaxHealth;
    [SerializeField]
    private int m_CurHealth;

    private Rigidbody m_Rigid;
    private Animator m_Anim;
    private CapsuleCollider m_Coll;
    private SkinnedMeshRenderer m_SkinmeshRenderer;

    public BoxCollider[] m_AttackArea;

    [SerializeField]
    private float m_PhaseNumber;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    private float Cutoff = default;
    private float Speed = 0.005f;

    private GameObject m_DropSoul;
    private GameObject m_DropHeal;

    public GameObject m_HitEffect;
    public Slider m_HpBar;
    public Text m_HpPercent;

    void Awake()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();
        m_Coll = GetComponent<CapsuleCollider>();
        m_SkinmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_DropSoul = Resources.Load<GameObject>("5Item/PurpleItem_Soul");
        m_DropHeal = Resources.Load<GameObject>("5Item/RedItem_HP");

        m_MaxHealth = 12000;
        m_HpBar.maxValue = m_MaxHealth;
        m_HpBar.minValue = 9100;
        m_PhaseNumber = 1;
    }

    void Start()
    {
        m_CurHealth = m_MaxHealth;

        Invoke("BossThinkStart", 2);
    }

    void BossThinkStart()
    {
        if (m_PhaseNumber == 1)
        {
            m_Anim.Play("Scream");
        }

        StartCoroutine(BossThink());
    }

    void Update()
    {
        if (m_CurHealth <= 9100 && m_CurHealth > 4200)
        {
            m_PhaseNumber = 2;
            m_HpBar.maxValue = 9100;
            m_HpBar.minValue = 4200;
        }
        else if (m_CurHealth <= 4200 && m_CurHealth > 0)
        {
            m_PhaseNumber = 3;
            m_HpBar.maxValue = 4200;
            m_HpBar.minValue = 0;
        }
        else if (m_CurHealth <= 0)
            Death();

        m_HpBar.value = m_CurHealth;

        float HpPercent = m_CurHealth / 120;
        m_HpPercent.text = HpPercent.ToString("F0") + "%";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            WeaponHitEff();
            m_CurHealth -= (int)ChoheeWeapon.Instance.m_CurDmg;

            if (PlayerStats.Instance.Slash >= 4)
            {
                return;
            }
            if (PlayerStats.Instance.Slash < 4)
            {
                PlayerStats.Instance.AddSlashGage(1);
            }
        }
        else if (other.tag == "ChargeWeapon")
        {
            WeaponHitEff();
            m_CurHealth -= (int)ChoheeWeapon.Instance.m_ChargeDmg;

            if (PlayerStats.Instance.Slash >= 4)
            {
                return;
            }
            if (PlayerStats.Instance.Slash < 4)
            {
                PlayerStats.Instance.AddSlashGage(1);
            }
        }
        else if (other.tag == "SlashWeapon")
        {
            WeaponHitEff();
            m_CurHealth -= (int)ChoheeWeapon.Instance.m_SlashDmg;
        }
    }

    void WeaponHitEff()
    {
        Vector3 Pos = this.transform.position;
        GameObject Effect = Instantiate(m_HitEffect, new Vector3(Pos.x, Pos.y + 1f, Pos.z), this.transform.rotation);
        Effect.transform.SetParent(null, false);
        Destroy(Effect, 1);
    }

    IEnumerator BossThink()
    {
        int OnePattenAttack = Random.Range(0, 2);
        int TwoPattenAttack = Random.Range(0, 3);

        yield return new WaitForSeconds(2);

        if(m_PhaseNumber == 1)
        {
            switch (OnePattenAttack)
            {
                case 0:
                    BossNormalAttack();
                    Debug.Log("페이즈 1 : 일반공격");
                    break;
                case 1:
                    BossRotateAttack();
                    Debug.Log("페이즈 1 : 회전공격");
                    break;
            }
        }

        else if (m_PhaseNumber == 2)
        {
            switch (TwoPattenAttack)
            {
                case 0:
                    BossNormalAttack();
                    Debug.Log("페이즈 2 : 일반공격");
                    break;
                case 1:
                    BossRotateAttack();
                    Debug.Log("페이즈 2 : 회전공격");
                    break;
                case 2:
                    StartCoroutine(BossThink());
                    Debug.Log("페이즈 2 : 이동공격");
                    break;
            }
        }

        else if (m_PhaseNumber == 3)
        {
            switch (TwoPattenAttack)
            {
                case 0:
                    BossNormalAttack();
                    Debug.Log("페이즈 3 : 일반공격");
                    break;
                case 1:
                    BossRotateAttack();
                    Debug.Log("페이즈 3 : 회전공격");
                    break;
                case 2:
                    StartCoroutine(BossThink());
                    Debug.Log("페이즈 3 : 폭탄발사");
                    break;
            }
        }
    }

    void BossNormalAttack()
    {
        int RandomAttack = Random.Range(0, 3);

        switch (RandomAttack)
        {
            case 0:
                m_Anim.SetTrigger("Front Attack");
                break;
            case 1:
                m_Anim.SetTrigger("Back Attack");
                break;
            case 2:
                m_Anim.SetTrigger("Right Attack");
                break;
            case 3:
                m_Anim.SetTrigger("Left Attack");
                break;
        }
    }

    void BossRotateAttack()
    {
        m_Anim.SetTrigger("Rotate Attack");
    }

    public void Attack(int AttackNumber)
    {
        if(AttackNumber != 4)
            m_AttackArea[AttackNumber].enabled = true;
        else
        {
            m_AttackArea[0].enabled = true;
            m_AttackArea[1].enabled = true;
            m_AttackArea[2].enabled = true;
            m_AttackArea[3].enabled = true;
        }
    }

    public void AttackFalse(int AttackNumber)
    {
        if(AttackNumber != 4)
            m_AttackArea[AttackNumber].enabled = false;
        else
        {
            m_AttackArea[0].enabled = false;
            m_AttackArea[1].enabled = false;
            m_AttackArea[2].enabled = false;
            m_AttackArea[3].enabled = false;
        }

        StartCoroutine(BossThink());
    }

    void Death()
    {
        Destroy(m_HpBar.gameObject);
        CancelInvoke();
        StopAllCoroutines();
        m_Anim.Play("Death");

        Vector3 Pos = this.transform.position;

        CapsuleCollider Bosscoll = GetComponent<CapsuleCollider>();

        Bosscoll.enabled = false;
        m_Rigid.isKinematic = true;

        if (Cutoff >= MaxCutoff)
        {
            Destroy(this.gameObject);
            GameManager.Instance.m_KillCount += 1;

            int MaxItems = Random.Range(20, 40);
            for (int i = 0; i < MaxItems; i++)
            {
                float RandomTFX = Random.Range(-8, 8);
                float RandomTFZ = Random.Range(-8, 8);
                Instantiate(m_DropSoul, new Vector3(Pos.x + RandomTFX, Pos.y + 0.5f, Pos.z + RandomTFZ), Quaternion.identity);
            }

            int HealthDrop = Random.Range(0, 100);
            if (HealthDrop <= 100 && PlayerStats.Instance.Health < PlayerStats.Instance.MaxHealth)
                Instantiate(m_DropHeal, new Vector3(Pos.x, Pos.y + 0.5f, Pos.z), Quaternion.identity);
        }

        Cutoff += Speed;
        if (Cutoff != MaxCutoff)
        {
            m_SkinmeshRenderer.material.SetFloat("_Dissolve", Cutoff);
        }
    }
}
