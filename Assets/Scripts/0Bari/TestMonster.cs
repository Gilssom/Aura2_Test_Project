using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMonster : MonoBehaviour
{
    public enum BossType { Maze, ETC }
    public BossType m_BossType;

    public enum CurState { idle, trace, attack, death}
    public CurState m_CurState = CurState.idle;

    private BariController m_Player;
    private NavMeshAgent m_NavAgent;
    private Rigidbody m_Rigid;
    private Animator m_Anim;
    public Material m_Dissolve;
    public AudioClip[] m_clip;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    public float Cutoff = default;
    public float Speed = default;

    float m_TraceDis = 7;
    float m_AttackDis = 2;
    float m_rotSpeed = 3;
    float m_moveSpeed = 3;

    bool isDeath;

    [SerializeField]
    private float m_MaxHP;
    private float m_CurHP;

    public GameObject m_HitEffect;

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindWithTag("Player").GetComponent<BariController>();
        m_Dissolve.SetFloat("_Dissolve", 0);
        switch (m_BossType)
        {
            case BossType.Maze:
                m_MaxHP = 100;
                break;
            case BossType.ETC:
                m_MaxHP = 40;
                break;
        }
        m_CurHP = m_MaxHP;

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WinterIce")
        {
            Debug.Log("Winter Dmg Check || Cur HP : " + m_CurHP);
            m_CurHP -= 20;
            m_moveSpeed = 1;
        }

        if(other.tag == "Missile")
        {
            Debug.Log("Falling Dmg Check || Cur HP : " + m_CurHP);
            m_CurHP -= 3;
        }

        if (other.tag == "SummerLaser")
        {
            Debug.Log("Summer Dmg Check || Cur HP : " + m_CurHP);
            m_CurHP -= 30;
        }

        if(other.tag == "Weapon")
        {
            if(m_Player.m_SkillStack < 3)
                m_Player.SkillAttack();
            SoundManager.Instance.SFXPlay("Enemy Hit", m_Player.m_clip[5]);
            Debug.Log("PlayerAttack Check || Cur HP: " + m_CurHP);
            WeaponHitEff();
            m_CurHP -= 5;
        }
    }

    void Update()
    {
        if (m_CurHP < 0)
        {
            m_Anim.SetBool("isTrace", false);
            m_Anim.SetBool("isAttack", false);
            m_Anim.SetTrigger("Death");
            if (!isDeath)
                SoundManager.Instance.SFXPlay("Skeleton Death", m_clip[0]);
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
                    m_Anim.SetBool("isTrace", false);
                    break;
                case CurState.trace:
                    m_NavAgent.speed = m_moveSpeed;
                    m_Rigid.isKinematic = true;
                    m_NavAgent.destination = m_Player.transform.position;
                    m_NavAgent.Resume();
                    LookPlayer();
                    m_Anim.SetBool("isAttack", false);
                    m_Anim.SetBool("isTrace", true);
                    break;
                case CurState.attack:
                    m_NavAgent.speed = 0;
                    m_Rigid.isKinematic = false;
                    LookPlayer();
                    m_Anim.SetBool("isTrace", false);
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

    void WeaponHitEff()
    {
        GameObject Effect = Instantiate(m_HitEffect, this.transform.position, this.transform.rotation);
        Effect.transform.SetParent(this.transform);
        Destroy(Effect, 1);
    }

    void Death()
    {
        isDeath = true;
        Debug.Log(gameObject.name + " Death");
        StopAllCoroutines();
        m_NavAgent.enabled = false;

        switch (m_BossType)
        {
            case BossType.Maze:
                Vector3 CurPos = transform.position;
                Quaternion CurRot = Quaternion.Euler(0, 0, 0);
                Instantiate(Resources.Load("2ModelingResource/SkillCryStal"),
                    new Vector3(CurPos.x, 0, CurPos.z), CurRot);
                Destroy(gameObject, 2);
                break;
            case BossType.ETC:
                if(isDeath)
                {
                    if (Cutoff >= MaxCutoff)
                        Destroy(gameObject);

                    Cutoff += Speed;

                    if(Cutoff != MaxCutoff)
                    {
                        m_Dissolve.SetFloat("_Dissolve", Cutoff);
                        return;
                    }
                }
                break;
            default:
                break;
        }
    }
}
