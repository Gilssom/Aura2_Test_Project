using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.VFX;

[System.Serializable] //반드시 필요
public class AttackEffect //행에 해당되는 이름
{
    public VisualEffect[] m_Effect;
}

public class ChoheeController : MonoBehaviour
{
    private Animator m_Animator;
    public Rigidbody m_Rigid;
    private NearItemCheck m_NearItem;
    private NearNpcCheck m_NearNpc;
    private Camera m_Camera;
    [SerializeField]
    private ChoheeWeapon m_Weapon;

    public AudioSource m_FootSFX;
    public AudioClip[] m_clip;

    public float Speed;

    public int comboStep = 0;
    public bool isAttackReady = true;
    public bool ComboPossible;

    bool isDeath;

    Vector3 moveVec;
    float HAxis;
    float VAxis;
    float Wheel;

    /// <summary>
    /// 0 = Normal Effect
    /// 1 = Fire Effect
    /// 2 = Ice Effect
    /// </summary>
    public int m_WeaponCurType;
    public BoxCollider[] m_AreaType;

    bool m_FinalAttack;
    [SerializeField]
    bool isAttack;
    bool isCharge;
    bool isDodge;

    public bool isLoading;

    [SerializeField]
    bool isWall;
    public LayerMask Ground;
    public LayerMask Wall;

    public bool m_ItemPickup;
    public GameObject scanObject;

    public AttackEffect[] m_AttackEffect;
    public GameObject m_SlashEffect;
    public Transform m_SlashPos;
    public VisualEffect[] m_EnforceEffectVFX;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_NearItem = GetComponent<NearItemCheck>();
        m_NearNpc = GetComponent<NearNpcCheck>();
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Weapon = GetComponentInChildren<ChoheeWeapon>();

        DontDestroyOnLoad(this);
    }
    void Start()
    {
        m_AreaType[0].enabled = false; // First
        m_AreaType[1].enabled = false; // Second
        m_AreaType[2].enabled = false; // Final
        m_AreaType[3].enabled = false; // Charge
        m_AreaType[4].enabled = false; // Fire Weapon
        m_AreaType[5].enabled = false; // Ice Weapon
        m_FinalAttack = false;
        isLoading = true;
        Speed = 5;

        m_EnforceEffectVFX[0].Stop();
        m_EnforceEffectVFX[1].Stop();
    }

    void Update()
    {
        GetInput();

        if (PlayerStats.Instance.Health <= 0 && !AllGameManager.Instance.isGameOver)
            Death();

        if (!m_FinalAttack && !isDeath && !isCharge && !isLoading && !AllGameManager.Instance.isWeaponShop && !AllGameManager.Instance.isTalkAction)
            Move();

        if(!isDeath && !isLoading && !AllGameManager.Instance.isWeaponShop && !AllGameManager.Instance.isTalkAction)
        {
            Attack();
            ItemUse();
            NpcInteraction();
        }

        if (Input.GetButtonDown("Jump") && !isDodge && !isLoading && !AllGameManager.Instance.isWeaponShop && !AllGameManager.Instance.isTalkAction)
            Dodge();

        if(isLoading)
        {
            m_Animator.SetBool("isRun", false);
            m_FootSFX.Stop();
        }

        if (Input.GetKeyDown(KeyCode.K))
            PlayerStats.Instance.TakeDamage(1);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster" && !isAttack)
        {
            HitDamage();
        }

        if(other.tag == "FirstGate")
        {
            GameManager.Instance.ObjectCtrl(1, true);
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
        bool isMoving = false;

        moveVec = new Vector3(HAxis, 0, VAxis).normalized;

        transform.position += moveVec * Speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

        m_Animator.SetBool("isRun", moveVec != Vector3.zero);

        if (moveVec != Vector3.zero && !isDodge)
            isMoving = true;
        else
            isMoving = false;

        if (isMoving)
        {
            if (!m_FootSFX.isPlaying)
                m_FootSFX.Play();
        }
        else
            m_FootSFX.Stop();
    }

    void Dodge()
    {
        if (moveVec != Vector3.zero && !isDodge) // ! = bool 형태의 반대(=false)
        {
            Speed *= 2;
            m_Animator.SetTrigger("DoJump");
            SoundManager.Instance.SFXPlay("Charge Attack", m_clip[4]);
            isDodge = true;

            Invoke("DodgeOut", 0.7f); // 시간차 호출
        }
    }

    void DodgeOut()
    {
        Speed *= 0.5f;
        isDodge = false;
    }

    void HitDamage()
    {
        PlayerStats.Instance.TakeDamage(0.25f);
        m_Weapon.WeaponTypeChange("Hit");
        m_Animator.SetTrigger("DoHit");
        StartCoroutine(UIManager.Instance.BloodScreen(0.5f));
        m_Camera.transform.DOShakePosition(0.2f, 0.3f, 8, 90, false, true);        
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayhit;
                if (Physics.Raycast(ray, out rayhit, 100))
                {
                    Vector3 nextVec = rayhit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                m_FootSFX.Stop();
            }      
        }

        if (Input.GetMouseButtonDown(0) && comboStep == 0)
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                m_Animator.SetBool("isRun", false);

                m_Animator.SetTrigger("Attack");
                comboStep = 1;
                isAttackReady = false;
                m_FinalAttack = true;
                return;
            }
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

        if (Input.GetMouseButton(1) && !isAttack &&!isCharge && PlayerStats.Instance.Slash > 0)
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
            m_FootSFX.Stop();
            SoundManager.Instance.SFXPlay("Charge Attack", m_clip[3]);
            m_Animator.SetTrigger("DoSlashAttack");
            QuarterView.Instance.isZoomOut = true;
            m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * -2, 0.2f);
        }

        if (Input.GetMouseButtonUp(1) && isCharge)
        {
            GameObject intantBullet = Instantiate(m_SlashEffect, m_SlashPos.position, m_SlashPos.rotation);
            Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = m_SlashPos.forward * 20;

            m_Animator.SetTrigger("DoSlashStart");
            m_Animator.ResetTrigger("DoSlashAttack");
            SoundManager.Instance.SFXPlay("Final Attack", m_clip[2]);
            m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 2, 0.05f);

            isCharge = false;
            QuarterView.Instance.isZoomOut = false;
            PlayerStats.Instance.AddSlashGage(-1);
            Destroy(intantBullet, 1.5f);
        }

        if (Input.GetMouseButtonDown(2) && !isAttack && !isCharge)
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
            m_FootSFX.Stop();
            SoundManager.Instance.SFXPlay("Charge Attack", m_clip[3]);
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
        m_AreaType[0].enabled = true;
        m_AreaType[4].enabled = true; 
        m_AreaType[5].enabled = true;
        m_AttackEffect[m_WeaponCurType].m_Effect[0].Play();
        SoundManager.Instance.SFXPlay("FirstAttack", m_clip[0]);
        this.transform.DOMove(transform.position + transform.forward * 0.5f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.05f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void SecondAttack()
    {
        m_AreaType[1].enabled = true;
        m_AttackEffect[m_WeaponCurType].m_Effect[1].Play();
        SoundManager.Instance.SFXPlay("SecondAttack", m_clip[1]);
        this.transform.DOMove(transform.position + transform.forward * 0.7f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.15f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void FinalAttack()
    {
        m_AreaType[2].enabled = true;
        m_AttackEffect[m_WeaponCurType].m_Effect[2].Play();
        SoundManager.Instance.SFXPlay("Final Attack", m_clip[2]);
        this.transform.DOMove(transform.position + transform.forward * 0.8f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.3f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void ChargeAttack()
    {
        m_AreaType[3].enabled = true;
        m_AttackEffect[m_WeaponCurType].m_Effect[3].Play();
        SoundManager.Instance.SFXPlay("Final Attack", m_clip[2]);
        this.transform.DOMove(transform.position + transform.forward * 7f, 0.2f);
        m_Camera.transform.DOMove(m_Camera.transform.position + m_Camera.transform.forward * 0.5f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
    void AreaEnd()
    {
        isCharge = false;
        m_AreaType[0].enabled = false;
        m_AreaType[1].enabled = false;
        m_AreaType[2].enabled = false;
        m_AreaType[3].enabled = false;
        m_AreaType[4].enabled = false;
        m_AreaType[5].enabled = false;
    }

    void ItemUse()
    {
        var Item = m_NearItem.m_NearItem;

        if (Item)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_Weapon.WeaponTypeChange(Item.name);
                Destroy(Item);
            }
        }
    }

    void NpcInteraction()
    {
        var Npc = m_NearNpc.m_NearNpc;

        if(Npc)
        {
            if(Input.GetKeyDown(KeyCode.E) && Npc.GetComponent<ObjData>().isNeedTalk)
            {
                AllGameManager.Instance.Action();
            }
            else if (Input.GetKeyDown(KeyCode.E) && !Npc.GetComponent<ObjData>().isNeedTalk && Npc.GetComponent<ObjData>().isNotSmithy)
            {
                AllGameManager.Instance.Action();
            }
            else if(Input.GetKeyDown(KeyCode.E) && !AllGameManager.Instance.isWeaponShop && !Npc.GetComponent<ObjData>().isNeedTalk && !Npc.GetComponent<ObjData>().isNotSmithy)
            {
                AllGameManager.Instance.Action();
                UIManager.Instance.SmithySystem(true, Npc.name);
                AllGameManager.Instance.isWeaponShop = true;
            }
            //else if(Input.GetKeyDown(KeyCode.Escape) && AllGameManager.Instance.isWeaponShop)
            //{
            //    UIManager.Instance.SmithySystem(false, Npc.name);
            //    AllGameManager.Instance.isWeaponShop = false;
            //}
        }
    }

    public IEnumerator EnforceEff(int ColorNumber)
    {
        m_EnforceEffectVFX[ColorNumber].Play();
        yield return new WaitForSeconds(0.5f);
        m_EnforceEffectVFX[ColorNumber].Stop();
    }

    void Death()
    {
        //StopAllCoroutines();
        StartCoroutine(UIManager.Instance.DeathScreen());
        m_Animator.SetTrigger("DoDeath");
        isDeath = true;
    }
}
