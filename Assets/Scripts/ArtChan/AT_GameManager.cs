using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AT_GameManager : MonoBehaviour
{
    public float FadeTime;  // Fade 효과 재생시간

    public GameObject m_TalkPanel;
    public Text m_TalkText;
    public GameObject m_ScanObject;
    public bool isAction;
    public Camera m_MainCamera;
    public Camera m_QuestCam;

    float _Start;
    float _End;
    float _Time = 0f;

    public Canvas _UICanvas;
    [SerializeField]
    private Image _FadeBG;
    [SerializeField]
    bool isPlaying = false;

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
    }

    public void Action(GameObject scanObj)
    {
        // 대화창에서 나왔을때
        if (isAction) 
        {
            m_QuestCam.gameObject.SetActive(false);
            m_MainCamera.gameObject.SetActive(true);       
            isAction = false;
        }
        // 대화창에 진입했을때
        else
        {
            m_MainCamera.gameObject.SetActive(false);
            m_QuestCam.gameObject.SetActive(true);
            isAction = true;
            m_ScanObject = scanObj;
            m_TalkText.text = "이것은 " + scanObj.name + " 이다.";
        }

        m_TalkPanel.SetActive(isAction); // isAction 상태에 따라 대화창이 껏다 켜졌다 함
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
