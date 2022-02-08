using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterTest : MonoBehaviour
{
    public enum Type { A, B, C}
    public Type _enemyType;

    public int _MaxHp;
    public int _Hp;

    private MeshRenderer _meshRenderer;
    private SkinnedMeshRenderer _skinmeshRenderer;

    public float Speed = default;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    public float Cutoff = default;

    private bool isDeath = false;

    private Animator _anim;

    void Start()
    {
        _skinmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(_Hp <= 0)
        {
            _anim.SetTrigger("Death");
            Death();
        }
    }

    void DeathAnimCheak()
    {
        isDeath = true;
    }

    void Death()
    {
        switch (_enemyType)
        {
            case Type.A:
                if(isDeath)
                {
                    if (Cutoff >= MaxCutoff)
                    {
                        Destroy(gameObject);
                    }

                    Material[] mats = _skinmeshRenderer.materials;

                    Cutoff += Speed;
                    //mats[0].SetFloat("_Cutoff", ++);
                    if (Cutoff != MaxCutoff)
                    {
                        this.GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("_Cutoff", Cutoff);
                    }
                    //t += Time.deltaTime;

                    _skinmeshRenderer.materials = mats;

                }
                break;
            case Type.B:
                break;
            case Type.C:
                break;
        }
    }
}
