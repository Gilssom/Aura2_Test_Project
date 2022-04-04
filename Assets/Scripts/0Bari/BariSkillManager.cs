using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BariSkillManager : MonoBehaviour
{
    private BariController m_Player;

    public GameObject m_WinterIcePrefab;

    private static BariSkillManager m_instance;
    // ΩÃ±€≈Ê
    public static BariSkillManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(BariSkillManager)) as BariSkillManager;

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

        m_Player = GetComponent<BariController>();
    }

    public IEnumerator WinterIceStun()
    {
        Vector3 Pos = this.transform.position;
        GameObject IceBreak = Instantiate(m_WinterIcePrefab, this.transform.forward + 
            new Vector3(Pos.x, Pos.y + 0.1f, Pos.z), this.transform.rotation);
        m_Player.DoSkill = true;
        m_Player.m_SkillStack = m_Player.m_SkillMinStack;
        yield return new WaitForSeconds(0.8f);
        m_Player.DoSkill = false;
        Destroy(IceBreak, 2f);
        yield return null;
    }
}
