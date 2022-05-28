using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class FieldObjectController : MonoBehaviour
{
    public int m_ObjectNumber;

    /// <summary>
    /// ObjectNumber
    /// 1 = Jangseung
    /// 2 = Sky Light Lamp
    /// </summary>

    public GameObject[] m_ObjectEvent;
    Vector3 m_CurPos;
    public bool isYMoving;
    [SerializeField]
    float m_Ydelta;
    public float m_MoveSpeed;

    void Start()
    {
        m_CurPos = transform.position;
        //GetRandom();
        m_Ydelta = Random.Range(0.5f, 1);
        m_MoveSpeed = Random.Range(1, 1.5f);
    }

    void GetRandom()
    {
        var exclude = new HashSet<float>() { 0 };
        var range = Enumerable.Range(1, 3).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(0, 3 - exclude.Count);
        m_MoveSpeed = range.ElementAt(index);
        //return range.ElementAt(index);
    }

    void Update()
    {
        if(m_ObjectNumber == 2)
        {
            Vector3 v = m_CurPos;

            if (isYMoving)
            {
                v.y += m_Ydelta * Mathf.Sin(Time.time * m_MoveSpeed);
            }

            transform.position = v;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_ObjectNumber == 1)
        {
            if (other.tag == "FireWeapon")
                m_ObjectEvent[0].SetActive(true);
            else if(other.tag == "IceWeapon")
                m_ObjectEvent[0].SetActive(false);
        }
    }
}
