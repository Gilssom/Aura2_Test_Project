using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public float m_FadeTime = 2f;

    float m_Start;
    float m_End;
    float m_Time = 0f;

    [SerializeField]
    bool isPlaying = false;

    public int m_CurCutNumber = 0;
    public Image[] m_CutImage;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !isPlaying && m_CurCutNumber < 4)
        {
            OutStartFadeAnim(m_CurCutNumber);
        }
        else if(Input.GetKeyDown(KeyCode.E) && !isPlaying && m_CurCutNumber == 4)
        {
            FadeInOutManager.Instance.InStartFadeAnim("VillageStage", 0);
        }
    }

    public void OutStartFadeAnim(int CutNumber)
    {
        if (isPlaying == true)
        {
            return;
        }

        m_Start = 1f;
        m_End = 0f;

        StartCoroutine(FadeOutPlay(CutNumber));
    }

    public void InStartFadeAnim(int CutNumber)
    {
        m_CutImage[CutNumber].gameObject.SetActive(true);

        m_Start = 0f;
        m_End = 1f;

        StartCoroutine(FadeInPlay(CutNumber));
    }

    IEnumerator FadeOutPlay(int CutNumber)
    {
        isPlaying = true;

        Color fadecolor = m_CutImage[CutNumber].color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a > 0f)
        {
            m_Time += Time.deltaTime / m_FadeTime;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_CutImage[CutNumber].color = fadecolor;

            yield return null;
        }

        m_CutImage[CutNumber].gameObject.SetActive(false);
        m_CurCutNumber++;

        InStartFadeAnim(m_CurCutNumber);
    }

    IEnumerator FadeInPlay(int CutNumber)
    {
        isPlaying = true;

        Color fadecolor = m_CutImage[CutNumber].color;

        m_Time = 0f;

        fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while (fadecolor.a < 1f)
        {
            m_Time += Time.deltaTime / m_FadeTime;

            fadecolor.a = Mathf.Lerp(m_Start, m_End, m_Time);

            m_CutImage[CutNumber].color = fadecolor;

            yield return null;
        }

        isPlaying = false;
    }
}
