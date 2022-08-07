using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int m_KillCount;

    public bool isPause;
    public bool isWeaponShop;

    public Transform[] m_FieldStartPos;

    private GameObject m_Player;

    [SerializeField]
    public AudioClip[] m_Clip;

    public GameObject[] m_CountObject;
    public GameObject[] m_DoorObject;
    public GameObject[] m_TutorialObject;

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

        DontDestroyOnLoad(this);

        isPause = false;

        m_Player = GameObject.FindWithTag("Player");
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
        else if (m_KillCount == 16)
            ObjectCtrl(3, true);
        else if (m_KillCount == 23)
            ObjectCtrl(5, true);
        // Fifth Field
        // 6, 12, 16 :: ObjData.cs
        else if (m_KillCount == 45)
            ObjectCtrl(7, true);
        else if (m_KillCount == 49)
            ObjectCtrl(8, true);
        else if (m_KillCount == 52)
            ObjectCtrl(9, true);
        else if (m_KillCount == 57)
            ObjectCtrl(10, true);
        else if (m_KillCount == 61)
            ObjectCtrl(11, true);
        else if (m_KillCount == 84) // 3-3 Spawn
            ObjectCtrl(13, true);
        else if (m_KillCount == 89) // 3-3 Spawn
            ObjectCtrl(14, true);
        else if (m_KillCount == 92) // 3-3 Spawn
            ObjectCtrl(15, true);
        else if (m_KillCount == 101) // 3-5 Spawn
            ObjectCtrl(17, true);
        else if (m_KillCount == 106) // 3-5 Spawn
            ObjectCtrl(18, true);
        else if (m_KillCount == 112) // 3-5 Spawn
            ObjectCtrl(19, true);
    }

    void DoorObject()
    {
        if (m_KillCount == 12)
            m_DoorObject[0].SetActive(true);
        else if (m_KillCount == 19)
            m_DoorObject[1].SetActive(true);
        else if (m_KillCount == 41)
            m_DoorObject[2].SetActive(true);
        else if (m_KillCount == 78) // 78 ~ 79
            m_DoorObject[3].SetActive(true);
        else if (m_KillCount == 96) // 96 ~ 97
            m_DoorObject[4].SetActive(true);
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
        Vector3 GatePos = new Vector3(81.06154f, 18.84901f, 67.98146f);

        SoundManager.Instance.SFXPlay("DoorSFX", m_Clip[0]);
        yield return new WaitForSeconds(0.6f);
        m_CountObject[1].transform.DOMove(GatePos, 0.1f);
        
        yield return null;
    }

    public void Tutorial(int Number)
    {
        TutorialManager.Instance.InStartFadeAnim(Number);
    }

    public void NextField(int SceneNum)
    {
        Debug.Log(SceneNum + " = SceneNumber");
        SceneManager.LoadScene(SceneNum);
        m_Player.transform.position = m_FieldStartPos[SceneNum].position;
        m_Player.transform.rotation = m_FieldStartPos[SceneNum].rotation;
    }
}
