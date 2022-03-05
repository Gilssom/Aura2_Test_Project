using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtChanController : MonoBehaviour
{
    private Animator _Animator;
    private Transform characterBody;

    public Transform cameraArm;

    private Vector3 moveDirection = Vector3.zero;
    Vector2 movement = new Vector2();

    public float Speed;
    public float MinSpeed = 2;
    public float MaxSpeed = 10;
    public float gravity = 10;

    private bool isRun = false;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    float HAxis;
    float VAxis;

    public BoxCollider m_FirstArea;
    public BoxCollider m_SecondArea;
    public BoxCollider m_FinalArea;

    public float m_SkillStack;
    public float m_StackTime;

    public bool m_AttackStacking;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
        characterBody = GetComponent<Transform>();
    }

    void Start()
    {
        m_FirstArea.enabled = false;
        m_SecondArea.enabled = false;
        m_FinalArea.enabled = false;

        m_SkillStack = 0;
        m_StackTime = 1;
    }

    void Update()
    {
        GetInput();
        Move();
        Attack();

        if(m_AttackStacking)
        {
            m_StackTime -= Time.deltaTime * 1;

            if(m_StackTime <= 0)
            {
                m_SkillStack = 0;
                m_AttackStacking = false;

                if(!m_AttackStacking)
                {
                    m_StackTime = 1;
                }
            }
        }
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(HAxis, VAxis);

        bool isMove = moveInput.magnitude != 0;

        if(isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;

            transform.position += moveDir * Time.deltaTime * Speed;

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                isRun = true;
                _Animator.SetBool("isRun", true);
                Speed = MaxSpeed;
            }
            else
            {
                isRun = false;
                _Animator.SetBool("isRun", false);
                Speed = MinSpeed;
            }
        }

        _Animator.SetBool("isWalk", isMove);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && comboStep == 0 && !isRun)
        {
            _Animator.SetBool("isWalk", false);

            _Animator.SetTrigger("Attack");
            comboStep = 1;
            isAttackReady = false;
            return;
        }

        if (comboStep != 0)
        {
            if (ComboPossible && Input.GetMouseButtonDown(0))
            {
                ComboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void SkillAttack()
    {
        m_StackTime = 1;
        m_AttackStacking = true;
        float MaxStack = 3;
        if (m_SkillStack < MaxStack)
        {
            m_SkillStack++;
        }

        else if(m_SkillStack == MaxStack)
        {
            Debug.Log("Skill Enable True");
        }
    }

    void Combopossible()
    {
        ComboPossible = true;
    }
    void Combo()
    {
        if (comboStep == 2)
        {
            _Animator.SetTrigger("SecondAttack");
        }

        else if (comboStep == 3)
        {
            _Animator.SetTrigger("FinalAttack");
        }
    }
    void ComboReset()
    {
        _Animator.ResetTrigger("SecondAttack");
        _Animator.ResetTrigger("FinalAttack");
        ComboPossible = false;
        isAttackReady = true;
        comboStep = 0;
    }

    void FirstAttack()
    {
        m_FirstArea.enabled = true;
    }
    void SecondAttack()
    {
        m_SecondArea.enabled = true;
    }
    void FinalAttack()
    {
        m_FinalArea.enabled = true;
    }
    void AreaEnd()
    {
        m_FirstArea.enabled = false;
        m_SecondArea.enabled = false;
        m_FinalArea.enabled = false;
    }
}
