using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image[] m_TutorialImage;
    public Text m_TutorialText;

    float _Start;
    float _End;
    float _Time = 0f;
    bool isPlaying = false;

    private static TutorialManager m_instance;
    // 싱글톤
    public static TutorialManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(TutorialManager)) as TutorialManager;

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

        //DontDestroyOnLoad(this);
    }

    private void Update()
    {
        m_TutorialText.color = m_TutorialImage[0].color;
    }

    public void OutStartFadeAnim(int TutorialNum)
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 1f;
        _End = 0f;

        StartCoroutine(FadeOutPlay(TutorialNum));
    }

    public void InStartFadeAnim(int TutorialNum) // m_GameManager.InStartFadeAnim(); 로 호출하여 페이드 인 아웃 시스템 재생
    {
        if (isPlaying == true)
        {
            return;
        }

        _Start = 0f;
        _End = 1f;

        StartCoroutine(FadeInPlay(TutorialNum));
    }

    IEnumerator FadeOutPlay(int TutorialNum)
    {
        isPlaying = true;

        Color fadecolor = m_TutorialImage[TutorialNum].color;

        _Time = 0f;

        fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

        while (fadecolor.a > 0f)
        {
            _Time += Time.deltaTime / 3;

            fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

            m_TutorialImage[TutorialNum].color = fadecolor;

            yield return null;
        }

        isPlaying = false;
        //this.gameObject.SetActive(false);
    }

    IEnumerator FadeInPlay(int TutorialNum)
    {
        isPlaying = true;

        Color fadecolor = m_TutorialImage[TutorialNum].color;

        _Time = 0f;

        fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

        while (fadecolor.a < 1f)
        {
            _Time += Time.deltaTime / 3;

            fadecolor.a = Mathf.Lerp(_Start, _End, _Time);

            m_TutorialImage[TutorialNum].color = fadecolor;

            yield return null;
        }

        isPlaying = false;
        OutStartFadeAnim(TutorialNum);
    }
}
