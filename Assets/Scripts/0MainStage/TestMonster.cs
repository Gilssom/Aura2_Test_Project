using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TestMonster : MonoBehaviour
{
    public enum EnemyType { HellGhost, FireMonster }
    public EnemyType m_EnemyType;

    public enum CurState { idle, trace, attack, death}
    public CurState m_CurState = CurState.idle;

    public bool isSpawnMonster;

    private ChoheeController m_Player;
    private NavMeshAgent m_NavAgent;
    private Rigidbody m_Rigid;
    private Animator m_Anim;
    private SkinnedMeshRenderer m_SkinmeshRenderer;
    private BoxCollider m_AttackArea;
    public AudioClip[] m_clip;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    private float Cutoff = default;
    private float Speed = 0.0025f;
    [SerializeField]
    float m_TraceDis = default;
    float m_AttackDis = default;
    float m_rotSpeed = default;
    float m_moveSpeed = default;

    bool isDeath;

    private float m_MaxHP;
    private float m_CurHP;

    public GameObject m_HitEffect;
    public GameObject m_FireEffect;
    private GameObject m_DropSoul;
    private GameObject m_DropHeal;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();
        m_SkinmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_AttackArea = GetComponentInChildren<BoxCollider>();

        m_DropSoul = Resources.Load<GameObject>("5Item/PurpleItem_Soul");
        m_DropHeal = Resources.Load<GameObject>("5Item/RedItem_HP");
    }

    private void Start()
    {
        switch (m_EnemyType)
        {
            case EnemyType.HellGhost:
                m_MaxHP = 600;
                break;
            case EnemyType.FireMonster:
                if (isSpawnMonster)
                    m_TraceDis = 100;
                else                
                    m_TraceDis = 7;

                m_AttackDis = 2;
                m_rotSpeed = 3;
                m_moveSpeed = 3;
                m_MaxHP = 200;
                break;
        }
        m_CurHP = m_MaxHP;
        m_AttackArea.enabled = false;

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            WeaponHitEff();
            this.transform.DOMove(transform.position + transform.forward * -0.5f, 0.2f);
            m_CurHP -= ChoheeWeapon.Instance.m_CurDmg;

            if (PlayerStats.Instance.Slash >= 3)
            {
                return;
            }
            if (PlayerStats.Instance.Slash < 3)
            {
                PlayerStats.Instance.AddSlashGage(1);
            }
        }
        else if (other.tag == "ChargeWeapon")
        {
            WeaponHitEff();
            this.transform.DOMove(transform.position + transform.forward * -0.5f, 0.2f);
            m_CurHP -= ChoheeWeapon.Instance.m_ChargeDmg;

            if (PlayerStats.Instance.Slash >= 3)
            {
                return;
            }
            if (PlayerStats.Instance.Slash < 3)
            {
                PlayerStats.Instance.AddSlashGage(1);
            }
        }
        else if (other.tag == "SlashWeapon")
        {
            WeaponHitEff();
            this.transform.DOMove(transform.position + transform.forward * -0.5f, 0.2f);
            m_CurHP -= ChoheeWeapon.Instance.m_SlashDmg;
        }
    }

    void Update()
    {
        if (m_CurHP < 0)
        {
            m_Anim.SetBool("isAttack", false);
            m_Anim.SetTrigger("Death");
            Death();
        }
 
        if (m_moveSpeed < 3)
            m_moveSpeed += Time.deltaTime;
    }

    void LookPlayer()
    {
        Vector3 dir = m_Player.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * m_rotSpeed);
    }

    IEnumerator CheckState()
    {
        while(!isDeath)
        {
            yield return new WaitForSeconds(0.3f);

            float Dis = Vector3.Distance(m_Player.transform.position, this.transform.position);
            if (Dis <= m_AttackDis)
                m_CurState = CurState.attack;
            else if (Dis <= m_TraceDis)
                m_CurState = CurState.trace;
            else
                m_CurState = CurState.idle;
        }
    }

    IEnumerator CheckStateForAction()
    {
        while(!isDeath)
        {
            switch (m_CurState)
            {
                case CurState.idle:
                    break;
                case CurState.trace:
                    m_NavAgent.speed = m_moveSpeed;
                    m_Rigid.isKinematic = true;
                    m_NavAgent.destination = m_Player.transform.position;
                    m_NavAgent.Resume();
                    LookPlayer();
                    m_Anim.SetBool("isAttack", false);
                    break;
                case CurState.attack:
                    m_NavAgent.speed = 0;
                    m_Rigid.isKinematic = false;
                    LookPlayer();
                    m_Anim.SetBool("isAttack", true);
                    break;
                case CurState.death:
                    break;
                default:
                    break;
            }

            yield return null;
        }
    }

    void Attack()
    {
        m_AttackArea.enabled = true;
    }
    
    void AttackEnd()
    {
        m_AttackArea.enabled = false;
    }

    void WeaponHitEff()
    {
        Vector3 Pos = this.transform.position;
        GameObject Effect = Instantiate(m_HitEffect, new Vector3(Pos.x, Pos.y + 1f, Pos.z), this.transform.rotation);
        Effect.transform.SetParent(null, false);
        Destroy(Effect, 1);
    }

    void Death()
    {
        Vector3 Pos = this.transform.position;

        isDeath = true;
        StopAllCoroutines();
        m_NavAgent.enabled = false;

        switch (m_EnemyType)
        {
            case EnemyType.HellGhost:
                break;
            case EnemyType.FireMonster:
                if(isDeath)
                {
                    SphereCollider coll = GetComponent<SphereCollider>();

                    coll.enabled = false;

                    m_FireEffect.SetActive(false);

                    if (Cutoff >= MaxCutoff)
                    {
                        Destroy(this.gameObject);
                        GameManager.Instance.m_KillCount += 1;

                        int MaxItems = Random.Range(1, 3);
                        for (int i = 0; i < MaxItems; i++)
                        {
                            float RandomTF = Random.Range(0.5f, 3);
                            Instantiate(m_DropSoul, new Vector3(Pos.x + RandomTF, Pos.y + 0.5f, Pos.z + RandomTF), Quaternion.identity);
                        }

                        int HealthDrop = Random.Range(0, 100);
                        Debug.Log(HealthDrop);
                        if(HealthDrop <= 10 && PlayerStats.Instance.Health < PlayerStats.Instance.MaxHealth)
                            Instantiate(m_DropHeal, new Vector3(Pos.x, Pos.y + 0.5f, Pos.z), Quaternion.identity);
                    }

                    Cutoff += Speed;
                    if (Cutoff != MaxCutoff)
                    {
                        m_SkinmeshRenderer.material.SetFloat("_Dissolve", Cutoff);
                    }
                }
                break;
            default:
                break;
        }
    }
}
