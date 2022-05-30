using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    /// <summary>
    /// ObjectNumber
    /// 0 = Jangseung
    /// 1 = Sky Light Lamp
    /// 2 = Fence ( Dissolve )
    /// </summary>
    public int id;
    public bool isNpc;

    // Jangseung properties
    private GameObject m_Child;

    // Light Lamp properties
    Vector3 m_CurPos;
    float m_Ydelta;
    float m_MoveSpeed;

    // Fence properties
    private MeshRenderer m_meshRenderer;
    private float Speed = default;
    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    private float Cutoff = default;
    private bool isFire = false;

    void Start()
    {
        if (id == 1)
        {
            m_Child = transform.GetChild(0).gameObject;
        }

        if (id == 2)
        {
            m_CurPos = this.transform.position;
            m_Ydelta = Random.Range(0.5f, 1);
            m_MoveSpeed = Random.Range(1, 1.5f);
        }

        if (id == 3 || id == 4)
        {
            Speed = 0.0025f;
            m_meshRenderer = GetComponent<MeshRenderer>();
        }
    }

    void Update()
    {
        if (id == 2)
        {
            Vector3 v = m_CurPos;
            v.y += m_Ydelta * Mathf.Sin(Time.time * m_MoveSpeed);

            transform.position = v;
        }

        if(id == 3 && FieldObjectController.Instance.m_FireOnCount >= 2)
        {
            FenceDissolve();
        }
        if (id == 4 && FieldObjectController.Instance.m_FireOnCount >= 4)
        {
            FenceDissolve();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (id == 1)
        {
            if (other.tag == "FireWeapon")
            {
                if(!isFire)
                    FieldObjectController.Instance.m_FireOnCount += 1;
                
                isFire = true;
                m_Child.SetActive(true);
            }
            else if (other.tag == "IceWeapon")
            {
                if (isFire)
                    FieldObjectController.Instance.m_FireOnCount -= 1;

                isFire = false;
                m_Child.SetActive(false);
            }
        }
    }

    void FenceDissolve()
    {
        BoxCollider coll = GetComponent<BoxCollider>();

        coll.enabled = false;


        if (Cutoff >= MaxCutoff)
        {
            Destroy(gameObject);
        }

        Material[] mats = m_meshRenderer.materials;

        Cutoff += Speed;
        if (Cutoff != MaxCutoff)
        {
            this.GetComponent<MeshRenderer>().material.SetFloat("_Dissolve", Cutoff);
        }

        m_meshRenderer.materials = mats;

        }
    }
