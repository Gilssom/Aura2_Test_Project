using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneMonsterText : MonoBehaviour
{
    private TestShot m_Target;
    private MeshRenderer m_meshRenderer;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    public float OutlineCutoff = default;

    void Start()
    {
        m_Target = GameObject.FindWithTag("Player").GetComponent<TestShot>();
        m_meshRenderer = GetComponent<MeshRenderer>();
    }


    void Update()
    {
        if(m_Target._Target != this.gameObject)
        {
            this.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0);
        }
    }

    public void OutLineCheck()
    {
        this.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.05f);         
    }
}
