using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    public GameObject m_Effect;
    public AudioClip m_BombSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor")
        {
            SoundManager.Instance.SFXPlay("Bomb Sound", m_BombSound);
            Instantiate(m_Effect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
