using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    string m_TargetMsg;
    public int m_CharPerSeconds;

    Text m_MsgText;

    int m_Index;
    float m_Interval;
  
    private void Awake()
    {
        m_MsgText = GetComponent<Text>();
        AllGameManager.Instance.m_Text = this;
    }

    public void SetMsg(string msg)
    {
        m_TargetMsg = msg;
        EffectStart();
    }

    void EffectStart()
    {
        m_MsgText.text = "";
        m_Index = 0;

        m_Interval = 1.0f / m_CharPerSeconds;
        Invoke("Effecting", m_Interval);
    }

    void Effecting()
    {
        if(m_MsgText.text == m_TargetMsg)
        {
            EffectEnd();
            return;
        }

        m_MsgText.text += m_TargetMsg[m_Index];
        m_Index++;

        Invoke("Effecting", m_Interval);
    }

    void EffectEnd()
    {

    }
}
