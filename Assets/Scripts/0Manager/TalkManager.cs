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
        // Winter Stage Object
        m_talkData.Add(1000, new string[] { "..." , "불상이다." });
        m_talkData.Add(2000, new string[] { "뭘 보는거야." });
        m_talkData.Add(3000, new string[] { "..." , "미궁에도 불상이 있다." });
        m_talkData.Add(4000, new string[] { "얼리기 스킬을 획득하였습니다." });

        // Quest Talk
        // Winter Stage
        m_talkData.Add(10 + 1000, new string[] { "나무아미타불 관세음보살" });

        m_talkData.Add(20 + 2000, new string[] { "뭐야 너?", "...", "꺄아아악!!! 인간이다!!!" });
        m_talkData.Add(21 + 2000, new string[] { "나도 성불할 뻔 했네.. 물론 그럴 일은 없지만..",
        "너 정체가 대체 뭐야?", "...", "뭐? 널 버린 아버지를 살리려고 왔다고?", "제정신이 아니구나...",
        "뭐 그래도 아무말 없이 누워있는게 괘씸할 수도 있겠군..","그 요상한 방울은 뭐고?","스님한테 받은 금방울이라고? 하얀데?",
        "아... 백금?","...","어쨌든 너가 찾는건 아마 여기 없을거야.","여기는 얼음과 고독밖에 없다고..","여긴 지옥이니까!!!",
        "음...","나갈 수 있는 방법이 딱 하나 있는데 너가 성공할지는 모르겠어.","그래도 밑저야 본전이지!","일단 날 따라와봐."});

        // Winter _ Maze Stage
        m_talkData.Add(40 + 4000, new string[] { "여기를 통과하면 이곳을 빠져나갈 수 있어!",
            "쉽지는 않겠지만..."});
        m_talkData.Add(41 + 4000, new string[] {"뭐지?", "아, 잠깐 붙어있을께~", "화이팅!!"});
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
