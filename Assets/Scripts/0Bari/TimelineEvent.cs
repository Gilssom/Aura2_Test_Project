using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimelineEvent : MonoBehaviour
{
    public BariController m_Player;
    public GameObject m_UICanvas;
    public Light m_ShiningNpc;
    public Light m_ShiningHelpers;
    public ParticleSystem m_YellowEffect;
    public Camera m_CineCam;

    public void CinemachineOn()
    {
        m_Player.scanObject = null;
        m_UICanvas.SetActive(false);
        AT_GameManager.Instance.isAction = true;
    }

    public void NpcShining()
    {
        m_ShiningNpc.DOIntensity(50, 2.5f);
    }

    public void HeplersShining()
    {
        m_ShiningHelpers.DOIntensity(30, 2f);
    }

    public void YellowEff()
    {
        m_YellowEffect.Play();
    }

    public void CinemachineOff()
    {
        m_UICanvas.SetActive(true);
        AT_GameManager.Instance.isAction = false;
        m_CineCam.gameObject.SetActive(false);
    }
}
