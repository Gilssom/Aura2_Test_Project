using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float FadeTime = 2f; // Fade È¿°ú Àç»ý½Ã°£

    public Canvas _UICanvas;

    [SerializeField]
    private PlayerController _Player;
    private Image _FadeBG;

    public MonsterTest m_Monster;
    
    [SerializeField]
    private Transform _PlayerPos;
    
    public Transform SecondStartPos;

    public int m_SceneNum;

    float _Start;
    float _End;
    float _Time = 0f;

    public GameObject m_Player;
    public int PlayerIndex; 
    public bool isPlaying = false;

    private static GameManager m_instance;
    // ½Ì±ÛÅæ
    public static GameManager Instance
    {
        get
        {
            if(!m_instance)
            {
                m_instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (m_instance == null)
                    Debug.Log("No Singletone Obj");
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
        }
        else if(m_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(_UICanvas);
    }

    void Start()
    {
        _FadeBG = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneChange");
        if (m_SceneNum != 0)
        {
            Debug.Log("Check");
            //_Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _Player = m_Player.GetComponent<PlayerController>();
            _PlayerPos = m_Player.GetComponent<Transform>();

            m_Monster = GameObject.FindWithTag("Monster").GetComponentInParent<MonsterTest>();
            _Player._Monster = m_Monster;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OutStartFadeAnim()
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 1f;
        _End = 0f;
        m_SceneNum++;

        StartCoroutine("FadeOutPlay");
    }

    public void InStartFadeAnim()
    {
        if(isPlaying == true)
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

        NextField(m_SceneNum);
    }

    void NextField(int FieldNum)
    {
        if (FieldNum == 0)
        {
            SceneManager.LoadScene("DoTweenSample");
            OutStartFadeAnim();
        }
        if (FieldNum == 1)
        {
            SceneManager.LoadScene("SecondScene");
            _Player.isPortal = false;
            _PlayerPos.transform.position = SecondStartPos.transform.position;
            OutStartFadeAnim();
        }
    }
}
