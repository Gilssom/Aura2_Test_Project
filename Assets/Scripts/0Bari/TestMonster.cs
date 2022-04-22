using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMonster : MonoBehaviour
{
    public enum CurState { idle, trace, attack, death}
    public CurState m_CurState = CurState.idle;

    private BariController m_Player;
    private NavMeshAgent m_NavAgent;
    private Rigidbody m_Rigid;

    float m_TraceDis = 7;
    float m_AttackDis = 2;
    float m_rotSpeed = 3;

    bool isDeath;

    private float m_MaxHP;
    private float m_CurHP;

    public GameObject m_HitEffect;

    private void Start()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindWithTag("Player").GetComponent<BariController>();
        m_MaxHP = 100;
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
                m_Player.m_SkillStack++;
            Debug.Log("PlayerAttack Check || Cur HP: " + m_CurHP);
            WeaponHitEff();
            m_CurHP -= 5;
        }
    }

    void Update()
    {
        if (m_CurHP < 0)
            Death();
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
                    m_NavAgent.speed = 3;
                    m_Rigid.isKinematic = true;
                    m_NavAgent.destination = m_Player.transform.position;
                    m_NavAgent.Resume();
                    LookPlayer();
                    break;
                case CurState.attack:
                    m_NavAgent.speed = 0;
                    m_Rigid.isKinematic = false;
                    LookPlayer();
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
        Debug.Log("Monster Death");
        StopAllCoroutines();
        m_NavAgent.enabled = false;
        isDeath = true;
        Destroy(gameObject, 2f);
    }
}
