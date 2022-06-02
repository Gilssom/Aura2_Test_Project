using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class MonsterTest : MonoBehaviour
{
    public enum Type { A, B, C}
    public Type _enemyType;

    public enum CurrentState { idle, trace, attack, dead}
    public CurrentState curState = CurrentState.idle;

    public int _MaxHp;
    public int _Hp;
    public int _AttackDmg;

    public GameObject m_HealthBar;
    public Slider m_Slider;

    private MeshRenderer _meshRenderer;
    private SkinnedMeshRenderer _skinmeshRenderer;
    private CapsuleCollider _CapsuleCol;
    private Rigidbody _Rigid;
    private Transform _transform;
    private Transform _Playertransform;
    private NavMeshAgent _navAgent;
    private Animator _anim;

    public GameObject m_Base;

    [SerializeField] Transform[] _WayPoints = null;
    public int m_count;

    public GameObject _RightAttack;
    public GameObject _LeftAttack;

    public float trastDis = 15;
    public float attackDis = 1.5f;

    public float Speed = default;

    private float MaxCutoff = 1;
    private float MinCutoff = 0;
    public float Cutoff = default;

    private bool isDeath = false;
    private bool isNotNav = false;

    public float _rotSpeed;

    bool isIdle;

    void MoveToNextWayPoint()
    {
        if (_navAgent.velocity == Vector3.zero)
        {
            _anim.SetBool("isPatrol", true);
            _navAgent.speed = 0.5f;
            _navAgent.SetDestination(_WayPoints[m_count++].position);

            if (m_count >= _WayPoints.Length)
            {
                m_count = 0;
            }
        }
    }

    void Start()
    {
        _skinmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _anim = GetComponent<Animator>();
        _CapsuleCol = GetComponentInChildren<CapsuleCollider>();
        _Rigid = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        //_Playertransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //_Playertransform = GameManager.Instance.m_Player.transform;

        m_Slider.value = CulHealth();
        m_Slider.maxValue = _MaxHp;
        m_HealthBar.SetActive(false);

        // 바로 추적
        //_navAgent.destination = _Playertransform.position;

        InvokeRepeating("MoveToNextWayPoint", 2f, 2f);

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    void Update()
    {
        if(!isNotNav)
        {
            if (_navAgent.remainingDistance <= 0.2f)
            {
                _anim.SetBool("isPatrol", false);
            }
        }

        m_Slider.value = CulHealth();

        if(_Hp < _MaxHp)
        {
            m_HealthBar.SetActive(true);
        }

        if (_Hp <= 0)
        {
            _anim.SetTrigger("Death");
            Death();
        }
    }

    void LookPlayer()
    {
        Vector3 dir = _Playertransform.position - this.transform.position;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotSpeed);
    }

    IEnumerator CheckState()
    {
        while(!isDeath)
        {
            yield return new WaitForSeconds(0.2f);

            float Dis = Vector3.Distance(_Playertransform.position, _transform.position);

            if (Dis <= attackDis)
            {
                curState = CurrentState.attack;
            }
            else if (Dis <= trastDis)
            {
                curState = CurrentState.trace;
            }
            else
            {
                curState = CurrentState.idle;
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while(!isDeath)
        {
            switch (curState)
            {
                case CurrentState.idle:
                    //_navAgent.speed = 0;
                    //_navAgent.Stop();
                    _anim.SetBool("isTrace", false);
                    break;
                case CurrentState.trace:
                    _navAgent.speed = 5;      
                    _Rigid.isKinematic = false;          
                    _navAgent.destination = _Playertransform.position;
                    _navAgent.Resume();
                    LookPlayer();
                    _anim.SetBool("isPatrol", false);
                    _anim.SetBool("isAttack", false);
                    _anim.SetBool("isTrace", true);                
                    break;
                case CurrentState.attack:
                    _navAgent.speed = 0;
                    _Rigid.isKinematic = true;
                    LookPlayer();
                    _anim.SetBool("isPatrol", false);
                    _anim.SetBool("isTrace", false);
                    _anim.SetBool("isAttack", true);
                    break;
            }

            yield return null;
        }
    }

    void RightAttack()
    {
        _LeftAttack.GetComponent<BoxCollider>().enabled = false;
        _RightAttack.GetComponent<BoxCollider>().enabled = true;
    }

    void LeftAttack()
    {
        _RightAttack.GetComponent<BoxCollider>().enabled = false;
        _LeftAttack.GetComponent<BoxCollider>().enabled = true;
    }

    void DeathAnimCheak()
    {
        isDeath = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            WeaponTest Weapon = other.GetComponent<WeaponTest>();

            _Hp -= Weapon._AttackDmg;
        }

        if(other.tag == "Missile")
        {
            BazierMissile Missile = other.GetComponent<BazierMissile>();

            //_Hp -= Missile.m_Dmg;
        }
    }

    int CulHealth()
    {
        return _Hp;
    }

    void Death()
    {
        StopAllCoroutines();
        CancelInvoke();
        m_Base.layer = 12;
        m_Base.tag = "Untagged";
        _navAgent.enabled = false;
        isNotNav = true;

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
