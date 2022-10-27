using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashBullet : MonoBehaviour
{
    Rigidbody m_Rigid = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        if(m_Rigid == null)
        {
            m_Rigid = GetComponent<Rigidbody>();
        }

        //m_Rigid.velocity = transform.forward * 20;

        //StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1.5f);
        ObjectPoolManager.instance.m_ObjectPoolList[0].Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
