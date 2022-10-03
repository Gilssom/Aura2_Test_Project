using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBarFrontCam : MonoBehaviour
{
    private Camera m_Camera;

    void Start()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
            m_Camera.transform.rotation * Vector3.up);
    }
}
