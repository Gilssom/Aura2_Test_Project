using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectCameraMove : MonoBehaviour
{
    public Vector3 m_FirstPos;
    public Vector3 m_SecondPos;
    public Vector3 m_ThridPos;
    public Vector3 m_ResetPos;

    private CharacterSelect m_Select;

    void Start()
    {
        m_Select = GameObject.Find("Manager").GetComponent<CharacterSelect>();
    }

    public void CameraMove(int Num)
    {
        if (Num == 1)
        {
            Debug.Log("FirstPlayer Select");
            transform.DOMove(m_FirstPos, 2f);
        }
        else if (Num == 2)
        {
            Debug.Log("SecondPlayer Select");
            transform.DOMove(m_SecondPos, 2f);
        }
        else if (Num == 3)
        {
            Debug.Log("ThirdPlayer Select");
            transform.DOMove(m_ThridPos, 2f);
        }
        else
            return;
    }

    public void CameraMoveReset()
    {
        Debug.Log("BackButton Select");
        transform.DOMove(m_ResetPos, 2f);
        m_Select.m_Selecting = false;
        m_Select.m_CharacterName.text = "Character Select";
        m_Select.m_InputIndex = 0;
    }
}
