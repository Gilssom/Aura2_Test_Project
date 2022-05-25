using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectController : MonoBehaviour
{
    public int m_ObjectNumber;

    public GameObject[] m_ObjectEvent;

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
