using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimelineEvent : MonoBehaviour
{
    public BariController m_Player;
    public GameObject m_UICanvas;
    public Light m_ShiningNpc;
    public Camera m_CineCam;
    public Material m_EnemyMT;
    public GameObject m_LightNpc;
    public GameObject m_TutorialPanel;
    public GameObject m_FieldMonsters;

    public AudioClip[] m_clip;

    bool EnemyDeath;
    float Cutoff;

    private void Update()
    {
        if(EnemyDeath)
        {
            Cutoff += 0.007f;

            if (Cutoff != 1)
            {
                m_EnemyMT.SetFloat("_Dissolve", Cutoff);
                return;
            }
        }
    }

    public void CinemachineOn()
    {
        m_EnemyMT.SetFloat("_Dissolve", 0);
        m_LightNpc.transform.Rotate(new Vector3(0, 0, 0));
        m_Player.scanObject = null;
        m_UICanvas.SetActive(false);
        AT_GameManager.Instance.isAction = true;
    }

    public void NpcShining()
    {
        m_ShiningNpc.DOIntensity(50, 2.5f);
    }

    public void EnemyVoice()
    {
        SoundManager.Instance.SFXPlay("Enemy Voice", m_clip[0]);
    }

    public void EnemyDissolve()
    {
        EnemyDeath = true;
    }

    public void CinemachineOff()
    {
        m_TutorialPanel.SetActive(true);
        m_FieldMonsters.SetActive(true);
        m_UICanvas.SetActive(true);
        AT_GameManager.Instance.isAction = false;
        m_CineCam.gameObject.SetActive(false);

        if (m_TutorialPanel.activeInHierarchy)
        {
            TutorialManager.Instance.InStartFadeAnim(1);
        }
    }
}
