using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    private NavMeshAgent m_NavAgent;

    [SerializeField] Transform[] m_WayPoints = null;
    public int m_count;

    public bool isTalking;

    public Transform m_MovePos;

    private static NPCMove m_instance;
    // ½Ì±ÛÅæ
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

    void MoveToNextWayPoint()
    {
        if (m_NavAgent.velocity == Vector3.zero)
        {
            m_NavAgent.speed = 4;
            m_NavAgent.SetDestination(m_WayPoints[m_count++].position);

            if (m_count >= m_WayPoints.Length)
            {
                m_count = 0;
            }
        }
    }

    void Start()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();

        isTalking = false;

        InvokeRepeating("MoveToNextWayPoint", 2f, 2f);
    }

    void Update()
    {
        if (isTalking)
        {
            CancelInvoke();
            m_NavAgent.speed = 0;
        }
    }

    public void Move()
    {
        m_NavAgent.destination = m_MovePos.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FirstPortal")
        {
            Destroy(gameObject);
        }
    }
}
