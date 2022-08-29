using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    public GameObject m_Effect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor")
        {
            Instantiate(m_Effect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
