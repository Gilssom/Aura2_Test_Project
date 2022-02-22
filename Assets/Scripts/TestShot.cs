using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestShot : MonoBehaviour
{
    private TestSceneMonsterText m_Monster;

    public GameObject _MissilePrefab; // �̻���
    public GameObject _SkillMissile; // ��ų �̻���
    public GameObject _Target; // Ÿ��

    public float m_Speed = 2; // �̻��ϼӵ�
    public float m_distanceStart = 8f; // �������� �������� ���̴� ����
    public float m_distanceEnd = 3f; // �������� �������� ���̴� ����

    public float m_SkillSpeed; // �̻��ϼӵ�
    public float m_SkilldistanceStart; // �������� �������� ���̴� ����
    public float m_SkilldistanceEnd; // �������� �������� ���̴� ����

    public float m_EnemyCheckDis; // �÷��̾�� ������ �Ÿ��� üũ�ϴ� �ּ����� �Ÿ�

    public int m_shotCount = 12; // �� �̻��� ����
    public float m_interval = 0.7f; // �̻��� �߻� �ֱ�
    public float m_intervalReset = 0.7f; // �̻��� �߻� �ֱ� �ʱⰪ
    public int m_shotCountAllinterval = 3; // �ѹ��� �߻��� �̻��� ����

    bool m_DoShot = false;
    [SerializeField]
    bool m_RSkill = false;

    private GameObject FindNearObjByTag(string tag)
    {
        // Ž���� ������Ʈ ����� List �� �����մϴ�.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
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

    // �� 12�� �߻� 
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
