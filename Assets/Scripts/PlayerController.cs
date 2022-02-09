using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private WeaponTest _Weapon;

    private Animator _Animator;
    private PlayerInputSystem _playerInput;
    private Rigidbody _Rigid;
    private CapsuleCollider _Collider;

    private Vector3 moveDirection = Vector3.zero;
    Vector2 movement = new Vector2();

    public float gravity = 10;
    //public float jumpSpeed = 4;
    public float MaxSpeed = 10;
    public float AttackMovePower = default;
    public float AttackMoveTime = default;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    private bool isRun = false;

    float HAxis;
    float VAxis;

    public ParticleSystem _Particle;
    public GameObject _SpringArm;

    public float _Strength;

    void Start()
    {
        _Weapon = GetComponentInChildren<WeaponTest>();

        _Animator = GetComponent<Animator>();
        _Rigid = GetComponent<Rigidbody>();
        _Collider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        GetInput();
        Move();
        Attack();
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
            transform.DOMove(transform.position + transform.forward * AttackMovePower, AttackMoveTime).SetEase(Ease.InQuad);
            _SpringArm.transform.DOMove(_SpringArm.transform.position + _SpringArm.transform.forward * 0.1f, 0.05f).SetLoops(2, LoopType.Yoyo);
        }
    }

    void AttackFalse()
    {
        _Weapon.AttackEnd();
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
}
