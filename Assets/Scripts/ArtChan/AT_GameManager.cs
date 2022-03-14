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
        // ��ȭâ���� ��������
        if (isAction) 
        {
            isAction = false;
        }
        // ��ȭâ�� ����������
        else
        {
            isAction = true;
            m_ScanObject = scanObj;
            m_TalkText.text = "�̰��� " + scanObj.name + " �̴�.";
        }

        m_TalkPanel.SetActive(isAction); // isAction ���¿� ���� ��ȭâ�� ���� ������ ��
    }
}
