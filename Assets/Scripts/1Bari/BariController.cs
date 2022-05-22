using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BariController : MonoBehaviour
{
    private Animator _Animator;
    private Rigidbody m_Rigid;
    private TestShot m_MissileSkill;
    private NearItemCheck m_NearItem;
    private Camera m_Camera;

    public AudioClip[] m_clip;

    public float Speed;
    public float MinSpeed;
    public float MaxSpeed;
    public float ParryingSpeed = 0.5f;
    public float JumpPower;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    bool isDeath;

    float HAxis;
    float VAxis;
    [SerializeField]
    float Wheel;

    //public BoxCollider m_FirstArea;
    //public BoxCollider m_SecondArea;
    //public BoxCollider m_FinalArea;
    public BoxCollider m_AttackArea;

    public float m_SkillStack;
    public float m_SkillMinStack = 0;
    public float m_SkillMaxStack = 3;
    public float m_StackTime;
    public bool m_SkillEnable;
    public bool DoSkill;
    public bool DoSpringBuff;

    public bool m_AttackStacking;

    bool m_FinalAttack;
    [SerializeField]
    bool isAttack;

    public bool isParrying;
    public bool isMazePlay;
    public bool isFading;

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

    public bool m_UseMazeKey;

    public GameObject m_TeleportEff;
    bool m_TportEnable;
    public float m_TportCount;
    public float m_TportAddTime;

    [SerializeField]
    float m_CurEnableSkill;

    bool m_WinterSkillacquire;
    bool m_FallingSkillacquire;
    bool m_SummerSkillacquire;
    bool m_SpringSkillacquire;

    public GameObject m_Tutorial;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_MissileSkill = GetComponent<TestShot>();
        m_NearItem = GetComponent<NearItemCheck>();
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_AttackArea.enabled = false;
        //m_SecondArea.enabled = false;
        //m_FinalArea.enabled = false;
        m_FinalAttack = false;

        m_SkillEnable = false;
        m_AttackStacking = true;

        m_SkillStack = 0;
        m_StackTime = 1;
        //m_SkillNum = 1;
        m_TportCount = 3;
        m_TportAddTime = 5;
    }

    void Update()
    {
        GetInput();
        if (!m_FinalAttack && !DoSkill && !AT_GameManager.Instance.isAction && !isFading && !isDeath)
        {
            Move();
        }
        if (!AT_GameManager.Instance.isAction && !isMazePlay)
        {
            Attack();
            QSkillAnim();
        }
        GroundCheck();
        ItemUse();
        HitDamage();
        MouseWheel();

        if (scanObject != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TalkStart());
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !AT_GameManager.Instance.isAction)
        {
            Jump();
        }

        SpringBuff();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Object")
        {
            scanObject = other.gameObject;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FirstPortal")
        {
            QuestManager.Instance.QuestId = 40;
            StartCoroutine(MazeManager.Instance.MazeStart(0));
        }
        else if (other.tag == "MazeFirstPortal")
            StartCoroutine(MazeManager.Instance.MazeStart(1));
        else if (other.tag == "MazeSecondPortal")
            StartCoroutine(MazeManager.Instance.MazeStart(2));
        else if (other.tag == "MazeBonusInPortal")
            StartCoroutine(MazeManager.Instance.MazeStart(3));
        else if (other.tag == "MazeBossPortal")
            StartCoroutine(MazeManager.Instance.MazeStart(4));
        else if (other.tag == "MazeBonusOutPortal")
            StartCoroutine(MazeManager.Instance.MazeStart(5));
        else if (other.tag == "MazeThirdPortal")
            StartCoroutine(MazeManager.Instance.MazeStart(6));
        else if (other.tag == "MazeFinalPortal")
        {
            if (m_UseMazeKey)
                StartCoroutine(MazeManager.Instance.MazeStart(7));
            else
                Debug.Log("¿­¼è°¡ ÇÊ¿äÇÕ´Ï´Ù.");
        }
        else if (other.tag == "MazeExitPortal")
        {
            isFading = true;
            AT_GameManager.Instance.InStartFadeAnim(2, true);
        }

        if (other.tag == "MazeDeathGround")
        {
            PlayerStats.Instance.TakeDamage(1);
            StartCoroutine(MazeManager.Instance.MazeDeath());
        }
        if(other.tag == "MazeDeathObj")
        {
            PlayerStats.Instance.TakeDamage(1);

            if (PlayerStats.Instance.Health <= 0)
            {
                StartCoroutine(MazeManager.Instance.MazeDeath_2());
            }
        }

        if(other.tag == "Tutorial")
        {
            m_Tutorial.SetActive(true);
            TutorialManager.Instance.InStartFadeAnim(0);
            Destroy(GameObject.Find("TutorialTrigger"));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            scanObject = null;
        }
    }

    IEnumerator TalkStart()
    {
        ObjPosition = new Vector3(scanObject.transform.position.x, transform.position.y, scanObject.transform.position.z);

        /*if (!AT_GameManager.Instance.isAction)
        {
            AT_GameManager.Instance.InStartFadeAnim(0.2f, false);
            yield return new WaitForSeconds(0.2f);
            AT_GameManager.Instance.OutStartFadeAnim(0.2f);
        }*/
        transform.LookAt(ObjPosition);

        if(scanObject.name == "LightNPC")
            scanObject.transform.LookAt(new Vector3(transform.position.x, scanObject.transform.position.y, transform.position.z));

        AT_GameManager.Instance.Action(scanObject);
        _Animator.SetBool("isRun", false);
        _Animator.SetBool("isWalk", false);
        ObjInteraction();
        yield return null;
    }

    void ObjInteraction()
    {
        if (scanObject.name == "LightNPC")
        {
            NPCMove.Instance.isTalking = true;
            NPCMove.Instance.DoTalking();
        }
        else if (scanObject.name == "SkillCryStal" && !AT_GameManager.Instance.isAction)
        {
            Debug.Log("¾ó¸®±â ½ºÅ³ È¹µæ");
            m_WinterSkillacquire = true;
            m_CurEnableSkill++;
            m_SkillNum++;
            Destroy(scanObject.gameObject);
        }
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");

        /*if (HAxis == 0 && VAxis == 0)
        {
            _Animator.SetBool("isRun", false);
            Speed = MinSpeed;
        }/*

        /*if (Input.GetMouseButton(1) && !isMazePlay)
        {
            isParrying = true;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextVec = rayhit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        else
            isParrying = false;*/

        if(m_TportCount < 3 && m_TportAddTime >= 0)
        {
            m_TportAddTime -= Time.deltaTime;

            if (m_TportAddTime <= 0)
            {
                m_TportAddTime = 5;

                if (m_TportAddTime == 5 && m_TportCount < 3)
                    m_TportCount += 1;
            }
        }

        if(m_TportCount > 0)
        {
            m_TportEnable = true;

            if (Input.GetKeyDown(KeyCode.LeftShift) && m_TportEnable && !isMazePlay && !isWall)
            {
                Vector3 Pos = this.transform.position;
                GameObject Teleport = Instantiate(m_TeleportEff, this.transform.forward +
                    new Vector3(Pos.x, Pos.y + 0.7f, Pos.z), this.transform.rotation);
                SoundManager.Instance.SFXPlay("Teleport", m_clip[0]);
                this.transform.DOMove(transform.position + transform.forward * 5, 0.2f);
                Destroy(Teleport, 2f);
                m_TportCount -= 1;
            }
        }
        else
            m_TportEnable = false;
    }

    void Move()
    {
        Vector3 moveVec = new Vector3(HAxis, 0, VAxis).normalized;

        transform.position += moveVec * Speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

        _Animator.SetBool("isWalk", moveVec != Vector3.zero);
    }

    void Jump()
    {
        m_Rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
    }

    void GroundCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f, Ground))
        {
            isGrounded = true;
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 4.5f, Wall))
        {
            isWall = true;
        }
        else
        {
            isGrounded = false;
            isWall = false;
        }
    }

    void Attack()
    {
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

        if (Input.GetMouseButtonDown(0) && comboStep == 0)
        {
            _Animator.SetBool("isWalk", false);
            _Animator.SetBool("isRun", false);

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
            _Animator.SetTrigger("SecondAttack");
        }
    }

    void ComboReset()
    {
        //_Animator.ResetTrigger("SecondAttack");
        ComboPossible = false;
        isAttackReady = true;
        m_FinalAttack = false;
        isAttack = false;
        comboStep = 0;
    }

    void FirstAttack()
    {
        m_AttackArea.enabled = true;
    }
    void SecondAttack()
    {
        m_AttackArea.enabled = true;
    }
    void AreaEnd()
    {
        m_AttackArea.enabled = false;
        m_AttackArea.enabled = false;
    }

    void ItemUse()
    {
        var Item = m_NearItem.m_NearItem;

        if (Item)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Item.name == "Pitch")            
                    PlayerStats.Instance.Heal(1);              
                else if (Item.name == "Flower")              
                    PlayerStats.Instance.AddHealth();              
                else if (Item.name == "MazeKey")
                    m_UseMazeKey = true;

                Debug.Log(Item.name + "À» È¹µæÇÏ¿´½À´Ï´Ù");
                Destroy(Item);
            }
        }
    }

    void HitDamage()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayerStats.Instance.TakeDamage(0.5f);
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

    void MouseWheel()
    {
        Wheel = Input.GetAxis("Mouse ScrollWheel");
        if (Wheel > 0 && m_SkillNum < m_CurEnableSkill)
        {
            m_SkillNum++;
        }
        else if (Wheel < 0 && m_SkillNum > 1)
        {
            m_SkillNum--;
        }
    }

    public void SkillChange()
    {
        if (m_SkillNum == 1)
        {
            m_SkillNum = 2;
        }
        else if (m_SkillNum == 2)
        {
            m_SkillNum = 1;
        }
    }

    void QSkillAnim()
    {
        if (m_SkillStack == m_SkillMaxStack)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isAttack)
            {
                DoSkill = true;
                m_SkillEnable = true;
                m_SkillStack = m_SkillMinStack;
                _Animator.SetBool("isWalk", false);
                _Animator.SetBool("isRun", false);
                _Animator.SetTrigger("DoSkill");
            }
        }
        else
            m_SkillEnable = false;
        return;
    }

    void QSkill()       
    {
        if (m_SkillNum == 1 && m_WinterSkillacquire)
        {
            StartCoroutine(BariSkillManager.Instance.WinterIceStun());
            SoundManager.Instance.SFXPlay("WinterSkill", m_clip[1]);
        }
        else if (m_SkillNum == 2 && m_FallingSkillacquire)
        {
            m_MissileSkill.MissileSkill();
            SoundManager.Instance.SFXPlay("FallingSkill", m_clip[2]);
        }
        else if (m_SkillNum == 3 && m_SummerSkillacquire)
        {
            StartCoroutine(BariSkillManager.Instance.SummerLaser());
            SoundManager.Instance.SFXPlay("SummerSkill", m_clip[3]);
        }
        else if (m_SkillNum == 4 && m_SpringSkillacquire)
        {
            StartCoroutine(BariSkillManager.Instance.SpringSkill());
            SoundManager.Instance.SFXPlay("SpringSkill", m_clip[4]);
        }
        else
            Debug.Log("½ºÅ³À» ¾ÆÁ÷ È¹µæÇÏÁö ¸øÇß½À´Ï´Ù.");       
        
    }

    void SpringBuff()
    {
        if (DoSpringBuff)
        {
            Speed = MaxSpeed;
            //_Animator.SetFloat("WalkSpeed", 3);
            _Animator.SetBool("isRun", true);
        }
        else
        {
            Speed = MinSpeed;
            //_Animator.SetFloat("WalkSpeed", 2);
            _Animator.SetBool("isRun", false);
        }
    }

    public void Death()
    {
        isDeath = true;
    }
}
