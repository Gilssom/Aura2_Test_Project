using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int m_KillCount;

    public GameObject[] m_CountObject;

    private static GameManager m_instance;
    // ΩÃ±€≈Ê
    public static GameManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(GameManager)) as GameManager;

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

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        CountObject();
    }

    void CountObject()
    {
        if (m_KillCount == 6)
            ObjectCtrl(0, false);
        // Third Field Spawn Controll
        else if (m_KillCount == 13)
            ObjectCtrl(3, true);
        else if (m_KillCount == 20)
            ObjectCtrl(5, true);
        // Fifth Field
        else if (m_KillCount == 42)
            ObjectCtrl(7, true);
        else if (m_KillCount == 49)
            ObjectCtrl(8, true);
        else if (m_KillCount == 52)
            ObjectCtrl(9, true);
        else if (m_KillCount == 57)
            ObjectCtrl(10, true);
        else if (m_KillCount == 61)
            ObjectCtrl(11, true);
        else if (m_KillCount == 84) // 3-3 Spawn
            ObjectCtrl(13, true);
        else if (m_KillCount == 91) // 3-3 Spawn
            ObjectCtrl(14, true);
        else if (m_KillCount == 93) // 3-3 Spawn
            ObjectCtrl(15, true);
        else if (m_KillCount == 103) // 3-5 Spawn
            ObjectCtrl(17, true);
        else if (m_KillCount == 109) // 3-5 Spawn
            ObjectCtrl(18, true);
        else if (m_KillCount == 114) // 3-5 Spawn
            ObjectCtrl(19, true);
    }

    public void ObjectCtrl(int GateNumber, bool Active)
    {
        m_CountObject[GateNumber].SetActive(Active);
    }
}
