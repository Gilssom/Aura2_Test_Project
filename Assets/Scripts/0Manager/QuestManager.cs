using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{
    public int QuestId; // ���� �������� ����Ʈ Id
    public int QuestActionIndex; // ����Ʈ ��ȭ���� ���� ����
    public GameObject[] QuestObject; // ����Ʈ ������Ʈ�� ������ ���� ����
    public Light m_BariLight;

    public Material m_EnemyMT;

    public PlayableDirector m_PlayableDirector;
    public Camera m_CinemaCam;

    // ����Ʈ �����͸� ������ Dictionary ���� ����
    Dictionary<int, QuestData> QuestList;

    private static QuestManager m_instance;
    // �̱���
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

    void Awake()
    {
        DontDestroyOnLoad(this);
        QuestList = new Dictionary<int, QuestData>();
        GenerateData();

        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void GenerateData()
    {
        // Add �Լ��� ����Ʈ ���̵� �� ����Ʈ ������ �� ����

        // Winter Stage
        QuestList.Add(10, new QuestData("��ȭ�غ���", new int[] { 1000 }));
        QuestList.Add(20, new QuestData("�߱��Ϳ� ��ȭ�غ���", new int[] { 2000, 2000 }));
        QuestList.Add(30, new QuestData("�߱����� ���� ���󰡺���", new int[] { 0 }));

        // Winter _ Maze Stage
        QuestList.Add(40, new QuestData("�̱ÿ� �ִ� �һ��� ã�ƺ���", new int[] { 4000, 4000 }));
        QuestList.Add(50, new QuestData("�߱��Ͱ� �ִ� �� ���� ������", new int[] { 0 }));
    }

    // Npc Id �� �ް� ����Ʈ��ȣ �� ��ȯ�ϴ� �Լ� ����
    public int GetQuestTalkIndex(int id)
    {
        return QuestId + QuestActionIndex; // ����Ʈ��ȣ + ����Ʈ ��ȭ���� = ����Ʈ ��ȭ Id
    }

    public string CheckQuest(int id)
    {
        // ���� ��ȭ
        if (id == QuestList[QuestId].NpcId[QuestActionIndex])
            QuestActionIndex++;


        // ����Ʈ ������Ʈ ����
        ControlObject();

        // ����Ʈ ��ȭ������ ���� �������� �� ����Ʈ��ȣ ����
        if (QuestActionIndex == QuestList[QuestId].NpcId.Length)
            NextQuest();

        // ����Ʈ �̸�
        return QuestList[QuestId].QuestName;
    }

    // �����ε� :: �Ű������� ���� �Լ��� ȣ���Ѵ�.
    public string CheckQuest()
    {
        // ����Ʈ �̸�
        return QuestList[QuestId].QuestName;
    }

    void NextQuest()
    {
        QuestId += 10;
        QuestActionIndex = 0;
    }

    // ����Ʈ ������Ʈ�� ������ �Լ� ����
    void ControlObject()
    {
        switch (QuestId)
        {
            case 10:
                if (QuestActionIndex == 1)
                {
                    QuestObject[0].SetActive(true);
                }
                break;
            case 20:
                if(QuestActionIndex == 1)
                {
                    m_CinemaCam.gameObject.SetActive(true);
                    m_PlayableDirector.Play();
                }
                if (QuestActionIndex == 2)
                {
                    QuestObject[1].SetActive(false);
                    NPCMove.Instance.Move();
                    QuestObject[5].SetActive(true);
                }
                break;
            case 40:
                if(QuestActionIndex == 1)
                {
                    QuestObject[2].transform.DOMoveY(-2, 2);
                }
                if(QuestActionIndex == 2)
                {
                    Vector3 Pos = QuestObject[3].transform.position;
                    GameObject Teleport = Instantiate(QuestObject[4], new Vector3(Pos.x, Pos.y, Pos.z), this.transform.rotation);

                    QuestObject[3].SetActive(false);
                    m_BariLight.DOIntensity(15, 3);
                }
                break;
            case 50:
                if(QuestActionIndex == 0)
                {
                    Debug.Log("Check");   
                }
                break;
        }
    }

    public void SetCamera()
    {
        m_CinemaCam.gameObject.SetActive(false);
    }
}
