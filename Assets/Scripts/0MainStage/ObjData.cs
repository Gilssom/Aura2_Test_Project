using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjData : MonoBehaviour
{
    /// <summary>
    /// ObjectNumber
    /// 1 = Sotdae
    /// 2 = Sky Object
    /// 3 ~ 5 = Fence  ( Dissolve ) ( Frist ~ Third Field )
    /// 6 ~ 7 = Door   ( Dissolve ) ( Frist ~ Third Field )
    /// 8 ~ 12 = Fence ( Dissolve ) ( Forth ~ Seventh Field )
    /// 13 ~ 16 = Door  ( Dissolve ) ( Forth ~ Seventh Field )
    /// ¤¤> 14  = Reverse Door ( Reverse Dissolve )
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
    private float Speed = default;
    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    private float Cutoff = default;
    private bool isFire = false;

    int Kill;
    float OnFire;
    bool OnDoor;

    public Vector3 m_DownPos;

    void Start()
    {
        // First ~ Third Field
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
            Speed = 0.0025f;

        // Forth ~
        if (id == 0)
        {
            isFire = true;
            m_Child = transform.GetChild(0).gameObject;
        }
        if (id == 5 || id == 8 || id == 9 || id == 10 || id == 11 || id == 12 || id == 13 || id == 14)
            Speed = 0.0025f;
    }

    void Update()
    {
        Kill = GameManager.Instance.m_KillCount;
        OnFire = FieldObjectController.Instance.m_FireOnCount;

        // First ~ Third Field
        if (id == 2)
        {
            Vector3 v = m_CurPos;
            v.y += m_Ydelta * Mathf.Sin(Time.time * m_MoveSpeed);
            transform.position = v;
        }
        if(id == 3 && OnFire >= 2)       
            FenceDissolve();       
        if (id == 4 && OnFire >= 4)        
            FenceDissolve();
        if (id == 6 && !OnDoor)
            StartCoroutine(DoorDown(m_DownPos, 2));
        if (id == 7 && !OnDoor)
            StartCoroutine(DoorDown(m_DownPos, 4));

        // Forth ~ Seventh Field
        if (id == 5 && OnFire >= 6)
            FenceDissolve();
        if (id == 8 && Kill == 27)
            FenceDissolve();
        if (id == 9 && Kill == 35)
            FenceDissolve();
        if (id == 10 && Kill == 67)
            FenceDissolve();
        if (id == 11 && Kill == 71)
            FenceDissolve();
        if (id == 12 && Kill == 121)
            FenceDissolve();
        if (id == 13 && !OnDoor)
            StartCoroutine(DoorDown(m_DownPos, 6));
        if (id == 14 && FieldObjectController.Instance.m_FireOffCount == 2)
            FenceDissolve();
        if (id == 15 && !OnDoor) // 3-2 Spawn
            StartCoroutine(DoorDown(m_DownPos, 12));
        if (id == 16 && !OnDoor) // 3-4 Spawn
            StartCoroutine(DoorDown(m_DownPos, 16));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (id == 1 || id == 0)
        {
            var audio = GetComponent<AudioSource>();

            if (other.tag == "FireWeapon" && !isFire)
            {
                if(!isFire)
                    FieldObjectController.Instance.m_FireOnCount += 1;
                
                isFire = true;
                SoundManager.Instance.SFXPlay("Fire On", GameManager.Instance.m_Clip[1]);
                m_Child.SetActive(true);
            }
            else if (other.tag == "IceWeapon" && isFire)
            {
                if (isFire)
                {
                    FieldObjectController.Instance.m_FireOnCount -= 1;

                    if(id == 0)
                        FieldObjectController.Instance.m_FireOffCount += 1;
                }

                isFire = false;
                m_Child.SetActive(false);
            }
            if(audio)
                audio.enabled = isFire;
        }
    }

    void FenceDissolve()
    {
        BoxCollider coll = GetComponent<BoxCollider>();

        if (coll)
            coll.enabled = false;

        if (Cutoff >= MaxCutoff)
        {
            Destroy(gameObject);
        }

        Cutoff += Speed;
        if (Cutoff != MaxCutoff)
        {
            this.GetComponent<MeshRenderer>().material.SetFloat("_Dissolve", Cutoff);
        }
    }

    IEnumerator DoorDown(Vector3 SpawnPos , int ObjNum)
    {
        OnDoor = true;
        SoundManager.Instance.SFXPlay("DoorSFX", GameManager.Instance.m_Clip[0]);
        yield return new WaitForSeconds(0.6f);
        this.gameObject.transform.DOMove(SpawnPos, 0.1f);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.ObjectCtrl(ObjNum, true);

        yield return null;
    }

    /*void DoorDissolve(int ObjNum)
    {
        BoxCollider coll = GetComponent<BoxCollider>();

        if (coll)
            coll.enabled = true;

        Cutoff -= Speed;
        if (Cutoff != MinCutoff && Cutoff >= MinCutoff)
        {
            this.GetComponent<MeshRenderer>().material.SetFloat("_Dissolve", Cutoff);
        }
        else if (Cutoff <= MinCutoff)
        {
            Speed = 0;
            GameManager.Instance.ObjectCtrl(ObjNum, true);
        }
    }*/
}
