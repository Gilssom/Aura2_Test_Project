using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtChanUI : MonoBehaviour
{
    private ArtChanController m_Player;
    private NearItemCheck m_ItemCS;

    public Text m_Text;
    public Text m_ItemNameText;

    private void Awake()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ArtChanController>();
        m_ItemCS = GameObject.FindWithTag("Player").GetComponent<NearItemCheck>();
    }

    void Update()
    {
        m_Text.text = "Skill Stack :: " + m_Player.m_SkillStack + "\n" +
            "Stack Time :: " + m_Player.m_StackTime.ToString("F1") + "\n" +
            "Q Skill Enable :: " + m_Player.m_SkillEnable + "\n" +
            "Parrying ::" + m_Player.isParrying + "\n" +
            "Skill Number ::" + m_Player.m_SkillNum;

        if (m_ItemCS.m_NearItem)
            m_ItemNameText.text = m_ItemCS.m_NearItem.name + " »πµÊ«œ±‚ (E)";
        else
            m_ItemNameText.text = null;
    }
}
