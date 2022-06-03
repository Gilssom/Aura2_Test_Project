using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public enum Type { Health, Soul };

    public Type type;

    Vector3 m_CurPos;

    void Awake()
    {
        m_CurPos = transform.position;
    }

    void Update()
    {
        Vector3 v = m_CurPos;
        v.y += 0.5f * Mathf.Sin(Time.time * 1);
        transform.position = v;
    }
}
