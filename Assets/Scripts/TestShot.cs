using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShot : MonoBehaviour
{
    public GameObject _MissilePrefab; // �̻���
    public GameObject _Target; // Ÿ��

    public float m_Speed = 2; // �̻��ϼӵ�
    public float m_distanceStart = 6f; // �������� �������� ���̴� ����
    public float m_distanceEnd = 3f; // �������� �������� ���̴� ����

    public int m_shotCount = 6; // �� �̻��� ����
    public float m_interval = 0.15f; // �̻��� �߻� �ֱ�
    public int m_shotCountAllinterval = 2; // �ѹ��� �߻��� �̻��� ����

    public Collider[] colls;
    public float nearRadius;

    void Update()
    {
        NearCollCheck();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_Target)
            {
                StartCoroutine(CreateMissile());
            }
            else
                return;
        }
    }

    IEnumerator CreateMissile()
    {
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
    }

    void NearCollCheck()
    {
        int layerID = 10;

        colls = Physics.OverlapSphere(this.transform.position, nearRadius, layerID);
    }
}
