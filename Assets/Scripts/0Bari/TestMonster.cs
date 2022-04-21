using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour
{
    private BariController m_Player;

    private float m_MaxHP;
    private float m_CurHP;

    private void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<BariController>();
        m_MaxHP = 100;
        m_CurHP = m_MaxHP;
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
            Debug.Log("PlayerAttack Check");
            m_CurHP -= 5;
        }
    }

    void Update()
    {
        if (m_CurHP < 0)
            Death();
    }

    void Death()
    {
        Debug.Log("Monster Death");
        Destroy(gameObject, 2f);
    }
}
