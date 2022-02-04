using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _Animator;
    private PlayerInputSystem _playerInput;
    private CharacterController _CharacterController;
    private Transform _transform;

    private Vector3 moveDirection = Vector3.zero;
    Vector2 movement = new Vector2();

    public float gravity = 10;
    //public float jumpSpeed = 4;
    public float MaxSpeed = 10;

    void Start()
    {
        _Animator = GetComponent<Animator>();
        _CharacterController = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        Move();
        Attack();

        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.Translate(Vector3.forward * 100 * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        movement.Normalize();

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            _Animator.SetBool("Run", true);
            MaxSpeed = 6;
        }
        else
        {
            _Animator.SetBool("Run", false);
            MaxSpeed = 3;
        }
    }

    void Move()
    {
        var x = movement.x;
        var y = movement.y;

        bool grounded = _CharacterController.isGrounded;

        if (grounded)
        {
            moveDirection = new Vector3(x, 0, y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= MaxSpeed;
            //if (_playerInput.JumpInput)
            //    moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        _CharacterController.Move(moveDirection * Time.deltaTime);

        _Animator.SetFloat("InputX", x);
        _Animator.SetFloat("InputY", y);
        //_Animator.SetBool("IsInAir", !grounded);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _Animator.SetTrigger("Attack");
        }
    }

    void AttackFire()
    {
        Debug.Log("Attack");
    }
}
