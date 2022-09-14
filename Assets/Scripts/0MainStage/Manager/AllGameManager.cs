using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class AllGameManager : MonoBehaviour
{
    private GameObject m_Player;
    private ChoheeController m_Chohee;
    private NearNpcCheck m_NearNpc;

    public GameObject m_PlayCanvas;

    public Transform[] m_FieldStartPos;

    [SerializeField]
    public AudioClip[] m_Clip;

    public bool isPause;
    public bool isWeaponShop;
    public bool isGameOver;

    public bool isTalkAction;

    public int m_TalkIndex;

    private static AllGameManager m_instance;
    // 싱글톤
    public static AllGameManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(AllGameManager)) as AllGameManager;

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
        m_Chohee = m_Player.GetComponent<ChoheeController>();
    }

    private void Update()
    {
        m_PlayCanvas.SetActive(!m_Chohee.isLoading);
    }

    public void NextField(string SceneName, int SceneNum)
    {
        Debug.Log(SceneNum + " = SceneNumber");
        LoadingSceneManager.LoadScene(SceneName);
        m_Player.transform.position = m_FieldStartPos[SceneNum].position;
        m_Player.transform.rotation = m_FieldStartPos[SceneNum].rotation;
        m_Chohee.m_Rigid.useGravity = false;
    }

    public void SceneChange()
    {
        m_Chohee.m_Rigid.useGravity = true;
        m_Chohee.isLoading = false;
    }

    public void Action()
    {
        ObjData objData = m_NearNpc.m_NearNpc.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        if (objData.isNeedTalk)
        {
            UIManager.Instance.m_TalkPanel.SetActive(isTalkAction);
            UIManager.Instance.m_TalkImage.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
        }
    }

    void Talk(int id, bool isNpc)
    {
        ObjData objData = m_NearNpc.m_NearNpc.GetComponent<ObjData>();
        int questTalkIndex = QuestManager.Instance.GetQuestTalkIndex(id);
        string talkData = TalkManager.Instance.GetTalk(id + questTalkIndex, m_TalkIndex);
        string RandomtalkData = TalkManager.Instance.GetTalk(id, Random.Range(0, 2));

        if (talkData == null)
        {
            QuestManager.Instance.m_QuestId = 10;

            if (id == 3000 && !objData.isNeedTalk)
                StationNpcInteraction();

            m_TalkIndex = 0;
            isTalkAction = false;
            objData.isNeedTalk = false;
            UIManager.Instance.m_TalkPanel.SetActive(isTalkAction);
            UIManager.Instance.m_TalkImage.DOAnchorPosY(-500, 0.75f).SetEase(Ease.OutQuad);
            return;
        }

        if (isNpc && objData.isNeedTalk)
        {
            UIManager.Instance.m_TalkText.text = talkData;

            if (m_NearNpc.m_NearNpc.name == "WeaponNpc")
                UIManager.Instance.m_TalkNpcNameImage.sprite = UIManager.Instance.m_CurTalkNpcImage[0];
            //UIManager.Instance.m_TalkNpcNameText.text = "대장장이";
            else if (m_NearNpc.m_NearNpc.name == "ArmorNpc")
                UIManager.Instance.m_TalkNpcNameImage.sprite = UIManager.Instance.m_CurTalkNpcImage[1];
            //UIManager.Instance.m_TalkNpcNameText.text = "재봉사";
            else if (m_NearNpc.m_NearNpc.name == "Station Master")
                UIManager.Instance.m_TalkNpcNameImage.sprite = UIManager.Instance.m_CurTalkNpcImage[2];
            //UIManager.Instance.m_TalkNpcNameText.text = "역원 주인";
            else if (m_NearNpc.m_NearNpc.name == "Gate Keeper")
                UIManager.Instance.m_TalkNpcNameText.text = "문지기";

            isTalkAction = true;
            m_TalkIndex++;
        }

        if (isNpc && !objData.isNeedTalk)
        {
            if (id == 1000)
                UIManager.Instance.m_WeaponPanelText.text = RandomtalkData;
            if (id == 2000)
                UIManager.Instance.m_ArmorPanelText.text = RandomtalkData;
            if (id == 3000)
            {
                m_TalkIndex = 2;
                QuestManager.Instance.m_QuestId = 0;
                UIManager.Instance.m_TalkNpcNameImage.sprite = UIManager.Instance.m_CurTalkNpcImage[2];
                UIManager.Instance.m_TalkText.text = RandomtalkData;
                isTalkAction = true;
                UIManager.Instance.m_TalkPanel.SetActive(isTalkAction);
                UIManager.Instance.m_TalkImage.DOAnchorPosY(0, 0.75f).SetEase(Ease.OutQuad);
                m_TalkIndex++;
            }
            if (id == 4000)
                UIManager.Instance.m_GatePanelText.text = RandomtalkData;
        }
        else
        {

        }
    }

    void StationNpcInteraction()
    {
        if (PlayerStats.Instance.Slash >= PlayerStats.Instance.MaxSlash && PlayerStats.Instance.Health >= PlayerStats.Instance.MaxHealth)
        {
            return;
        }
        if (PlayerStats.Instance.Slash < PlayerStats.Instance.MaxSlash)
        {
            float AddSlashGage = PlayerStats.Instance.MaxSlash - PlayerStats.Instance.Slash;
            PlayerStats.Instance.AddSlashGage(AddSlashGage);
            SoundManager.Instance.SFXPlay("Heal", m_Clip[9]);
            StartCoroutine(m_Chohee.EnforceEff(2));
        }
        if (PlayerStats.Instance.Health < PlayerStats.Instance.MaxHealth)
        {
            float AddHealth = PlayerStats.Instance.MaxHealth - PlayerStats.Instance.Health;
            PlayerStats.Instance.Heal(AddHealth);
            SoundManager.Instance.SFXPlay("Heal", m_Clip[9]);
            StartCoroutine(m_Chohee.EnforceEff(2));
        }
    }
}
