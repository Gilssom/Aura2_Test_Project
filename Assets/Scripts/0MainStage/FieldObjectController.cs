using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class FieldObjectController : MonoBehaviour
{
    public float m_FireOnCount;

    private static FieldObjectController m_instance;
    // ΩÃ±€≈Ê
    public static FieldObjectController Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(FieldObjectController)) as FieldObjectController;

                if (m_instance == null)
                    Debug.Log("No Singletone Obj");
            }
            return m_instance;
        }
    }
    void Awake()
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
        m_FireOnCount = 0;
    }
}
