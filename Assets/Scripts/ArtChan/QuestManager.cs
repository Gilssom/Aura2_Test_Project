using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int QuestId; // 현재 진행중인 퀘스트 Id
    public int QuestActionIndex; // 퀘스트 대화순서 변수 생성
    public GameObject[] QuestObject; // 퀘스트 오브젝트를 저장할 변수 생성
    public BoxCollider FirstGate;

    // 퀘스트 데이터를 저장할 Dictionary 변수 생성
    Dictionary<int, QuestData> QuestList;

    void Awake()
    {
        DontDestroyOnLoad(this);
        QuestList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        // Add 함수로 퀘스트 아이디 와 퀘스트 데이터 를 저장
        QuestList.Add(10, new QuestData("대화해보기", new int[] { 1000 }));
        QuestList.Add(20, new QuestData("야광귀와 대화해보기", new int[] { 2000, 2000 }));
        QuestList.Add(30, new QuestData("야광귀의 빛을 따라가보기", new int[] { 0 }));
    }

    // Npc Id 를 받고 퀘스트번호 를 반환하는 함수 생성
    public int GetQuestTalkIndex(int id)
    {
        return QuestId + QuestActionIndex; // 퀘스트번호 + 퀘스트 대화순서 = 퀘스트 대화 Id
    }

    public string CheckQuest(int id)
    {
        // 다음 대화
        if (id == QuestList[QuestId].NpcId[QuestActionIndex])
            QuestActionIndex++;


        // 퀘스트 오브젝트 관리
        ControlObject();

        // 퀘스트 대화순서가 끝에 도달했을 때 퀘스트번호 증가
        if (QuestActionIndex == QuestList[QuestId].NpcId.Length)
            NextQuest();

        // 퀘스트 이름
        return QuestList[QuestId].QuestName;
    }

    // 오버로딩 :: 매개변수에 따라 함수를 호출한다.
    public string CheckQuest()
    {
        // 퀘스트 이름
        return QuestList[QuestId].QuestName;
    }

    void NextQuest()
    {
        QuestId += 10;
        QuestActionIndex = 0;
    }

    // 퀘스트 오브젝트를 관리할 함수 생성
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
