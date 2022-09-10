using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathCreation;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private BossFollowPath m_PathSystem;

    [SerializeField]
    private int m_MaxHealth;
    [SerializeField]
    private int m_CurHealth;

    private Rigidbody m_Rigid;
    private Animator m_Anim;
    private CapsuleCollider m_Coll;
    private SkinnedMeshRenderer m_SkinmeshRenderer;

    /// <summary>
    /// 0   ::   FrontAttack    보스 기준 앞 공격
    /// 1   ::   BackAttack     보스 기준 뒤 공격
    /// 2   ::   RightAttack    보스 기준 오른쪽 공격
    /// 3   ::   LeftAttack     보스 기준 왼쪽 공격
    /// </summary>
    public BoxCollider[] m_AttackArea;
    public CapsuleCollider m_TornadoArea;

    [SerializeField]
    private float m_PhaseNumber;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    private float Cutoff = default;
    private float Speed = 0.005f;

    private bool isDeath = false;

    private GameObject m_DropSoul;
    private GameObject m_DropHeal;

    public GameObject m_HitEffect;
    public GameObject m_BossUI;
    public Slider m_HpBar;
    public Text m_HpPercent;

    public GameObject m_BombObject;

    [SerializeField]
    private int m_TurnAttackNumber;

    /// <summary>
    /// 0 :: Normal Attack Sound
    /// 1 :: Moving Attack Sound
    /// 2 :: Voice Sound
    /// 3 :: Short Normal Attack Sound 
    /// 4 :: Short Voice Sound
    /// </summary>
    public AudioClip[] m_SoundEffect;

    void Awake()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();
        m_Coll = GetComponent<CapsuleCollider>();
        m_PathSystem = GetComponent<BossFollowPath>();
        m_SkinmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_DropSoul = Resources.Load<GameObject>("5Item/PurpleItem_Soul");
        m_DropHeal = Resources.Load<GameObject>("5Item/RedItem_HP");

        m_MaxHealth = 12000;
        m_HpBar.maxValue = m_MaxHealth;
        m_HpBar.minValue = 9100;
        m_PhaseNumber = 1;
        m_TurnAttackNumber = 0;
    }

    void Start()
    {
        m_BossUI.SetActive(true);
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
        HealthCtrl();

        // 공격 테스트용 :: StartCoroutine(BossThink()) 비활성화
        //TestAttack();
    }

    void HealthCtrl()
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

        m_HpBar.value = Mathf.Lerp(m_HpBar.value, m_CurHealth, Time.deltaTime * 3f);

        float HpPercent = m_CurHealth / 120;
        m_HpPercent.text = HpPercent.ToString("F0") + "%";
    }

    void TestAttack()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            SoundManager.Instance.BgSoundPlay(SoundManager.Instance.m_BgList[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BossNormalAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BossRotateAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BossMovingAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(BossLongDisAttack());
        }
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

    public IEnumerator BossThink()
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
                    BossMovingAttack();
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
                    StartCoroutine(BossLongDisAttack());
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

    void BossMovingAttack()
    {
        m_Anim.SetBool("Right Tornado Attack", true);
        m_TornadoArea.enabled = true;
        m_PathSystem.enabled = true;
    }

    public void AttackSound(int AttackNumber)
    {
        SoundManager.Instance.SFXPlay("Attack", m_SoundEffect[AttackNumber]);
    }

    public void BossMovingAttackFalse()
    {
        m_Anim.SetBool("Right Tornado Attack", false);
        m_TornadoArea.enabled = false;
        StartCoroutine(BossThink());
    }

    void BossRotateAttack()
    {
        switch (m_TurnAttackNumber)
        {
            case 0:
                m_Anim.SetTrigger("Turn_1 Attack");
                m_TurnAttackNumber++;
                break;
            case 1:
                m_Anim.SetTrigger("Turn_2 Attack");
                m_TurnAttackNumber++;
                break;
            case 2:
                m_Anim.SetTrigger("Turn_3 Attack");
                m_TurnAttackNumber = 1;
                break;
        }
    }

    IEnumerator BossLongDisAttack()
    {
        SoundManager.Instance.SFXPlay("Attack", m_SoundEffect[2]);
        m_Anim.SetBool("Long Attack", true);
        Vector3 Pos = this.transform.position;
        int AttackNumbers = Random.Range(10, 15);

        for (int i = 0; i < AttackNumbers; i++)
        {
            float RandomTFX = Random.Range(-10f, 10f);
            float RandomTFZ = Random.Range(-10f, 10f);
            Instantiate(m_BombObject, new Vector3(Pos.x + RandomTFX, Pos.y + 20, Pos.z + RandomTFZ), Quaternion.identity);           
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1);
        m_Anim.SetBool("Long Attack", false);
        StartCoroutine(BossThink());
    }

    public void Attack(int AttackNumber)
    {
        if(AttackNumber != 5)
            SoundManager.Instance.SFXPlay("Attack", m_SoundEffect[3]);

        if(AttackNumber < 4)
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
        if(AttackNumber < 4)
            m_AttackArea[AttackNumber].enabled = false;
        else
        {
            m_AttackArea[0].enabled = false;
            m_AttackArea[1].enabled = false;
            m_AttackArea[2].enabled = false;
            m_AttackArea[3].enabled = false;
        }
    }

    public void AttackEnd()
    {
        StartCoroutine(BossThink());
    }

    void Death()
    {
        if (!isDeath)
            SoundManager.Instance.SFXPlay("Death", m_SoundEffect[6]);
        //Destroy(m_HpBar.gameObject);
        isDeath = true;
        m_HpBar.gameObject.SetActive(false);
        CancelInvoke();
        StopAllCoroutines();
        m_Anim.Play("Death");

        Vector3 Pos = this.transform.position;

        CapsuleCollider Bosscoll = GetComponent<CapsuleCollider>();

        Bosscoll.enabled = false;
        m_Rigid.isKinematic = true;

        if (Cutoff >= MaxCutoff)
        {
            GameManager.Instance.m_FirstBossEnd = true;
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
