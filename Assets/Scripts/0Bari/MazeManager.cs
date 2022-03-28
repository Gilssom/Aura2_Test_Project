using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public GameObject[] m_Stage;

    private static MazeManager m_instance;
    // ΩÃ±€≈Ê
    public static MazeManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(MazeManager)) as MazeManager;

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
    }

    public void StageCtrl(int StageNum)
    {
        if(StageNum == 0)
        {
            m_Stage[StageNum].SetActive(true);
        }
        else if(StageNum != 0)
        {
            m_Stage[StageNum - 1].SetActive(false);
            m_Stage[StageNum].SetActive(true);
        }
    }
}
