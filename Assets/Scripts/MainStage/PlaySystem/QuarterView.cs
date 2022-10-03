using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterView : MonoBehaviour
{
    public Transform m_Target;
    public Vector3 m_offset;
    public GameObject m_Fog;

    public bool isZoomOut;

    private static QuarterView m_instance;

    public static QuarterView Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(QuarterView)) as QuarterView;

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
        if (!isZoomOut)
            transform.position = m_Target.position + m_offset;

        FogOnOff();
    }

    void FogOnOff()
    {
        if (SoundManager.Instance.m_SceneName == "VillageStage")
            m_Fog.SetActive(false);
        else
            m_Fog.SetActive(true);
    }
}
