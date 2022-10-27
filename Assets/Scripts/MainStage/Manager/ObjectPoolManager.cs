using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsInfo
{
    // Object 들의 정보들을 관리하는 Class
    public string m_ObjectName;
    public GameObject m_Prefab;
    public int m_Count;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;


    /// <summary>
    /// 0 :: Slash Effect
    /// 1 :: Monster Hit Effect
    /// 2 :: Boss Red Bomb
    /// 3 :: Boss Blue Bomb
    /// 4 :: Boss Yellow Bomb
    /// 5 :: Boss White Bomb
    /// 6 :: Drop Soul
    /// 7 :: Drop Heal
    /// 8 :: Lantern Ball
    /// </summary>

    // Object List Save
    [SerializeField]
    ObjectsInfo[] m_ObjectInfos = null;

    [Header("Object Pool Location")]
    [SerializeField]
    Transform m_PoolParent;

    public List<Queue<GameObject>> m_ObjectPoolList;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_ObjectPoolList = new List<Queue<GameObject>>();

        if(m_ObjectInfos != null)
            for (int i = 0; i < m_ObjectInfos.Length; i++)
            {
                m_ObjectPoolList.Add(InsertQueue(m_ObjectInfos[i]));
            }
    }

    Queue<GameObject> InsertQueue(ObjectsInfo prefab_objectInfo)
    {
        Queue<GameObject> m_Queue = new Queue<GameObject>();

        for (int i = 0; i < prefab_objectInfo.m_Count; i++)
        {
            GameObject objectClone = Instantiate(prefab_objectInfo.m_Prefab) as GameObject;

            objectClone.SetActive(false);
            objectClone.transform.SetParent(m_PoolParent);
            m_Queue.Enqueue(objectClone);
        }

        return m_Queue;
    }

    public IEnumerator DestroyObj(float Seconds, int PoolNumber, GameObject Object)
    {
        yield return new WaitForSeconds(Seconds);
        m_ObjectPoolList[PoolNumber].Enqueue(Object);
        Object.SetActive(false);
    }
}
