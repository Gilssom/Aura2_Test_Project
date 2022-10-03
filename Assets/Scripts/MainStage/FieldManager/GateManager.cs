using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public GameObject[] m_Portal;

    public void GateOpen(int GateNumber)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == GateNumber)
            {
                m_Portal[i].SetActive(true);
                UIManager.Instance.SmithySystem(false, "Gate Keeper");
                AllGameManager.Instance.isWeaponShop = false;
            }
            else
                break;
        }
    }
}
