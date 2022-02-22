using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneMonsterText : MonoBehaviour
{
    private TestShot m_Target;
    public float OutlineCutoff = default;

    void Start()
    {
        m_Target = GameObject.FindWithTag("Player").GetComponent<TestShot>();
    }


    void Update()
    {
        if(m_Target._Target != this.gameObject)
        {
            this.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0);
        }
    }
}
