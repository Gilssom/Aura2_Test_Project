using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BariController : MonoBehaviour
{
    private Animator _Animator;
    private Transform characterBody;
    private Rigidbody m_Rigid;
    private TestShot m_MissileSkill;
    private NearItemCheck m_NearItem;

    public Transform cameraArm;

    public Camera m_Camera;
    public Camera m_MazeCam;
    public Camera m_MazeQuaterCam;
    public GameObject m_MazeGround;

    public Transform m_MazeStartPos;

    public float Speed;
    public float MinSpeed;
    public float MaxSpeed;
    public float ParryingSpeed = 0.5f;
    public float JumpPower;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    float HAxis;
    float VAxis;
    [SerializeField]
    float Wheel;

    //public BoxCollider m_FirstArea;
    //public BoxCollider m_SecondArea;
    //public BoxCollider m_FinalArea;

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

    bool isGrounded;
    public LayerMask Ground;

    public float m_SkillNum;

    public bool m_ItemPickup;
    [SerializeField]
    GameObject scanObject;
    private Vector3 ObjPosition;

    public Light m_Light;
    public Light m_MazeLigth;

    public bool m_UseMazeKey;

    public GameObject m_TeleportEff;
    bool m_TportEnable;
    public float m_TportCount;
    public float m_TportAddTime;

    void Awake()
    {
        _Animator = GetComponent<Animator>();
        characterBody = GetComponent<Transform>();
        m_Rigid = GetComponent<Rigidbody>();
        m_MissileSkill = GetComponent<TestShot>();
        m_NearItem = GetComponent<NearItemCheck>();

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //m_FirstArea.enabled = false;
        //m_SecondArea.enabled = false;
        //m_FinalArea.enabled = false;
        m_FinalAttack = false;

        m_SkillEnable = false;

        m_SkillStack = 0;
        m_StackTime = 1;
        m_SkillNum = 1;
        m_TportCount = 3;
        m_TportAddTime = 5;
    }

    void Update()
    {
        GetInput();
        if (!m_FinalAttack && !DoSkill && !AT_GameManager.Instance.isAction && !isFading)
        {
            Move();
        }
        if (!AT_GameManager.Instance.isAction && !isMazePlay)
        {
            QSkill();
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
            StartCoroutine(MazeStart(0));
        else if(other.tag == "MazeFirstPortal")
            StartCoroutine(MazeStart(1));
        else if (other.tag == "MazeSecondPortal")
            StartCoroutine(MazeStart(2));
        else if(other.tag == "MazeBonusInPortal")
            StartCoroutine(MazeStart(3));
        else if (other.tag == "MazeBossPortal")
            StartCoroutine(MazeStart(4));
        else if (other.tag == "MazeBonusOutPortal")
            StartCoroutine(MazeStart(5));
        else if (other.tag == "MazeThirdPortal")
            StartCoroutine(MazeStart(6));
        else if (other.tag == "MazeFinalPortal")
        {
            if (m_UseMazeKey)
                StartCoroutine(MazeStart(7));
            else
                Debug.Log("ø≠ºË∞° « ø‰«’¥œ¥Ÿ.");
        }
        else if (other.tag == "MazeExitPortal")
        {
            isFading = true;
            AT_GameManager.Instance.InStartFadeAnim(2, true);
        }

        if (other.tag == "MazeDeathObj")
        {
            Debug.Log("Death");
            transform.position = m_MazeStartPos.transform.position;
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

        if (!AT_GameManager.Instance.isAction)
        {
            AT_GameManager.Instance.InStartFadeAnim(0.2f, false);
            yield return new WaitForSeconds(0.2f);
            AT_GameManager.Instance.OutStartFadeAnim(0.2f);
        }
        transform.LookAt(ObjPosition);
        scanObject.transform.LookAt(new Vector3(transform.position.x, scanObject.transform.position.y, transform.position.z));

        AT_GameManager.Instance.Action(scanObject);
        _Animator.SetBool("isRun", false);
        _Animator.SetBool("isWalk", false);
        yield return null;
    }

    IEnumerator MazeStart(int StageNum)
    {
        isFading = true;
        AT_GameManager.Instance.InStartFadeAnim(0.2f, false);
        AT_GameManager.Instance.isMazePlaying = true;
        yield return new WaitForSeconds(3);
        MazeManager.Instance.StageCtrl(StageNum);
        m_Camera.gameObject.SetActive(false);
        if (StageNum == 0 || StageNum == 5)
        {
            m_Light.gameObject.SetActive(false);
            m_MazeLigth.gameObject.SetActive(true);
            isMazePlay = true;
            m_MazeQuaterCam.gameObject.SetActive(false);
            m_MazeGround.gameObject.SetActive(true);
            m_MazeCam.gameObject.SetActive(true);
        }
        else if (StageNum == 2 || StageNum == 7)
        {
            m_MazeLigth.gameObject.SetActive(false);
            m_Light.gameObject.SetActive(true);
            isMazePlay = false;
            m_MazeCam.gameObject.SetActive(false);
            m_MazeGround.gameObject.SetActive(false);
            m_MazeQuaterCam.gameObject.SetActive(true);
        }
        transform.position = m_MazeStartPos.transform.position;
        AT_GameManager.Instance.OutStartFadeAnim(0.2f);
        isFading = false;
        yield return null;
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");

        if (HAxis == 0 && VAxis == 0)
        {
            _Animator.SetBool("isRun", false);
            Speed = MinSpeed;
        }

        //if (Input.GetMouseButton(1) && !isMazePlay)
        //{
        //    isParrying = true;
        //    Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit rayhit;
        //    if (Physics.Raycast(ray, out rayhit, 100))
        //    {
        //        Vector3 nextVec = rayhit.point - transform.position;
        //        nextVec.y = 0;
        //        transform.LookAt(transform.position + nextVec);
        //    }
        //}
        //else
        //    isParrying = false;

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

            if (Input.GetKeyDown(KeyCode.LeftShift) && m_TportEnable && !isMazePlay)
            {
                Vector3 Pos = this.transform.position;
                GameObject Teleport = Instantiate(m_TeleportEff, this.transform.forward +
                    new Vector3(Pos.x, Pos.y + 0.7f, Pos.z), this.transform.rotation);
                this.transform.DOMove(transform.position + transform.forward * 5, 0.2f);
                Destroy(Teleport, 2f);
                m_TportCount -= 1;
            }
        }
        else
            m_TportEnable = false;
    }

    void MouseWheel()
    {
        Wheel = Input.GetAxis("Mouse ScrollWheel");
        if (Wheel > 0 && m_SkillNum < 4)
        {
            m_SkillNum++;
        }
        else if (Wheel < 0 && m_SkillNum > 1)
        {
            m_SkillNum--;
        }
    }

    void Move()
    {
        //Vector2 moveInput = new Vector2(HAxis, VAxis);
        Vector3 moveVec = new Vector3(HAxis, 0, VAxis).normalized;

        //bool isMove = moveVec.magnitude != 0;

        transform.position += moveVec * Speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

        /*if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            if (!isParrying)
                characterBody.forward = moveDir;

        }*/

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
        else
        {
            isGrounded = false;
        }
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

                Debug.Log(Item.name + "¿ª »πµÊ«œø¥Ω¿¥œ¥Ÿ");
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

    void QSkill()       
    {
        if (m_SkillStack == m_SkillMaxStack)
        {
            m_SkillEnable = true;
            if (Input.GetKeyDown(KeyCode.Q) && !isAttack)
            {
                if (m_SkillNum == 1)
                {
                    StartCoroutine(BariSkillManager.Instance.WinterIceStun());
                }
                else if (m_SkillNum == 2)
                {
                    m_MissileSkill.MissileSkill();
                }
                else if(m_SkillNum == 3)
                {
                    StartCoroutine(BariSkillManager.Instance.SummerLaser());
                }
                else if(m_SkillNum == 4)
                {
                    StartCoroutine(BariSkillManager.Instance.SpringSkill());
                }
            }
        }
        else
            m_SkillEnable = false;
        return;
    }

    void SpringBuff()
    {
        if (DoSpringBuff)
        {
            Speed = MaxSpeed;
            _Animator.SetFloat("WalkSpeed", 3);
        }
        else
        {
            Speed = MinSpeed;
            _Animator.SetFloat("WalkSpeed", 2);
        }
    }
}
