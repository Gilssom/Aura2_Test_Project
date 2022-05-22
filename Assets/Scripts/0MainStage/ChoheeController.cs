using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class ChoheeController : MonoBehaviour
{
    private Animator m_Animator;
    private Rigidbody m_Rigid;
    private NearItemCheck m_NearItem;
    private Camera m_Camera;

    public AudioClip[] m_clip;

    public float Speed;
    public float MinSpeed;
    public float MaxSpeed;
    public float JumpPower;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    bool isDeath;

    float HAxis;
    float VAxis;
    [SerializeField]
    float Wheel;

    public BoxCollider m_FirstArea;
    public BoxCollider m_SecondArea;
    public BoxCollider m_FinalArea;
    public BoxCollider m_ChargeArea;

    bool m_FinalAttack;
    [SerializeField]
    bool isAttack;
    bool isCharge;

    [SerializeField]
    bool isGrounded;
    [SerializeField]
    bool isWall;
    public LayerMask Ground;
    public LayerMask Wall;

    public float m_SkillNum;

    public bool m_ItemPickup;
    public GameObject scanObject;
    private Vector3 ObjPosition;

    public VisualEffect m_FirstSlash;
    public VisualEffect m_SecondSlash;
    public VisualEffect m_FinalSlash;
    public VisualEffect m_ChargeSlash;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_NearItem = GetComponent<NearItemCheck>();
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        DontDestroyOnLoad(this);
    }
    void Start()
    {
        m_FirstArea.enabled = false;
        m_SecondArea.enabled = false;
        m_FinalArea.enabled = false;
        m_FinalAttack = false;
    }

    void Update()
    {
        GetInput();
        if (!m_FinalAttack && !isDeath && !isCharge)
        {
            Move();
        }
        Attack();
        GroundCheck();
        HitDamage();

        //if (scanObject != null && Input.GetKeyDown(KeyCode.E))
        //{
        //    StartCoroutine(TalkStart());
        //}

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            scanObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            scanObject = null;
        }
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");
    }

    void Move()
    {
        Vector3 moveVec = new Vector3(HAxis, 0, VAxis).normalized;

        transform.position += moveVec * Speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

        m_Animator.SetBool("isRun", moveVec != Vector3.zero);
    }

    void Jump()
    {
        m_Rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        m_Animator.SetTrigger("DoJump");
    }

    void GroundCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.green);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, Ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void HitDamage()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            m_Animator.SetTrigger("DoHit");
            m_Camera.transform.DOShakePosition(0.2f, 0.3f, 8, 90, false, true);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextVec = rayhit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }

        if (Input.GetMouseButtonDown(0) && comboStep == 0)
        {
            m_Animator.SetBool("isRun", false);

            m_Animator.SetTrigger("Attack");
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

        if (Input.GetMouseButton(1) && !isAttack)
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextVec = rayhit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }

            isCharge = true;
            m_Animator.SetTrigger("DoSlashAttack");
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_Animator.SetTrigger("DoSlashStart");
            m_Animator.ResetTrigger("DoSlashAttack");
        }

        if (Input.GetMouseButtonDown(2) && !isAttack)
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextVec = rayhit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }

            isCharge = true;
            m_Animator.SetTrigger("DoChargeAttack");
        }
    }

    void AttackCheck()
    {
        isAttack = true;
    }

    void Attackfalse()
    {
        isAttack = false;
    }

    void Combopossible()
    {
        ComboPossible = true;
    }
    void Combo()
    {
        if (comboStep == 2)
        {
            m_Animator.SetTrigger("SecondAttack");
        }

        else if (comboStep == 3)
        {
            m_Animator.SetTrigger("FinalAttack");
        }
    }

    void ComboReset()
    {
        m_Animator.ResetTrigger("SecondAttack");
        m_Animator.ResetTrigger("FinalAttack");
        ComboPossible = false;
        isAttackReady = true;
        m_FinalAttack = false;
        isAttack = false;
        comboStep = 0;
    }

    void FirstAttack()
    {
        m_FirstArea.enabled = true;
        m_FirstSlash.Play();
        SoundManager.Instance.SFXPlay("FirstAttack", m_clip[0]);
        this.transform.DOMove(transform.position + transform.forward * 0.5f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.05f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void SecondAttack()
    {
        m_SecondArea.enabled = true;
        m_SecondSlash.Play();
        SoundManager.Instance.SFXPlay("SecondAttack", m_clip[1]);
        this.transform.DOMove(transform.position + transform.forward * 0.7f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.15f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void FinalAttack()
    {
        m_FinalArea.enabled = true;
        m_FinalSlash.Play();
        SoundManager.Instance.SFXPlay("Final Attack", m_clip[2]);
        this.transform.DOMove(transform.position + transform.forward * 0.8f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.3f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void ChargeAttack()
    {
        m_ChargeArea.enabled = true;
        m_ChargeSlash.Play();
        SoundManager.Instance.SFXPlay("Charge Attack", m_clip[3]);
        this.transform.DOMove(transform.position + transform.forward * 7f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.5f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void AreaEnd()
    {
        isCharge = false;
        m_FirstArea.enabled = false;
        m_SecondArea.enabled = false;
        m_FinalArea.enabled = false;
        m_ChargeArea.enabled = false;
    }
}
