using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestShot : MonoBehaviour
{
    public GameObject _MissilePrefab; // �̻���
    public GameObject _Target; // Ÿ��

    public float m_Speed = 2; // �̻��ϼӵ�
    public float m_distanceStart = 6f; // �������� �������� ���̴� ����
    public float m_distanceEnd = 3f; // �������� �������� ���̴� ����

    public float m_EnemyCheckDis; // �÷��̾�� ������ �Ÿ��� üũ�ϴ� �ּ����� �Ÿ�

    public int m_shotCount = 6; // �� �̻��� ����
    public float m_interval; // �̻��� �߻� �ֱ�
    public float m_intervalReset; // �̻��� �߻� �ֱ� �ʱⰪ
    public int m_shotCountAllinterval = 2; // �ѹ��� �߻��� �̻��� ����

    public bool m_DoShot = false;

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

    void Update()
    {
        FindNearObjByTag("Monster");

        float Dis = Vector3.Distance(_Target.transform.position, transform.position);

        if(Dis >= m_EnemyCheckDis)
        {
            _Target = null;
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
}
