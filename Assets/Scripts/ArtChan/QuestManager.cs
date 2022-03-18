using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int QuestId; // ���� �������� ����Ʈ Id
    public int QuestActionIndex; // ����Ʈ ��ȭ���� ���� ����
    public GameObject[] QuestObject; // ����Ʈ ������Ʈ�� ������ ���� ����
    public BoxCollider FirstGate;

    // ����Ʈ �����͸� ������ Dictionary ���� ����
    Dictionary<int, QuestData> QuestList;

    void Awake()
    {
        DontDestroyOnLoad(this);
        QuestList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        // Add �Լ��� ����Ʈ ���̵� �� ����Ʈ ������ �� ����
        QuestList.Add(10, new QuestData("��ȭ�غ���", new int[] { 1000 }));
        QuestList.Add(20, new QuestData("�߱��Ϳ� ��ȭ�غ���", new int[] { 2000, 2000 }));
        QuestList.Add(30, new QuestData("�߱����� ���� ���󰡺���", new int[] { 0 }));
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
                    QuestObject[0].SetActive(true);
                break;
            case 20:
                if (QuestActionIndex == 2)
                {
                    FirstGate.isTrigger = true;
                    NPCMove.Instance.Move();
                }
                break;
        }
    }
}
