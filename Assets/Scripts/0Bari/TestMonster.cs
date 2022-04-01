using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WinterIce")
        {
            Debug.Log("Monster Dmg Check");
        }
    }
}
