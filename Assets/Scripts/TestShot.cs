using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShot : MonoBehaviour
{
    public GameObject _MissilePrefab; // 미사일
    public GameObject _Target; // 타겟

    public float m_Speed = 2; // 미사일속도
    public float m_distanceStart = 6f; // 시작지점 기준으로 꺾이는 정도
    public float m_distanceEnd = 3f; // 도착지점 기준으로 꺾이는 정도

    public int m_shotCount = 6; // 총 미사일 갯수
    public float m_interval = 0.15f; // 미사일 발사 주기
    public int m_shotCountAllinterval = 2; // 한번에 발사할 미사일 갯수

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
