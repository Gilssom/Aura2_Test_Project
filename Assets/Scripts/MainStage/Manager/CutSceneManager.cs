using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public GameObject m_BlackPlayer;
    public GameObject m_Cinemachine;

    public float m_FadeTime = 2f;

    float m_Start;
    float m_End;
    float m_Time = 0f;

    [SerializeField]
    bool isPlaying = false;

    [SerializeField]
    bool isContinue = false;

    public int m_CurCutNumber = 0;
    public Image[] m_CutImage;
    public Text[] m_CutText;
    public Image m_BGPanel;
    public Image m_WhitePanel;

    public GameObject m_ContinuePanel;
    public GameObject m_ContinueText;

    private void Start()
    {
        StartCoroutine(InStartFadeAnim(m_CurCutNumber));
        Invoke("BGPlay", 7.33f);
        Invoke("BGStop", 28.33f);
    }

    private void Update()
    {
        Continue();
    }

    void Continue()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isContinue)
            {
                isContinue = true;
                m_ContinueText.SetActive(true);
            }
            else
            {
                m_ContinueText.SetActive(false);
                StartCoroutine(FadeInPlayBG());
            }
        }
    }

    void BGPlay()
    {
        SoundManager.Instance.BgSoundPlay(SoundManager.Instance.m_BgList[4]);
    }

    void BGStop()
    {
        SoundManager.Instance.m_BgSound.Stop();
    }

    IEnumerator OutStartFadeAnim(int CutNumber)
    {
        if (isPlaying == true)
        {
            yield return null;
        }

        m_Start = 1f;
        m_End = 0f;

        StartCoroutine(FadeOutPlay(CutNumber));

        yield return null;
    }

    IEnumerator InStartFadeAnim(int CutNumber)
    {
        if(m_CurCutNumber == 0)
            yield return new WaitForSeconds(1f);

        //Debug.Log("Start");
        //m_CutImage[CutNumber].gameObject.SetActive(true);

        m_Start = 0f;
        m_End = 1f;

        StartCoroutine(FadeInPlay(CutNumber));

        yield return null;
    }

    IEnumerator FadeOutPlay(int CutNumber)
    {
        isPlaying = true;
        
        Color fadecolor = m_CutText[CutNumber].color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a > 0f)
        {
            m_Time += Time.deltaTime / 2;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_CutText[CutNumber].color = fadecolor;

            yield return null;
        }

        //m_CutImage[CutNumber].gameObject.SetActive(false);
        isPlaying = false;
        m_CurCutNumber++;

        if(m_CurCutNumber < 3)
            StartCoroutine(OutStartFadeAnim(m_CurCutNumber));
        else if(m_CurCutNumber == 3)
        {
            m_BlackPlayer.SetActive(true);
            m_Cinemachine.SetActive(true);
            StartCoroutine(OutStartFadeBG());
        }


        //InStartFadeAnim(m_CurCutNumber);
        //OutStartFadeAnim(3);
    }

    IEnumerator FadeInPlay(int CutNumber)
    {
        isPlaying = true;

        //Color fadecolor = m_CutImage[CutNumber].color;
     
        Color fadecolor = m_CutText[CutNumber].color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a < 1f)
        {
            m_Time += Time.deltaTime / 4;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_CutText[CutNumber].color = fadecolor;

            yield return null;
        }

        if (m_CurCutNumber == 0)
            m_ContinuePanel.SetActive(true);

        isPlaying = false;
        m_CurCutNumber++;

        if (m_CurCutNumber < 3)
            StartCoroutine(InStartFadeAnim(m_CurCutNumber));
        else if(m_CurCutNumber == 3)
        {
            m_CurCutNumber = 0;
            StartCoroutine(OutStartFadeAnim(0));
        }
    }

    // Fade BackGround

    IEnumerator OutStartFadeBG()
    {
        if (isPlaying == true)
        {
            yield return null;
        }

        m_Start = 1f;
        m_End = 0f;

        StartCoroutine(FadeOutPlayBG());

        yield return null;
    }

    IEnumerator FadeOutPlayBG()
    {
        isPlaying = true;

        Color fadecolor = m_BGPanel.color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a > 0f)
        {
            m_Time += Time.deltaTime / 2;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_BGPanel.color = fadecolor;

            yield return null;
        }

        isPlaying = false;
        m_BGPanel.gameObject.SetActive(false);
        StartCoroutine(InStartFadeBG());
    }

    public IEnumerator InStartFadeBG()
    {
        yield return new WaitForSeconds(6.33f);

        //Debug.Log("Start");
        //m_CutImage[CutNumber].gameObject.SetActive(true);

        m_Start = 0f;
        m_End = 1f;

        StartCoroutine(FadeInPlayBG());

        yield return null;
    }

    IEnumerator FadeInPlayBG()
    {
        isPlaying = true;

        //Color fadecolor = m_CutImage[CutNumber].color;

        Color fadecolor = m_WhitePanel.color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a < 1f)
        {
            m_Time += Time.deltaTime / 1;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_WhitePanel.color = fadecolor;

            yield return null;
        }

        isPlaying = false;

        FadeInOutManager.Instance.InStartFadeAnim("VillageStage", 0);
    }
}
