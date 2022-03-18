using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // 대화 데이터를 저장할 Dictionary
    Dictionary<int, string[]> m_talkData;

    [SerializeField]
    bool LastTalk;

    void Awake()
    {
        DontDestroyOnLoad(this);
        m_talkData = new Dictionary<int, string[]>(); // 초기화
        GenerateData();
    }

    void GenerateData()
    {
        // Add 로 대화 내용 추가 , 대화 하나에는 여러 문장이 들어있으므로 배열로 사용
        m_talkData.Add(1000, new string[] { "..." , "불상이다." });
        m_talkData.Add(2000, new string[] { "뭘 보는거야." });

        // Quest Talk
        m_talkData.Add(10 + 1000, new string[] { "어서오세요." , "무슨 일로 오셨나요." });

        m_talkData.Add(20 + 2000, new string[] { "너는 누구야?!", "... 사람이잖아!!", "멀리 떨어져!!" });
        m_talkData.Add(21 + 2000, new string[] { "잔말 말고 일단 나만 따라와." });
    }

    // 지정된 대화 문장을 반환하는 함수 생성
    public string GetTalk(int id, int talkIndex)
    {
        // ContainsKey() :: Dictionary 에 데이터가 있는지 확인을 해줌
        if(!m_talkData.ContainsKey(id))
        {
            // 퀘스트 맨 처음 대사마저 없을때
            // 기본 대사를 가지고 오면 된다.
            if (!m_talkData.ContainsKey(id - id % 10))
            {
                // 반환 값이 있는 재귀함수는 return 까지 꼭 써주어야 한다.
                return GetTalk(id - id % 100, talkIndex); // Get First Talk
            }
            // 해당 퀘스트 진행 순서 대사가 없을때
            // 퀘스트 맨 처음 대사를 가져온다
            else
            {
                return GetTalk(id - id % 10, talkIndex); // Get First Quest Talk
            }
        }

        // 대화의 문장 갯수를 비교하여 끝을 확인\
        if(talkIndex == m_talkData[id].Length)
        {
            LastTalk = true;
            return null;
        }
        else
        {
            // id 로 대화 Get -> talkIndex 로 대화의 한 문장을 Get
            LastTalk = false;
            return m_talkData[id][talkIndex];
        }
    }
}
