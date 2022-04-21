using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public bool isAction;

    float _Start;
    float _End;
    float _Time = 0f;

    public Canvas _UICanvas;
    public Image _FadeBG;
    [SerializeField]
    bool isPlaying = false;

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

    public void InStartFadeAnim(float FadeTime, bool NextField) // m_GameManager.InStartFadeAnim(); 로 호출하여 페이드 인 아웃 시스템 재생
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

        if (NextField)
            SceneChange();
    }

    public void StartButton()
    {
        _FadeBG.gameObject.SetActive(true);
        InStartFadeAnim(2, true);
    }

    void SceneChange()
    {
        SceneManager.LoadScene("WinterStage");
        OutStartFadeAnim(2);
    }
}
