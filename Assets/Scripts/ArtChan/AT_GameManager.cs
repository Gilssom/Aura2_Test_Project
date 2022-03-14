using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AT_GameManager : MonoBehaviour
{
    public GameObject m_TalkPanel;
    public Text m_TalkText;
    public GameObject m_ScanObject;
    public bool isAction;

    public void Action(GameObject scanObj)
    {
        // 대화창에서 나왔을때
        if (isAction) 
        {
            isAction = false;
        }
        // 대화창에 진입했을때
        else
        {
            isAction = true;
            m_ScanObject = scanObj;
            m_TalkText.text = "이것은 " + scanObj.name + " 이다.";
        }

        m_TalkPanel.SetActive(isAction); // isAction 상태에 따라 대화창이 껏다 켜졌다 함
    }
}
