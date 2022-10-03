using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NearItemCheck : MonoBehaviour
{
    private ChoheeController m_Player;

    [SerializeField]
    public GameObject m_NearItem;

    public float m_ItemCheckDis; // 플레이어와 아이템과의 거리를 체크하는 최소한의 거리

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

        m_NearItem = neareastObject;

        return neareastObject;
    }

    private void Awake()
    {
        m_Player = GetComponent<ChoheeController>();
    }

    void Update()
    {
        FindNearObjByTag("Item");

        if (m_NearItem)
        {
            m_Player.m_ItemPickup = true;

            float Dis = Vector3.Distance(m_NearItem.transform.position, transform.position);

            if (Dis >= m_ItemCheckDis)
            {
                m_NearItem = null;
                m_Player.m_ItemPickup = false;
            }
        }
    }
}
