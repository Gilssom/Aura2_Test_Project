using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtChanMonster : MonoBehaviour
{
    private ArtChanController m_Player;

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ArtChanController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            m_Player.SkillAttack();
        }
    }
}
