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
    float m_Zdelta = 5.0f; // 좌우로 이동 가능한 최대값
    float m_Xdelta = 18.0f;
    public bool isXmoving;

    void Start()
    {
        m_CurPos = transform.position;
    }

    void Update()
    {
        MazeObjController();
    }

    void MazeObjController()
    {
        switch (m_MazeObjectType)
        {
            case Type.Hall:
                break;
            case Type.SpinWall:
                //transform.DORotate(new Vector3(0, -360, 0), m_MoveSpeed, RotateMode.FastBeyond360).
                //    SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear); // 360도로 무한회전
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
                StartCoroutine(StopSpinWall());
            break;
        }   
    }

    IEnumerator StopSpinWall()
    {
        transform.DORotate(new Vector3(0, 0, 360), m_MoveSpeed, RotateMode.FastBeyond360);
        yield return null;
    }
}
