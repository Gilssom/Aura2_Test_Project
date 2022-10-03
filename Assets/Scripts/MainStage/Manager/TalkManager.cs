using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private static TalkManager m_instance;
    // 싱글톤
    public static TalkManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(TalkManager)) as TalkManager;

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

        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "지금도 충분히 멋진 무기지만," + "\n" + "내 손길 한번이면 더 멋져진다네!",
                                            "어디 영혼은" + "\n" + "많이 구해왔나?",
                                            "고생이 많구만."});
        talkData.Add(2000, new string[] { "옷이 상하지는 않았니?",
                                            "너가 참 고생이 많아.",
                                            "내 옷은 내가" + "\n" + "직접 짓는 단다."});
        talkData.Add(3000, new string[] { "힘드신 가요?",
                                            "푹 쉬세요.",
                                            "무슨 일 있으신가요?"});
        talkData.Add(4000, new string[] { "조심하십시오.",
                                            "조심하십시오.", 
                                            "조심하십시오."});

        talkData.Add(10 + 1000, new string[] { "여기에 온다고 서찰을 보냈던 사람이" + "\n" + "자네가 맞는가?",
                                            "허허.. 아무리 말세라지만"  + "\n" + "이런 꼬맹이가 오다니 걱정이구만.",
                                            "... 그래 그래도 상황이 상황인지라"  + "\n" + "어쩔 수가 없구만..",
                                            "난 신선에게 신묘한 망치를 받아 특별한 대장술을 쓸 수 있네."  + "\n" + "나정도면 직접 싸워도 될 것 같구만..",
                                            "아무튼 영혼으로 쇠를 벼를 수 있으니,"  + "\n" + "영혼을 충분히 모았다면 찾아와주게."});

        talkData.Add(10 + 2000, new string[] { "이렇게 어린 여자애가 올 줄은 몰랐네.",
                                            "싫은 건 아니야. 난 여자 옷 만드는 게 더 좋더라.",
                                            "지금 입고 있는 옷도 정말 곱구나.",
                                            "영혼과 옷감이 만나면 대단한 옷이 된단다?"  + "\n" + "영혼을 충분히 모았다면 찾아와줘."});

        talkData.Add(10 + 3000, new string[] { "어서오시지요...",
                                            "이곳은 아시다 싶이 신선에게 신묘한 능력을 받은 이들이 모여,"  + "\n" + "지옥귀를 토벌하던 거점인데 지금은 많이 누추하지요.",
                                            "그동안 많은 희생이 있었지요.",
                                            "그쪽처럼 어린 분을 끌어 드리고싶지는 않았지만,"  + "\n" + "상황이 상황인지라...",
                                            "어쨌든 잘 부탁드립니다."  + "\n" + "세상이 이렇게 망할 수는 없는 노릇이니까요.",
                                            "쉬고 싶으실 때 언제든 오세요."});

        talkData.Add(10 + 4000, new string[] { "안녕하세요.",
                                            "지옥도와 인간도의 균형이 깨져... 이곳 저곳의 공간이 찢어져,"  + "\n" + "지옥귀들이 들어닥치는 상황에...",
                                            "제 능력으로 지옥문을 열어 지옥귀들을 처치하면," + "\n" + "그런 일도 없어진다는 사실을 알게 되었죠.",
                                            "그래서 많은 영웅들이 제 문을 지나고..."  + "\n" + "많이들 돌아오시지 못했습니다.",
                                            "그래서... 마음이 가지 않는다면"  + "\n" + "되도록 이 일을 하지 않으셨으면 합니다.",
                                            "계속 이래봤자 멸망을 겨우겨우 조금씩 늦출 뿐...",
                                            "그러는 제가 왜 이러고 있는지 물으신다면 할 말은 없지만," + "\n" + "무거운 마음으로 할 일을 겨우 하는 형국입니다.",
                                            "말이 길어졌네요." + "\n" + "그래도 마음을 먹으셨다면 무사하시길 바랍니다."});
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
