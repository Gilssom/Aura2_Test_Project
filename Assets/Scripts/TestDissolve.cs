using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDissolve : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public float Speed = default;
    //private float t = 0.0f;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    public float Cutoff = default;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.F))
        {
            if(Cutoff >= MaxCutoff)
            {
                Destroy(gameObject);
            }

            Material[] mats = _meshRenderer.materials;

            Cutoff += Speed;
            //mats[0].SetFloat("_Cutoff", ++);
            if(Cutoff != MaxCutoff)
            {
                this.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", Cutoff);
            }
            //t += Time.deltaTime;

            _meshRenderer.materials = mats;
            
        }
    }
}
