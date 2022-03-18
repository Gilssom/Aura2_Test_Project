using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    private NavMeshAgent _navAgent;

    public Transform m_MovePos;

    private static NPCMove m_instance;
    // ΩÃ±€≈Ê
    public static NPCMove Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(NPCMove)) as NPCMove;

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
        _navAgent = GetComponent<NavMeshAgent>();
    }

    public void Move()
    {
        _navAgent.destination = m_MovePos.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FirstPortal")
        {
            Destroy(gameObject);
        }
    }
}
