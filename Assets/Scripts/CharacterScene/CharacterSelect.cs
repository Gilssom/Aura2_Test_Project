using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public SelectCameraMove m_Camera;
    public Button m_StartButton;
    public Text m_CharacterName;

    public int m_InputIndex;
    public bool m_Selecting = false;
    
    void Start()
    {
        m_StartButton.interactable = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "First")
                {
                    m_Selecting = true;
                    m_CharacterName.text = "FirstPlayer";
                    m_InputIndex = 1;
                    m_Camera.CameraMove(m_InputIndex);
                }
                else if (hit.transform.gameObject.tag == "Second")
                {
                    m_Selecting = true;
                    m_CharacterName.text = "SecondPlayer";
                    m_InputIndex = 2;
                    m_Camera.CameraMove(m_InputIndex);
                }
                else if (hit.transform.gameObject.tag == "Third")
                {
                    m_Selecting = true;
                    m_CharacterName.text = "ThirdPlayer";
                    m_InputIndex = 3;
                    m_Camera.CameraMove(m_InputIndex);
                }
            }
        }

        if (m_Selecting)
        {
            m_StartButton.interactable = true;
        }
        else
            m_StartButton.interactable = false;
    }

    public void PlayerSelect()
    {
        m_CharacterName.text = "Game Start !";
        if (m_InputIndex == 1)
        {
            Debug.Log("First Player Start");
        }
        else if (m_InputIndex == 2)
        {
            Debug.Log("Second Player Start");
        }
        else if (m_InputIndex == 3)
        {
            Debug.Log("Third Player Start");
        }
        else
            return;
    }
}
