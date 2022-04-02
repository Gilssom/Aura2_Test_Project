using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimelineEvent : MonoBehaviour
{
    public Light m_ShiningNpc;
    public Light m_ShiningHelpers;

    public void NpcShining()
    {
        m_ShiningNpc.DOIntensity(50, 2.5f);
    }

    public void HeplersShining()
    {
        m_ShiningHelpers.DOIntensity(30, 2f);
    }
}
