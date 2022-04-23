using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MazeManager : MonoBehaviour
{
    public GameObject[] m_Stage;

    private BariController m_Player;

    public Camera m_Camera;
    public Camera m_MazeCam;
    public Camera m_MazeQuaterCam;
    public GameObject m_MazeGround;
    public Light m_Light;
    public Light m_MazeLigth;
    public Transform m_MazeStartPos;

    public Vector3 m_MazeCamPoint;

    private static MazeManager m_instance;
    // ΩÃ±€≈Ê
    public static MazeManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(MazeManager)) as MazeManager;

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
    }

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<BariController>();
    }

    public void StageCtrl(int StageNum)
    {
        if(StageNum == 0)
        {
            m_Stage[StageNum].SetActive(true);
        }
        else if(StageNum != 0)
        {
            m_Stage[StageNum - 1].SetActive(false);
            m_Stage[StageNum].SetActive(true);
        }
    }

    public void MazeCamChange() // Maze Npc Talking Next Method
    {
        m_Camera.gameObject.SetActive(false);
        m_Light.gameObject.SetActive(false);
        m_MazeLigth.gameObject.SetActive(true);
        m_MazeCam.gameObject.SetActive(true);
        m_MazeCam.transform.DOMove(m_MazeCamPoint, 4);
        m_MazeCam.transform.DORotate(new Vector3(71.5f, 0, 0), 4);
    }

    public IEnumerator MazeStart(int StageNum)
    {
        m_Player.isFading = true;
        AT_GameManager.Instance.InStartFadeAnim(0.2f, false);
        AT_GameManager.Instance.isMazePlaying = true;
        yield return new WaitForSeconds(3);
        StageCtrl(StageNum);
        if (StageNum == 0) // Normal Maze Stage
        {
            m_Player.isMazePlay = true;
            m_MazeGround.gameObject.SetActive(true);
        }
        else if (StageNum == 2 || StageNum == 7) // Queater View Maze Stage
        {
            m_MazeLigth.gameObject.SetActive(false);
            m_Light.gameObject.SetActive(true);
            m_Player.isMazePlay = false;
            m_MazeCam.gameObject.SetActive(false);
            m_MazeGround.gameObject.SetActive(false);
        }
        else if (StageNum == 3) // Middle Boss Stage
        {
            m_MazeQuaterCam.gameObject.SetActive(true);
        }
        else if (StageNum == 4) // Not Boss Stage
        {
            m_MazeQuaterCam.gameObject.SetActive(false);
        }
        else if(StageNum == 5) // Queatur View Maze Stage
        {
            m_Light.gameObject.SetActive(false);
            m_MazeLigth.gameObject.SetActive(true);
            m_MazeQuaterCam.gameObject.SetActive(false);
            m_MazeGround.gameObject.SetActive(true);
            m_MazeCam.gameObject.SetActive(true);
        }
        m_Player.transform.position = m_MazeStartPos.transform.position;
        AT_GameManager.Instance.OutStartFadeAnim(0.2f);
        m_Player.isFading = false;
        yield return null;
    }

    public void MazeDeath()
    {
        m_Player.transform.position = m_MazeStartPos.transform.position;
    }
}
