using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MazeController : MonoBehaviour
{
    public enum Type { Hall, SpinWall, MoveWall, StopSpinWall, StopTurn }
    public Type m_MazeObjectType;

    public float m_MoveSpeed;
    public float m_RotateSpeed;

    Vector3 m_CurPos;
    public float m_Zdelta; // 좌우로 이동 가능한 최대값
    public float m_Xdelta;
    public bool isXmoving;
    public bool isMinusZWall;

    public bool isStopTurn;

    [SerializeField]
    Transform[] m_TurnPos;

    int m_TurnNum = 0;

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
                var IceStone = transform.Find("MazeIceStone").GetComponent<Transform>();
                var IceStone_2 = transform.Find("MazeIceStone_2").GetComponent<Transform>();
                Vector3 v = m_CurPos;
                IceStone.transform.Rotate(new Vector3(0, 360, 0), m_RotateSpeed * Time.deltaTime);
                IceStone_2.transform.Rotate(new Vector3(0, 360, 0), m_RotateSpeed * Time.deltaTime);
                if (isXmoving)
                {
                    v.x += m_Xdelta * Mathf.Sin(Time.time * m_MoveSpeed);
                }
                else if(isMinusZWall)
                {
                    m_MoveSpeed = -1.3f;
                    v.z += m_Zdelta * Mathf.Sin(Time.time * m_MoveSpeed);
                }             
                else
                {
                    m_MoveSpeed = 1.3f;
                    v.z += m_Zdelta * Mathf.Sin(Time.time * m_MoveSpeed);
                }
                transform.position = v;
                break;
            case Type.StopSpinWall:
                transform.Rotate(new Vector3(0, 360, 0), m_MoveSpeed * Time.deltaTime);
                break;
            case Type.StopTurn:
                transform.position = Vector3.MoveTowards
                    (transform.position, m_TurnPos[m_TurnNum].transform.position, m_MoveSpeed * Time.deltaTime);

                if (transform.position == m_TurnPos[m_TurnNum].transform.position)
                    m_TurnNum++;

                if (m_TurnNum == m_TurnPos.Length)
                    m_TurnNum = 0;
                break;
        }   
    }

    IEnumerator StopSpinWall()
    {
        isStop = true;
        m_MoveSpeed = 0;
        yield return new WaitForSeconds(1);
        m_MoveSpeed = 40;
        yield return new WaitForSeconds(0.3f);
        isStop = false;
        yield return null;
    }
}
