using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TestMonster : MonoBehaviour
{
    public enum EnemyType { HellGhost, FireMonster, LanternMonster }
    public EnemyType m_EnemyType;

    public enum CurState { idle, trace, attack, death}
    public CurState m_CurState = CurState.idle;

    public bool isSpawnMonster;
    [SerializeField]
    private int m_HaveMaskNum;

    private ChoheeController m_Player;
    private NavMeshAgent m_NavAgent;
    private Rigidbody m_Rigid;
    private Animator m_Anim;
    private SkinnedMeshRenderer m_SkinmeshRenderer;
    private BoxCollider m_AttackArea;
    public Transform m_FirePos;
    public AudioClip[] m_clip;

    private float MaxCutoff = 1;
    private float Cutoff = default;
    private float Speed = 0.005f;
    [SerializeField]
    float m_TraceDis = default;
    float m_AttackDis = default;
    float m_rotSpeed = default;
    float m_moveSpeed = default;

    bool isDeath;
    bool isAttack;

    [SerializeField]
    private float m_MaxHP;
    [SerializeField]
    private float m_CurHP;

    public GameObject m_FireEffect;
    /// <summary>
    /// 1 = Normal
    /// 2 = Speed
    /// 3 = Fire
    /// 4 = Ice
    /// </summary>
    /// 
    private int m_MinDrop;
    private int m_MaxDrop;
    Vector3 curPos;

    [SerializeField]
    private GameObject[] m_DropMask;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigid = GetComponent<Rigidbody>();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();
        m_SkinmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if(m_EnemyType != EnemyType.LanternMonster)
            m_AttackArea = GetComponentInChildren<BoxCollider>();

        m_DropMask[0] = null;
        m_DropMask[1] = Resources.Load<GameObject>("5Item/NormalMask_Item");
        m_DropMask[2] = Resources.Load<GameObject>("5Item/SpeedMask_Item");
        m_DropMask[3] = Resources.Load<GameObject>("5Item/FireMask_Item");
        m_DropMask[4] = Resources.Load<GameObject>("5Item/IceMask_Item");
    }

    private void Start()
    {
        switch (m_EnemyType)
        {
            case EnemyType.HellGhost:
                if (isSpawnMonster)
                    m_TraceDis = 100;
                else
                    m_TraceDis = 10;

                m_AttackDis = 3;
                m_rotSpeed = 3;
                m_moveSpeed = 2;
                m_MaxHP = 600;

                m_MinDrop = 1;
                m_MaxDrop = 3;
                break;
            case EnemyType.FireMonster:
                if (isSpawnMonster)
                    m_TraceDis = 100;
                else                
                    m_TraceDis = 7;

                m_AttackDis = 2;
                m_rotSpeed = 3;
                m_moveSpeed = 3;
                m_MaxHP = 200;

                m_MinDrop = 1;
                m_MaxDrop = 3;
                break;
            case EnemyType.LanternMonster:
                if (isSpawnMonster)
                    m_TraceDis = 100;
                else
                    m_TraceDis = 10;

                m_AttackDis = 8;
                m_rotSpeed = 3;
                m_moveSpeed = 3;
                m_MaxHP = 200;

                m_MinDrop = 2;
                m_MaxDrop = 4;
                break;
        }
        if(m_AttackArea)
            m_AttackArea.enabled = false;

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
        NameCheck();

        m_CurHP = m_MaxHP;
    }

    void NameCheck()
    {
        switch (m_EnemyType)
        {
            case EnemyType.HellGhost:
                if (gameObject.name == "GhostMonster_NormalMask")
                    m_HaveMaskNum = 1;
                else if (gameObject.name == "GhostMonster_SpeedMask")
                {
                    m_HaveMaskNum = 2;
                    m_MaxHP = 750;
                }
                else if (gameObject.name == "GhostMonster_FireMask")
                {
                    m_HaveMaskNum = 3;
                    m_MaxHP = 1000;
                }
                else if (gameObject.name == "GhostMonster_IceMask")
                    m_HaveMaskNum = 4;
                else
                    m_HaveMaskNum = 0;
                break;
            case EnemyType.FireMonster:
                if (gameObject.name == "FireMonster_NormalMask")
                    m_HaveMaskNum = 1;
                else if (gameObject.name == "FireMonster_SpeedMask")
                {
                    m_HaveMaskNum = 2;
                    m_MaxHP = 300;
                }                 
                else if (gameObject.name == "FireMonster_FireMask")
                {
                    m_HaveMaskNum = 3;
                    m_MaxHP = 400;
                }                  
                else if (gameObject.name == "FireMonster_IceMask")
                    m_HaveMaskNum = 4;
                else
                    m_HaveMaskNum = 0;
                break;
            case EnemyType.LanternMonster:
                if (gameObject.name == "LanternMonster_NormalMask")
                    m_HaveMaskNum = 1;
                else if (gameObject.name == "LanternMonster_SpeedMask")
                {
                    m_HaveMaskNum = 2;
                    m_MaxHP = 300;
                }
                else if (gameObject.name == "LanternMonster_FireMask")
                {
                    m_HaveMaskNum = 3;
                    m_MaxHP = 400;
                }
                else if (gameObject.name == "LanternMonster_IceMask")
                    m_HaveMaskNum = 4;
                else
                    m_HaveMaskNum = 0;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            WeaponHitEff();
            this.transform.DOMove(transform.position + transform.forward * -0.5f, 0.2f);
            m_CurHP -= ChoheeWeapon.Instance.m_CurDmg;

            if (PlayerStats.Instance.Slash >= PlayerStats.Instance.MaxSlash)
            {
                return;
            }
            if (PlayerStats.Instance.Slash < PlayerStats.Instance.MaxSlash)
            {
                PlayerStats.Instance.AddSlashGage(1);
            }
        }
        else if (other.tag == "ChargeWeapon")
        {
            WeaponHitEff();
            this.transform.DOMove(transform.position + transform.forward * -0.5f, 0.2f);
            m_CurHP -= ChoheeWeapon.Instance.m_ChargeDmg;

            if (PlayerStats.Instance.Slash >= PlayerStats.Instance.MaxSlash)
            {
                return;
            }
            if (PlayerStats.Instance.Slash < PlayerStats.Instance.MaxSlash)
            {
                PlayerStats.Instance.AddSlashGage(1);
            }
        }
        else if (other.tag == "SlashWeapon")
        {
            WeaponHitEff();
            this.transform.DOMove(transform.position + transform.forward * -0.5f, 0.2f);
            m_CurHP -= ChoheeWeapon.Instance.m_SlashDmg;
        }
    }

    private void FixedUpdate()
    {
        if (m_CurHP <= 0)
        {
            m_Anim.SetBool("isAttack", false);
            m_Anim.SetTrigger("Death");
            Death();
        }
    }

    void Update()
    {
        if (m_moveSpeed < 3)
            m_moveSpeed += Time.deltaTime;
    }

    void LookPlayer()
    {
        Vector3 dir = m_Player.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * m_rotSpeed);
    }

    IEnumerator CheckState()
    {
        while(!isDeath)
        {
            yield return new WaitForSeconds(0.3f);

            float Dis = Vector3.Distance(m_Player.transform.position, this.transform.position);
            if (Dis <= m_AttackDis)
                m_CurState = CurState.attack;
            else if (Dis <= m_TraceDis)
                m_CurState = CurState.trace;
            else if(m_TraceDis < Dis)
                m_CurState = CurState.idle;
        }
    }

    IEnumerator CheckStateForAction()
    {
        while(!isDeath)
        {
            switch (m_CurState)
            {
                case CurState.idle:
                    if (m_EnemyType != EnemyType.FireMonster)
                        m_Anim.SetBool("isRun", false);
                    m_Anim.SetBool("isAttack", false);

                    while (m_CurState == CurState.idle && !isSpawnMonster && m_EnemyType == EnemyType.FireMonster)
                    {
                        Vector3 curPos = this.transform.position;
                        float dir1 = Random.Range(-0.3f, 0.3f);
                        float dir2 = Random.Range(-0.3f, 0.3f);
                        float ranTime = Random.Range(0, 3);

                        yield return new WaitForSeconds(ranTime);                   
                        this.transform.DOMove(new Vector3(curPos.x + dir1, curPos.y, curPos.z + dir2), 1);
                    }
                    break;
                case CurState.trace:
                    m_NavAgent.speed = m_moveSpeed;
                    m_Rigid.isKinematic = true;
                    m_NavAgent.destination = m_Player.transform.position;
                    m_NavAgent.Resume();
                    LookPlayer();
                    m_Anim.SetBool("isAttack", false);
                    if (m_EnemyType != EnemyType.FireMonster)
                        m_Anim.SetBool("isRun", true);
                    break;
                case CurState.attack:
                    isAttack = true;
                    m_NavAgent.speed = 0;
                    m_Rigid.isKinematic = false;
                    LookPlayer();
                    if (m_EnemyType != EnemyType.FireMonster)
                        m_Anim.SetBool("isRun", false);
                    m_Anim.SetBool("isAttack", true);
                    break;
                case CurState.death:
                    break;
                default:
                    break;
            }

            yield return null;
        }
    }

    void Attack()
    {
        switch (m_EnemyType)
        {
            case EnemyType.HellGhost:
                m_AttackArea.enabled = true;
                SoundManager.Instance.SFXPlay("Ghost Attack", AllGameManager.Instance.m_Clip[5]);
                break;
            case EnemyType.FireMonster:
                m_AttackArea.enabled = true;
                SoundManager.Instance.SFXPlay("Fire Attack", AllGameManager.Instance.m_Clip[6]);
                break;
            case EnemyType.LanternMonster:
                SoundManager.Instance.SFXPlay("Fire Attack", AllGameManager.Instance.m_Clip[7]);
                FireBallAttack();
                break;
        }

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
    
    void FireBallAttack()
    {
        GameObject FireBall = ObjectPoolManager.instance.m_ObjectPoolList[8].Dequeue();
        FireBall.SetActive(true);

        FireBall.transform.position = m_FirePos.position;
        FireBall.transform.rotation = m_FirePos.rotation;
        Rigidbody BallRigid = FireBall.GetComponent<Rigidbody>();
        BallRigid.velocity = m_FirePos.forward * 13;
    }

    void AttackEnd()
    {
        if (m_AttackArea)
            m_AttackArea.enabled = false;
    }

    void AttackFalse()
    {
        isAttack = false;
    }

    void WeaponHitEff()
    {
        if(m_EnemyType != EnemyType.FireMonster)
            m_Anim.SetTrigger("DoHurt");
        Vector3 Pos = this.transform.position;

        GameObject HitEffect = ObjectPoolManager.instance.m_ObjectPoolList[1].Dequeue();
        HitEffect.transform.position = new Vector3(Pos.x, Pos.y + 1f, Pos.z);
        HitEffect.transform.rotation = this.transform.rotation;
        HitEffect.SetActive(true);

        ObjectPoolManager.instance.StartCoroutine(ObjectPoolManager.instance.DestroyObj(1, 1, HitEffect));

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());

    }

    void Death()
    {
        isDeath = true;
        StopAllCoroutines();
        m_NavAgent.enabled = false;
        m_Rigid.isKinematic = true;

        if (m_FireEffect)
            m_FireEffect.SetActive(false);

        switch (m_EnemyType)
        {
            case EnemyType.HellGhost:
                CapsuleCollider Ghostcoll = GetComponent<CapsuleCollider>();

                Ghostcoll.enabled = false;
                m_AttackArea.enabled = false;

                Cutoff += Speed;
                if (Cutoff != MaxCutoff)
                {
                    m_SkinmeshRenderer.material.SetFloat("_Dissolve", Cutoff);
                }
                break;

            case EnemyType.FireMonster:
                if(isDeath)
                {
                    SphereCollider Firecoll = GetComponent<SphereCollider>();

                    Firecoll.enabled = false;
                    m_AttackArea.enabled = false;

                    Cutoff += Speed;
                    if (Cutoff != MaxCutoff)
                    {
                        m_SkinmeshRenderer.material.SetFloat("_Dissolve", Cutoff);
                    }
                }
                break;
            case EnemyType.LanternMonster:
                if (isDeath)
                {
                    CapsuleCollider Lanterncoll = GetComponent<CapsuleCollider>();
                    Lanterncoll.enabled = false;

                    Cutoff += Speed;
                    if (Cutoff != MaxCutoff)
                    {
                        m_SkinmeshRenderer.material.SetFloat("_Dissolve", Cutoff);
                    }
                }
                break;
            default:
                break;
        }

        DeathEvent();
    }

    void DeathEvent()
    {
        curPos = this.transform.position;

        if (Cutoff >= MaxCutoff)
        {
            Destroy(this.gameObject);
            GameManager.Instance.m_KillCount += 1;

            int ItemCount = Random.Range(m_MinDrop, m_MaxDrop);
            for (int i = 0; i < ItemCount; i++)
            {
                float RandomTF = Random.Range(0.5f, 3);
                GameObject SoulItem = ObjectPoolManager.instance.m_ObjectPoolList[6].Dequeue();
                SoulItem.SetActive(true);
                SoulItem.transform.position = new Vector3(curPos.x + RandomTF, curPos.y + 0.5f, curPos.z + RandomTF);
                SoulItem.transform.rotation = Quaternion.identity;
            }

            int HealthDrop = Random.Range(0, 100);
            if (HealthDrop <= 10 && PlayerStats.Instance.Health < PlayerStats.Instance.MaxHealth)
            {
                GameObject HealItem = ObjectPoolManager.instance.m_ObjectPoolList[7].Dequeue();
                HealItem.SetActive(true);
                HealItem.transform.position = new Vector3(curPos.x, curPos.y + 0.5f, curPos.z);
                HealItem.transform.rotation = Quaternion.identity;
            }

            if (m_HaveMaskNum != 0)
            {
                string ItemName = m_DropMask[m_HaveMaskNum].name;

                GameObject Mask = Instantiate(m_DropMask[m_HaveMaskNum],
                    new Vector3(curPos.x, curPos.y + 0.2f, curPos.z), Quaternion.Euler(0, 0, 180));
                Mask.name = ItemName;
            }
        }
    }
}
