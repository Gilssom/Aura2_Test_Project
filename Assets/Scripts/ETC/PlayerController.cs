using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameManager _Gamemanager;
    private WeaponTest _Weapon;

    private Animator _Animator;
    private PlayerInputSystem _playerInput;
    private Rigidbody _Rigid;
    private CapsuleCollider _Collider;

    [SerializeField]
    public MonsterTest _Monster;

    private Vector3 moveDirection = Vector3.zero;
    Vector2 movement = new Vector2();

    public float gravity = 10;
    //public float jumpSpeed = 4;
    public float MaxSpeed = 10;
    public float AttackMovePower = default;
    public float AttackMoveTime = default;

    public int _MaxHP;
    public int _CurHP;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    private bool isRun = false;

    float HAxis;
    float VAxis;

    public ParticleSystem _Particle;
    public GameObject _SpringArm;

    public float _Strength;

    public GameObject[] _WeaponArray;

    public bool isPortal = false;

    public int PlayerNum;

    void Awake()
    {
        //_Monster = GameManager.Instance.m_Monster;
        _Gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _SpringArm = GameObject.FindWithTag("SpringArm");
        _Weapon = GetComponentInChildren<WeaponTest>();
        _Animator = GetComponent<Animator>();
        _Rigid = GetComponent<Rigidbody>();
        _Collider = GetComponent<CapsuleCollider>(); 

        _CurHP = _MaxHP;

        /*if (_Gamemanager.PlayerIndex != PlayerNum)
        {
            Debug.Log("None");
            Destroy(gameObject);
        }
        else if(_Gamemanager.PlayerIndex == PlayerNum)
        {
            GameManager.Instance.m_Player = this.gameObject;
        }*/

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(!isPortal)
        {
            GetInput();
            Move();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");
    }

    void MoveCharacter()
    {
        movement.x = HAxis;
        movement.y = VAxis;

        movement.Normalize();

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            isRun = true;
            _Animator.SetBool("Run", true);
            MaxSpeed = 6;
        }
        else
        {
            isRun = false;
            _Animator.SetBool("Run", false);
            MaxSpeed = 3;
        }
    }

    void Move()
    {
        var x = movement.x;
        var y = movement.y;

        moveDirection = new Vector3(x, 0, y);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= MaxSpeed;

        moveDirection.y -= gravity * Time.deltaTime;
        transform.position += (moveDirection * Time.deltaTime);

        _Animator.SetFloat("InputX", x);
        _Animator.SetFloat("InputY", y);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && comboStep == 0 && !isRun)
        {
            _Animator.SetTrigger("Attack");
            _Particle.Play();
            comboStep = 1;
            isAttackReady = false;
            return;
        }

        if(comboStep != 0)
        {
            if(ComboPossible && Input.GetMouseButtonDown(0))
            {
                ComboPossible = false;
                comboStep += 1;
            }
        }
    }

    void AttackFire()
    {
        if (movement.x == 0 && movement.y == 0)
        {
            //transform.DOMove(transform.position + transform.forward * AttackMovePower, AttackMoveTime).SetEase(Ease.InQuad);
            _SpringArm.transform.DOMove(_SpringArm.transform.position + _SpringArm.transform.forward * 0.1f, 0.05f).SetLoops(2, LoopType.Yoyo);
        }
    }

    void ShieldAttackTrue()
    {
        _WeaponArray[0].GetComponent<BoxCollider>().enabled = true;
    }

    void SwordAttackTrue()
    {
        _WeaponArray[1].GetComponent<BoxCollider>().enabled = true;
    }

    void AttackFalse()
    {
        _WeaponArray[0].GetComponent<BoxCollider>().enabled = false;
        _WeaponArray[1].GetComponent<BoxCollider>().enabled = false;
    }

    void Combopossible()
    {
        ComboPossible = true;
    }
    void Combo()
    {
        if (comboStep == 2)
        {
            _Animator.SetTrigger("NextAttack");
        }
    }
    void ComboReset()
    {
        ComboPossible = false;
        isAttackReady = true;
        comboStep = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MonsterWeapon")
        {
            _CurHP -= _Monster._AttackDmg;
            OnDamage();
        }

        if(other.tag == "Portal")
        {
            isPortal = true;
            //_Gamemanager.InStartFadeAnim();
        }
    }

    void OnDamage()
    {
        _SpringArm.transform.DOShakePosition(0.2f, 0.3f, 8, 90, false, true);
    }
}
