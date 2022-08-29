using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class BossFollowPath : MonoBehaviour
{
    [SerializeField]
    private Boss m_Boss;

    public PathCreator m_PathCreator;
    [SerializeField]
    private float m_PathSpeed = 1;
    [SerializeField]
    float m_DistanceTravelled;

    private void Awake()
    {
        this.enabled = false;
        m_Boss = GetComponent<Boss>();
    }

    void Update()
    {
        if (m_PathCreator != null)
        {
            m_Boss.Attack(5);
            m_DistanceTravelled += m_PathSpeed * Time.deltaTime;
            transform.position = m_PathCreator.path.GetPointAtDistance(m_DistanceTravelled, EndOfPathInstruction.Stop);
            transform.rotation = m_PathCreator.path.GetRotationAtDistance(m_DistanceTravelled, EndOfPathInstruction.Stop);

            if (m_DistanceTravelled >= 15.3f)
            {
                m_DistanceTravelled = 0;
                this.enabled = false;
                m_Boss.AttackFalse(5);
            }
        }
    }
}
