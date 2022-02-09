using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTest : MonoBehaviour
{
    private PlayerController _Player;

    public enum Type { Shield, Sword }
    public Type _WeaponType;

    private BoxCollider _AttackCol;

    public float _AttackDmg;

    void Start()
    {
        _Player = GetComponentInParent<PlayerController>();

        _AttackCol = GetComponent<BoxCollider>();
        _AttackCol.enabled = false;
    }
    
    void Update()
    {
        
    }

    public void AttackStart()
    {
        //if (_WeaponType == Type.Shield)
        //{
        //    _AttackCol.enabled = true;
        //}

        //if (_WeaponType == Type.Sword)
        //{
        //    _AttackCol.enabled = true;
        //}
    }

    public void AttackEnd()
    {
        _AttackCol.enabled = false;
    }
}
