using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtChanUI : MonoBehaviour
{
    private ArtChanController m_Player;

    public Text m_Text;

    private void Awake()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ArtChanController>();
    }

    void Update()
    {
        m_Text.text = "Skill Stack :: " + m_Player.m_SkillStack + "\n" +
            "Stack Time :: " + m_Player.m_StackTime.ToString("F1") + "\n" +
            "Q Skill Enable :: " + m_Player.m_SkillEnable + "\n" +
            "Parrying ::" + m_Player.isParrying;

    }
}
