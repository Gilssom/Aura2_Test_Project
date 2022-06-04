using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NearItemCheck : MonoBehaviour
{
    private ChoheeController m_Player;

    [SerializeField]
    public GameObject m_NearItem;

    public float m_ItemCheckDis; // �÷��̾�� �����۰��� �Ÿ��� üũ�ϴ� �ּ����� �Ÿ�

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
