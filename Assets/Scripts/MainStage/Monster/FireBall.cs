using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject m_Effect;

    private void OnEnable()
    {
        StartCoroutine(DesObject());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Floor")
        {
            Instantiate(m_Effect, transform.position, Quaternion.identity);
            ObjectPoolManager.instance.m_ObjectPoolList[8].Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }

    IEnumerator DesObject()
    {
        //Debug.Log("FireBall StartCoroutine");
        yield return new WaitForSeconds(3);

        ObjectPoolManager.instance.m_ObjectPoolList[8].Enqueue(gameObject);
        gameObject.SetActive(false);
        yield return null;
    }
}
