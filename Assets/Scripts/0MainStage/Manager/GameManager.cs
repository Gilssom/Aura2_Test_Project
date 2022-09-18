using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int m_KillCount;

    public GameObject[] m_CountObject;
    public GameObject[] m_DoorObject;
    public GameObject[] m_TutorialObject;

    public GameObject m_Boss;
    public PlayableDirector m_BossCinemachine;
    public bool m_FirstBossEnd;

    private static GameManager m_instance;
    // ΩÃ±€≈Ê
    public static GameManager Instance
    {
        get
        {
            if (!m_instance)
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

    void Update()
    {
        CountObject();
        DoorObject();
    }

    void CountObject()
    {
        if (m_KillCount == 6)
            ObjectCtrl(0, false);
        // Third Field Spawn Controll
        // 2 , 4 :: ObjData.cs
        else if (m_KillCount == 14)
        {
            ObjectCtrl(4, true);
            ObjectCtrl(5, true);
        }
        // Fifth Field
        // 6, 12, 16 :: ObjData.cs
        else if (m_KillCount == 35)
            ObjectCtrl(7, true);
        else if (m_KillCount == 41)
            ObjectCtrl(8, true);
        else if (m_KillCount == 61)
            ObjectCtrl(11, true);
        else if (m_KillCount == 69)
            ObjectCtrl(12, true);
    }

    void DoorObject()
    {
        if (m_KillCount == 10)
        {
            m_DoorObject[0].SetActive(true);
            m_DoorObject[1].SetActive(true);
        }
        else if (m_KillCount == 31)
            m_DoorObject[2].SetActive(true);
        else if (m_KillCount == 56)
        {
            m_DoorObject[3].SetActive(true);
            m_DoorObject[4].SetActive(true);
        }
    }

    public void ObjectCtrl(int GateNumber, bool Active)
    {
        if(GateNumber == 1)
        {
            StartCoroutine(SkyDownGate());
        }
        else
            m_CountObject[GateNumber].SetActive(Active);
    }

    IEnumerator SkyDownGate()
    {
        Debug.Log("Double Check");

        Vector3 GatePos = new Vector3(81.06154f, 18.84901f, 67.98146f);
        SoundManager.Instance.SFXPlay("DoorSFX", AllGameManager.Instance.m_Clip[0]);

        yield return new WaitForSeconds(0.6f);
        m_CountObject[1].transform.DOMove(GatePos, 0.1f);

        yield return null;
    }

    public void Tutorial(int Number)
    {
        TutorialManager.Instance.InStartFadeAnim(Number);
    }

    public void BossStage()
    {
        m_BossCinemachine.gameObject.SetActive(true);
        m_BossCinemachine.Play();
        SoundManager.Instance.BgSoundPlay(SoundManager.Instance.m_BgList[1]);
        m_Boss.gameObject.SetActive(true);
    }
}
