using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    private GameObject m_Player;

    public Animator m_FirstAnim;
    public Animator m_SecondAnim;
    public Animator m_ThirdAnim;

    public SelectCameraMove m_Camera;
    public Button m_StartButton;
    public Text m_CharacterName;
    public Text m_CharacterStats;
    public GameObject m_CharacterInfo;
    public GameManager m_manager;
    public Canvas m_Canvas;

    public int m_InputIndex;
    public bool m_Selecting = false;
    
    void Start()
    {
        m_Player = GameObject.FindWithTag("Player");
        m_StartButton.interactable = false;
        m_CharacterInfo.SetActive(false);
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
                    m_CharacterInfo.SetActive(true);
                    m_CharacterName.text = "FirstPlayer";
                    m_CharacterStats.text = "Attack : ???" + "\n" + "Armor : ???" + "\n" + "Speed : ???"; 
                    m_InputIndex = 1;
                    m_FirstAnim.SetBool("Select", true);
                    m_Camera.CameraMove(m_InputIndex);
                }
                else if (hit.transform.gameObject.tag == "Second")
                {
                    m_Selecting = true;
                    m_CharacterInfo.SetActive(true);
                    m_CharacterName.text = "SecondPlayer";
                    m_CharacterStats.text = "Attack : ???" + "\n" + "Armor : ???" + "\n" + "Speed : ???";
                    m_InputIndex = 2;
                    m_SecondAnim.SetBool("Select", true);
                    m_Camera.CameraMove(m_InputIndex);
                }
                else if (hit.transform.gameObject.tag == "Third")
                {
                    m_Selecting = true;
                    m_CharacterInfo.SetActive(true);
                    m_CharacterName.text = "ThirdPlayer";
                    m_CharacterStats.text = "Attack : ???" + "\n" + "Armor : ???" + "\n" + "Speed : ???";
                    m_InputIndex = 3;
                    m_ThirdAnim.SetBool("Select", true);
                    m_Camera.CameraMove(m_InputIndex);
                }
            }
        }

        //m_manager.PlayerIndex = m_InputIndex;

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
        //m_manager.InStartFadeAnim();
        m_Canvas.enabled = false;
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
