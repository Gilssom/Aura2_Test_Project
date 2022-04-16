using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AT_GameManager : MonoBehaviour
{
    [SerializeField]
    private BariController m_Player;

    public QuestManager m_QuestManager;
    public TalkManager m_TalkManager;
    public GameObject m_TalkPanel;
    public Text m_TalkText;
    public GameObject m_ScanObject;
    public bool isAction;

    int m_SceneNum;
    public int talkIndex;

    float _Start;
    float _End;
    float _Time = 0f;

    public Canvas _UICanvas;
    [SerializeField]
    private Image _FadeBG;
    [SerializeField]
    bool isPlaying = false;

    public bool isMazePlaying = false;

    private static AT_GameManager m_instance;
    // 싱글톤
    public static AT_GameManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(AT_GameManager)) as AT_GameManager;

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
        DontDestroyOnLoad(_UICanvas);
    }

    void Start()
    {
        m_SceneNum = 0;
        _FadeBG = GameObject.Find("FadeImage").GetComponent<Image>();
        m_Player = GameObject.FindWithTag("Player").GetComponent<BariController>();
        Debug.Log(m_QuestManager.CheckQuest());
    }

    public void Action(GameObject scanObj)
    {     
        m_ScanObject = scanObj;
        ObjData objData = m_ScanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        m_TalkPanel.SetActive(isAction); // isAction 상태에 따라 대화창이 껏다 켜졌다 함
    }

    void Talk(int id, bool isNpc)
    {
        // 퀘스트 매니저를 변수로 생성 후 , 퀘스트번호 를 가져옴
        int QuestTalkIndex = m_QuestManager.GetQuestTalkIndex(id);

        string talkData = m_TalkManager.GetTalk(id + QuestTalkIndex, talkIndex); // 퀘스트번호 + Npc Id = 퀘스트 대화 데이터 Id

        // 대화 종료
        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(m_QuestManager.CheckQuest(id));
            return; 
            // void 함수에서 return 은 강제종료 역할
        }
        
        // Npc 일 경우
        if(isNpc)
        {
            m_TalkText.text = talkData;
        }
        // Npc 가 아닐 경우
        else
        {
            m_TalkText.text = talkData;
        }

        isAction = true;
        talkIndex++;
    }

    public void OutStartFadeAnim(float FadeTime)
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 1f;
        _End = 0f;

        StartCoroutine(FadeOutPlay(FadeTime));
    }

    public void InStartFadeAnim(float FadeTime , bool NextField) // m_GameManager.InStartFadeAnim(); 로 호출하여 페이드 인 아웃 시스템 재생
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 0f;
        _End = 1f;

        StartCoroutine(FadeInPlay(FadeTime, NextField));
    }

    IEnumerator FadeOutPlay(float FadeTime)
    {
        isPlaying = true;

        Color fadecolor = _FadeBG.color;

        _Time = 0f;

        fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

        while (fadecolor.a > 0f)
        {
            _Time += Time.deltaTime / FadeTime;

            fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

            _FadeBG.color = fadecolor;

            yield return null;
        }

        isPlaying = false;
    }

    IEnumerator FadeInPlay(float FadeTime, bool NextField)
    {
        isPlaying = true;

        Color fadecolor = _FadeBG.color;

        _Time = 0f;

        fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

        while (fadecolor.a < 1f)
        {
            _Time += Time.deltaTime / FadeTime;

            fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

            _FadeBG.color = fadecolor;

            yield return null;
        }

        isPlaying = false;

        if(NextField)
            SceneChange();
    }

    void SceneChange()
    {
        SceneManager.LoadScene(m_SceneNum + 1);
        m_SceneNum++;
        m_Player.isFading = false;
        m_Player.transform.position = new Vector3(0, 0, 0);
        OutStartFadeAnim(2);
    }
}
