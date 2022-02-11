using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTest : MonoBehaviour
{
    private PlayerController _Player;

    private BoxCollider _AttackCol;

    public int _AttackDmg;

    void Start()
    {
        _Player = GetComponentInParent<PlayerController>();

        _AttackCol = GetComponent<BoxCollider>();
        _AttackCol.enabled = false;
    }
    
    void Update()
    {
        
    }
}
