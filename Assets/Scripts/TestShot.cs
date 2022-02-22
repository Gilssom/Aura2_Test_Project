using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestShot : MonoBehaviour
{
    private TestSceneMonsterText m_Monster;

    public GameObject _MissilePrefab; // 미사일
    public GameObject _SkillMissile; // 스킬 미사일
    public GameObject _Target; // 타겟

    public float m_Speed = 2; // 미사일속도
    public float m_distanceStart = 8f; // 시작지점 기준으로 꺾이는 정도
    public float m_distanceEnd = 3f; // 도착지점 기준으로 꺾이는 정도

    public float m_SkillSpeed; // 미사일속도
    public float m_SkilldistanceStart; // 시작지점 기준으로 꺾이는 정도
    public float m_SkilldistanceEnd; // 도착지점 기준으로 꺾이는 정도

    public float m_EnemyCheckDis; // 플레이어와 적과의 거리를 체크하는 최소한의 거리

    public int m_shotCount = 12; // 총 미사일 갯수
    public float m_interval = 0.7f; // 미사일 발사 주기
    public float m_intervalReset = 0.7f; // 미사일 발사 주기 초기값
    public int m_shotCountAllinterval = 3; // 한번에 발사할 미사일 갯수

    bool m_DoShot = false;
    [SerializeField]
    bool m_RSkill = false;

    private GameObject FindNearObjByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        _Target = neareastObject;

        return neareastObject;
    }

    private void Start()
    {
        m_Monster = GameObject.FindWithTag("Monster").GetComponent<TestSceneMonsterText>();
    }

    void Update()
    {
        FindNearObjByTag("Monster");

        float Dis = Vector3.Distance(_Target.transform.position, transform.position);

        if (Dis >= m_EnemyCheckDis && !m_DoShot)
        {
            _Target = null;
        }
        else
        {
            _Target.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.05f);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_Target)
            {
                StartCoroutine(CreateMissile());
            }
            else
                return;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if (_Target && !m_RSkill)
            {
                m_RSkill = true;
                StartCoroutine(SkillMissile());
            }
            else
                return;
        }

        if(m_DoShot && m_interval > 0.1f)
        {
            m_interval -= Time.deltaTime * 0.35f;
        }
    }

    // 총 12발 발사 
    IEnumerator CreateMissile()
    {
        m_DoShot = true;
        int ShotCount = m_shotCount;
        while(ShotCount > 0)
        {
            for (int i = 0; i < m_shotCountAllinterval; i++)
            {
                if(ShotCount > 0)
                {
                    GameObject missile = Instantiate(_MissilePrefab);
                    missile.GetComponent<BazierMissile>().Init(this.gameObject.transform, _Target.transform, m_Speed, m_distanceStart, m_distanceEnd);

                    ShotCount--;
                }
            }
            yield return new WaitForSeconds(m_interval);
        }
        yield return null;
        m_DoShot = false;
        m_interval = m_intervalReset;
    }

    IEnumerator SkillMissile()
    {
        GameObject missile = Instantiate(_SkillMissile);
        missile.GetComponent<BazierMissile>().Init(this.gameObject.transform, _Target.transform, m_SkillSpeed, m_SkilldistanceStart, m_SkilldistanceEnd);
        yield return new WaitForSeconds(5f);
        m_RSkill = false;
        yield return null;
    }
}
