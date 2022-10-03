using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NearNpcCheck : MonoBehaviour
{
    private ChoheeController m_Player;

    [SerializeField]
    public GameObject m_NearNpc;

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

        m_NearNpc = neareastObject;

        return neareastObject;
    }

    private void Awake()
    {
        m_Player = GetComponent<ChoheeController>();
    }

    void Update()
    {
        FindNearObjByTag("Npc");

        if (m_NearNpc)
        {
            m_Player.m_ItemPickup = true;

            float Dis = Vector3.Distance(m_NearNpc.transform.position, transform.position);

            if (Dis >= m_ItemCheckDis)
            {
                m_NearNpc = null;
                m_Player.m_ItemPickup = false;
            }
        }
    }
}
