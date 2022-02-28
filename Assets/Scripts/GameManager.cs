using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float FadeTime = 2f; // Fade ȿ�� ����ð�

    public Canvas _UICanvas;

    private PlayerController _Player;
    private Image _FadeBG;

    public GameObject m_PickPlayer;
    private Transform _PlayerPos;

    public Transform FirstStartPos;
    public Transform SecondStartPos;

    public int m_SceneNum;

    float _Start;
    float _End;
    float _Time = 0f;

    public bool isPlaying = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(_UICanvas);
    }

    void Start()
    {
        if(m_SceneNum == 0)
        {
            GameObject obj = Resources.Load("SwordWarrior") as GameObject;
            Debug.Log(obj);

            m_PickPlayer = obj;
            _PlayerPos = obj.transform;
        }

        if(m_SceneNum == 1)
        {
            Instantiate(m_PickPlayer);
        }

        if(m_SceneNum > 0)
        {
            _Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _PlayerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        _FadeBG = GameObject.FindWithTag("FadeImage").GetComponent<Image>();
    }
    
    void Update()
    {

    }

    public void CheckPlayer(GameObject Player)
    {

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
            //_Player.isPortal = false;
            //_PlayerPos.transform.position = FirstStartPos.transform.position;
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
