using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int m_QuestId;

    Dictionary<int, QuestData> m_QuestList;

    private static QuestManager m_instance;
    // 싱글톤
    public static QuestManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(QuestManager)) as QuestManager;

                if (m_instance == null)
                    Debug.Log("No Singletone Obj");
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        m_QuestList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        m_QuestList.Add(10, new QuestData("마을 사람들과 대화하기.", new int[] { 1000 , 2000 , 3000 , 4000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return m_QuestId;
    }
}
