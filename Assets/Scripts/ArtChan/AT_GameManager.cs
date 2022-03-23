using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aura2API;

public class AT_GameManager : MonoBehaviour
{
    public float FadeTime;  // Fade 효과 재생시간

    public QuestManager m_QuestManager;
    public TalkManager m_TalkManager;
    public GameObject m_TalkPanel;
    public Text m_TalkText;
    public GameObject m_ScanObject;
    public bool isAction;
    public Camera m_MainCamera;
    public Camera m_QuestCam;
    public MazeController[] m_MazeCtrl;

    public int talkIndex;

    float _Start;
    float _End;
    float _Time = 0f;

    public Canvas _UICanvas;
    [SerializeField]
    private Image _FadeBG;
    [SerializeField]
    bool isPlaying = false;
    [SerializeField]
    public bool isAuraFalse;

    public bool isMazePlaying = false;

    public AuraVolume BoxVolume;

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
        _FadeBG = GameObject.Find("FadeImage").GetComponent<Image>();
        Debug.Log(m_QuestManager.CheckQuest());
    }

    void Update()
    {
        if(isAuraFalse && BoxVolume.densityInjection.strength > 0)
        {
            BoxVolume.densityInjection.strength -= Time.deltaTime * 5;
        }
    }

    public void Action(GameObject scanObj)
    {
        m_MainCamera.gameObject.SetActive(false);         
        m_QuestCam.gameObject.SetActive(true);        
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
            m_QuestCam.gameObject.SetActive(false);
            m_MainCamera.gameObject.SetActive(true);
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

    public void MazeStageIn(int StageNum)
    {
        isMazePlaying = true;

        if (StageNum == 1)
        {
            m_MazeCtrl[StageNum].GetComponent<MazeController>().enabled = true;
            m_MazeCtrl[StageNum + 1].GetComponent<MazeController>().enabled = true;
        }

        m_MazeCtrl[StageNum].GetComponent<MazeController>().enabled = true;
    }

    public void OutStartFadeAnim()
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 1f;
        _End = 0f;
        //m_SceneNum++;

        StartCoroutine("FadeOutPlay");
    }

    public void InStartFadeAnim() // m_GameManager.InStartFadeAnim(); 로 호출하여 페이드 인 아웃 시스템 재생
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 0f;
        _End = 1f;

        StartCoroutine("FadeInPlay");
    }

    IEnumerator FadeOutPlay()
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

    IEnumerator FadeInPlay()
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

        //NextField(m_SceneNum);
    }
}
