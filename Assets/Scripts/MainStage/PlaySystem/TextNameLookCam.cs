using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNameLookCam : MonoBehaviour
{
    public GameObject m_Cam;

    void Start()
    {
        m_Cam = GameObject.Find("Main Camera");    
    }

    void Update()
    {
        transform.rotation = m_Cam.transform.rotation;
    }
}
