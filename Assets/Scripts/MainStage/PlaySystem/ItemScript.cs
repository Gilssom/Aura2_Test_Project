using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemScript : MonoBehaviour
{
    private Transform m_Player;
    public enum Type { Health, Soul };
    public Type type;
    public int PoolNumber;

    bool isMagnet;

    void Awake()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        isMagnet = false;
    }

    void Update()
    {
        if(!isMagnet)
        {
            Vector3 v = transform.position;
            v.y += 0.005f * Mathf.Sin(Time.time * 1);
            transform.position = v;
        }

        float Dis = Vector3.Distance(m_Player.position, this.transform.position);

        switch (type)
        {
            case Type.Health:
                if(PlayerStats.Instance.Health < 4)
                    if (Dis <= 3)
                    {
                        isMagnet = true;
                        this.transform.DOMove(m_Player.position, 1f);
                    }
                break;
            case Type.Soul:
                if (Dis <= 3)
                {
                    isMagnet = true;
                    this.transform.DOMove(m_Player.position, 1f);
                }
                break;
        }
    }

    public void ObjectReturn()
    {
        ObjectPoolManager.instance.m_ObjectPoolList[PoolNumber].Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}
