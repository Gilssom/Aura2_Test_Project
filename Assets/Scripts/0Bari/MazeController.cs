using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MazeController : MonoBehaviour
{
    public enum Type { Hall, SpinWall, MoveWall, StopSpinWall }
    public Type m_MazeObjectType;

    public float m_MoveSpeed;

    Vector3 m_CurPos;
    float m_Zdelta = 7; // 좌우로 이동 가능한 최대값
    float m_Xdelta = 18;
    public bool isXmoving;

    public bool isStop;

    public bool isStopSpin;

    void Start()
    {
        m_CurPos = transform.position;
    }

    void Update()
    {
        MazeObjController();

        int Angles = (int)transform.rotation.eulerAngles.y;

        if(isStopSpin)
        {
            if (Angles % 90 == 0 && !isStop)
            {
                StartCoroutine(StopSpinWall());
            }
        }
    }

    void MazeObjController()
    {
        switch (m_MazeObjectType)
        {
            case Type.Hall:
                break;
            case Type.SpinWall:
                transform.Rotate(new Vector3(0, 360, 0), m_MoveSpeed * Time.deltaTime);
                break;
            case Type.MoveWall:
                Vector3 v = m_CurPos;
                if(isXmoving)
                {
                    m_MoveSpeed = 1;
                    v.x += m_Xdelta * Mathf.Sin(Time.time * m_MoveSpeed);
                }
                else
                {
                    m_MoveSpeed = 2.3f;
                    v.z += m_Zdelta * Mathf.Sin(Time.time * m_MoveSpeed);
                }             
                transform.position = v;
                break;
            case Type.StopSpinWall:
                transform.Rotate(new Vector3(0, 360, 0), m_MoveSpeed * Time.deltaTime);
                break;
        }   
    }

    IEnumerator StopSpinWall()
    {
        Debug.Log("Check");
        isStop = true;
        m_MoveSpeed = 0;
        yield return new WaitForSeconds(1);
        m_MoveSpeed = 40;
        yield return new WaitForSeconds(0.3f);
        isStop = false;
        yield return null;
    }
}
