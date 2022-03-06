using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtChanController : MonoBehaviour
{
    private Animator _Animator;
    private Transform characterBody;
    private Rigidbody m_Rigid;

    public Transform cameraArm;

    private CapsuleCollider m_Coll;
    public Transform m_YAxisColl;

    private Vector3 moveDirection = Vector3.zero;
    Vector2 movement = new Vector2();

    public float Speed;
    public float MinSpeed = 2;
    public float MaxSpeed = 10;
    public float ParryingSpeed = 0.5f;
    public float gravity = 10;
    public float JumpPower;

    private bool isRun = false;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    float HAxis;
    float VAxis;
    bool Run;

    public BoxCollider m_FirstArea;
    public BoxCollider m_SecondArea;
    public BoxCollider m_FinalArea;

    public float m_SkillStack;
    public float m_SkillMaxStack = 3;
    public float m_StackTime;
    public bool m_SkillEnable;

    public bool m_AttackStacking;

    bool m_FinalAttack;
    [SerializeField]
    bool isJumping;
    bool isMoving;

    public bool isParrying;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
        characterBody = GetComponent<Transform>();
        m_Rigid = GetComponent<Rigidbody>();
        m_Coll = GetComponentInChildren<CapsuleCollider>();
    }

    void Start()
    {
        m_FirstArea.enabled = false;
        m_SecondArea.enabled = false;
        m_FinalArea.enabled = false;
        m_FinalAttack = false;

        m_SkillEnable = false;

        m_SkillStack = 0;
        m_StackTime = 1;
    }

    void Update()
    {
        GetInput();
        if(!m_FinalAttack)
        {
            Move();
        }
        Attack();
        Parrying();
        QSkill();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            Jump();
        }

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

        //m_Coll.transform.position = m_YAxisColl.transform.position;
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");
        Run = Input.GetButton("Parrying");

        if (!Run || HAxis == 0 && VAxis == 0)
        {
            isMoving = false;
            isRun = false;
            _Animator.SetBool("isRun", false);
            Speed = MinSpeed;
        }
        else
            isMoving = true;

        if (Input.GetMouseButton(1))
        {
            isParrying = true;
        }
        else
            isParrying = false;
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
            if (Run && isMoving && !isParrying)
            {
                isRun = true;
                _Animator.SetBool("isRun", true);
                Speed = MaxSpeed;
            }
            //if (!isMoving)
            //{
            //    isRun = false;
            //    _Animator.SetBool("isRun", false);
            //    Speed = MinSpeed;
            //}
        }

        _Animator.SetBool("isWalk", isMove);
    }

    void Jump()
    {
        _Animator.SetTrigger("DoJump");
        m_Rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);

        isJumping = false;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && comboStep == 0 && !isRun)
        {
            _Animator.SetBool("isWalk", false);

            _Animator.SetTrigger("Attack");
            comboStep = 1;
            isAttackReady = false;
            m_FinalAttack = true;
            return;
        }

        if (comboStep != 0)
        {
            if (ComboPossible && Input.GetMouseButtonDown(0))
            {
                m_FinalAttack = true;
                ComboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void SkillAttack()
    {
        m_StackTime = 1;
        m_AttackStacking = true;
        if (m_SkillStack < m_SkillMaxStack)
        {
            m_SkillStack++;
        }
    }

    void QSkill()
    {
        if (m_SkillStack == m_SkillMaxStack)
        {
            m_SkillEnable = true;
        }
        else
            m_SkillEnable = false;
    }

    void Parrying()
    {
        if (isParrying)
        {
            _Animator.SetBool("isParrying", true);
            Speed = ParryingSpeed;
        }
        else if (!isParrying && !isRun)
        {
            _Animator.SetBool("isParrying", false);
            Speed = MinSpeed;
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
        m_FinalAttack = false;
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
