using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInOutManager : MonoBehaviour
{
    [SerializeField]
    private Image m_FadeBG;
    public float m_FadeTime = 2f;

    float m_Start;
    float m_End;
    float m_Time = 0f;

    [SerializeField]
    bool isPlaying = false;

    private static FadeInOutManager m_instance;
    // ΩÃ±€≈Ê
    public static FadeInOutManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(FadeInOutManager)) as FadeInOutManager;

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

        m_FadeBG = GameObject.Find("FadeBG").GetComponent<Image>();
        //StartCoroutine(GameStartFade());
    }

    IEnumerator GameStartFade()
    {
        yield return new WaitForSeconds(1);

        Color fadecolor = m_FadeBG.color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(1, 0, m_Time);

        while (fadecolor.a > 0f)
        {
            m_Time += Time.deltaTime / 4;

            fadecolor.a = Mathf.Lerp(1, 0, m_Time);

            m_FadeBG.color = fadecolor;

            yield return null;
        }

        isPlaying = false;
    }

    public void OutStartFadeAnim(string NextScene)
    {
        if (isPlaying == true)
        {
            return;
        }

        m_Start = 1f;
        m_End = 0f;

        StartCoroutine(FadeOutPlay(NextScene));
    }

    public void InStartFadeAnim(string SceneName, int SceneNumber)
    {
        if (isPlaying == true)
        {
            return;
        }

        m_Start = 0f;
        m_End = 1f;

        StartCoroutine(FadeInPlay(SceneName, SceneNumber));
    }

    IEnumerator FadeOutPlay(string NowScene)
    {
        isPlaying = true;

        Color fadecolor = m_FadeBG.color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a > 0f)
        {
            m_Time += Time.deltaTime / m_FadeTime;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_FadeBG.color = fadecolor;

            yield return null;
        }

        isPlaying = false;

        if(NowScene != "CutScene")
            AllGameManager.Instance.SceneChange();
    }

    IEnumerator FadeInPlay(string SceneName, int SceneNumber)
    {
        isPlaying = true;

        Color fadecolor = m_FadeBG.color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a < 1f)
        {
            m_Time += Time.deltaTime / m_FadeTime;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_FadeBG.color = fadecolor;

            yield return null;
        }

        isPlaying = false;

        if(SceneNumber == 1 || SceneNumber == 2)
            AllGameManager.Instance.NextField(SceneName, SceneNumber);
        else if(SceneNumber == 0)
            LoadingSceneManager.LoadScene(SceneName);
    }
}
