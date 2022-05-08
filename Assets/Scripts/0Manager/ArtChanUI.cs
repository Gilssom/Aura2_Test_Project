using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtChanUI : MonoBehaviour
{
    private BariController m_Player;
    private NearItemCheck m_ItemCS;

    public Text m_Text;
    public Text m_NearNameText;

    public Image m_Skill1Img;
    public Image m_Skill2Img;

    public GameObject m_MazePanel;
    public GameObject m_SkillPanel;
    public GameObject m_PausePanel;
    bool isPause;

    public Sprite[] m_SkillIcon;

    public Image m_KeyIcon;

    private void Awake()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<BariController>();
        m_ItemCS = GameObject.FindWithTag("Player").GetComponent<NearItemCheck>();
    }

    void Update()
    {
        m_Text.text = "Skill Stack :: " + m_Player.m_SkillStack + "\n" +
            "Stack Time :: " + m_Player.m_StackTime.ToString("F1") + "\n" +
            "Q Skill Enable :: " + m_Player.m_SkillEnable + "\n" +
            "Parrying ::" + m_Player.isParrying + "\n" +
            "Skill Number ::" + m_Player.m_SkillNum + "\n" +
            "Tport Count ::" + m_Player.m_TportCount + "\n" +
            "Tport AddTime ::" + m_Player.m_TportAddTime.ToString("F1");

        if (m_ItemCS.m_NearItem)
            m_NearNameText.text = m_ItemCS.m_NearItem.name + " »πµÊ«œ±‚ (E)";
        //else if (m_Player.scanObject && !AT_GameManager.Instance.isAction)
            //m_NearNameText.text = "¥Î»≠«œ±‚ (E)";
        else
            m_NearNameText.text = null;

        SkillImageChange();
        
        if (AT_GameManager.Instance.isMazePlaying)
        {
            if (!m_Player.isMazePlay)
                m_SkillPanel.SetActive(true);
            else
            {
                m_SkillPanel.SetActive(false);
                //m_MazePanel.SetActive(true);
            }
        }


        if(m_Player.m_UseMazeKey)
        {
            Color color = m_KeyIcon.color;
            color.a = 1.0f;
            m_KeyIcon.color = color;
        }
    }

    void SkillImageChange()
    {
        float SkillNum = m_Player.m_SkillNum;
        if(SkillNum == 0)
        {
            m_Skill1Img.gameObject.SetActive(false);
        }
        else if (SkillNum == 1)
        {
            m_Skill1Img.gameObject.SetActive(true);
            m_Skill1Img.sprite = m_SkillIcon[0];
            //m_Skill2Img.sprite = m_SkillIcon[1];
        }
        else if (SkillNum == 2)
        {
            m_Skill1Img.sprite = m_SkillIcon[1];
            //m_Skill2Img.sprite = m_SkillIcon[2];
        }
        else if(SkillNum == 3)
        {
            m_Skill1Img.sprite = m_SkillIcon[2];
            //m_Skill2Img.sprite = m_SkillIcon[3];
        }
        else if (SkillNum == 4)
        {
            m_Skill1Img.sprite = m_SkillIcon[3];
            //m_Skill2Img.sprite = m_SkillIcon[2];
        }
    }

    public void PauseMenuCtrl()
    {
        Debug.Log("Check");

        if (isPause)
        {
            m_PausePanel.SetActive(false);
            isPause = false;
            Time.timeScale = 1;
        }
        else
        {
            m_PausePanel.SetActive(true);
            isPause = true;
            Time.timeScale = 0;
        }
    }
}
