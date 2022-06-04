using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private ChoheeController m_Player;
    private NearItemCheck m_Item;

    public Text m_NearNameText;

    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<ChoheeController>();
        m_Item = GameObject.FindWithTag("Player").GetComponent<NearItemCheck>();
    }

    void Update()
    {
        ItemInfo();
    }

    void ItemInfo()
    {
        if (m_Item.m_NearItem)
        {
            if (m_Item.m_NearItem.name == "FireMask_Item")
                m_NearNameText.text = "∏Ò¡ﬂ ≈ª »πµÊ«œ±‚" + "<color=#FFE400>" + " (E)" + "</color>";
        }
        else
            m_NearNameText.text = null;
    }
}
