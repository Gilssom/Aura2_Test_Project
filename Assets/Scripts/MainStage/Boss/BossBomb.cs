using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    public int PoolNumber;

    public GameObject m_Effect;
    public AudioClip m_BombSound;

    Vector3 Pos;

    private void Update()
    {
        Pos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor")
        {
            SoundManager.Instance.SFXPlay("Bomb Sound", m_BombSound);
            Instantiate(m_Effect, new Vector3(Pos.x, Pos.y + 6.68f, Pos.z), Quaternion.identity);

            ObjectPoolManager.instance.StartCoroutine(ObjectPoolManager.instance.DestroyObj(1.5f, PoolNumber, gameObject));
        }
    }
}
