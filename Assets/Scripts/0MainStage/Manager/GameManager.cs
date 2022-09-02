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

    public bool isTalkAction;

    public int m_TalkIndex;

    public Transform[] m_FieldStartPos;

    private GameObject m_Player;
    private NearNpcCheck m_NearNpc;

    [SerializeField]
    public AudioClip[] m_Clip;

    public GameObject[] m_CountObject;
    public GameObject[] m_DoorObject;
    public GameObject[] m_TutorialObject;

    public GameObject m_Boss;

    private static GameManager m_instance;
    // 싱글톤
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
        m_NearNpc = m_Player.GetComponent<NearNpcCheck>();
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

    public void BossStage()
    {
        SoundManager.Instance.BgSoundPlay(SoundManager.Instance.m_BgList[1]);
        UIManager.Instance.BossStageUI();
        m_Boss.gameObject.SetActive(true);
    }

    public void NextField(int SceneNum)
    {
        Debug.Log(SceneNum + " = SceneNumber");
        SceneManager.LoadScene(SceneNum);
        m_Player.transform.position = m_FieldStartPos[SceneNum].position;
        m_Player.transform.rotation = m_FieldStartPos[SceneNum].rotation;
    }

    public void Action()
    {
        ObjData objData = m_NearNpc.m_NearNpc.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        if(objData.isNeedTalk)
        {
            UIManager.Instance.m_TalkPanel.SetActive(isTalkAction);
            UIManager.Instance.m_TalkImage.DOAnchorPosY(-250, 0.75f).SetEase(Ease.OutQuad);
        }
    }

    void Talk(int id, bool isNpc)
    {
        ObjData objData = m_NearNpc.m_NearNpc.GetComponent<ObjData>();
        int questTalkIndex = QuestManager.Instance.GetQuestTalkIndex(id);
        string talkData = TalkManager.Instance.GetTalk(id + questTalkIndex, m_TalkIndex);
        string RandomtalkData = TalkManager.Instance.GetTalk(id, Random.Range(0, 2));

        if(talkData == null)
        {
            m_TalkIndex = 0;
            isTalkAction = false;
            objData.isNeedTalk = false;
            UIManager.Instance.m_TalkImage.DOAnchorPosY(-1000, 0.75f).SetEase(Ease.OutQuad);
            return;
        }

        if(isNpc && objData.isNeedTalk)
        {
            UIManager.Instance.m_TalkText.text = talkData;

            if (m_NearNpc.m_NearNpc.name == "WeaponNpc")
                UIManager.Instance.m_TalkNpcNameText.text = "대장장이";
            else if (m_NearNpc.m_NearNpc.name == "ArmorNpc")
                UIManager.Instance.m_TalkNpcNameText.text = "재봉사";
            else if (m_NearNpc.m_NearNpc.name == "Station Master")
                UIManager.Instance.m_TalkNpcNameText.text = "역원 주인";
            else if (m_NearNpc.m_NearNpc.name == "Gate Keeper")
                UIManager.Instance.m_TalkNpcNameText.text = "문지기";

            isTalkAction = true;
            m_TalkIndex++;
        }

        if(isNpc && !objData.isNeedTalk)
        {
            if (id == 1000)
                UIManager.Instance.m_WeaponPanelText.text = RandomtalkData;
            if (id == 2000)
                UIManager.Instance.m_ArmorPanelText.text = RandomtalkData;
            if (id == 3000)
                UIManager.Instance.m_StationPanelText.text = RandomtalkData;
            if (id == 4000)
                UIManager.Instance.m_GatePanelText.text = RandomtalkData;
        }
        else
        {

        }
    }
}
