using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterView : MonoBehaviour
{
    public Transform m_Target;
    public Vector3 m_offset;

    void Update()
    {
        transform.position = m_Target.position + m_offset;
    }
}
