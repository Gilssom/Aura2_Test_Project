using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoheeCheck : MonoBehaviour
{
    private ChoheeController m_Player;

    private void Awake()
    {
        m_Player = this.GetComponent<ChoheeController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DropItem")
        {
            ItemScript item = other.GetComponent<ItemScript>();
            switch (item.type)
            {
                case ItemScript.Type.Health:
                    if (PlayerStats.Instance.Health >= PlayerStats.Instance.MaxHealth)
                    {
                        return;
                    }
                    if (PlayerStats.Instance.Health < PlayerStats.Instance.MaxHealth)
                    {
                        PlayerStats.Instance.Heal(1);
                        item.ObjectReturn();
                    }
                    break;
                case ItemScript.Type.Soul:
                    SoundManager.Instance.SFXPlay("Soul acquire", AllGameManager.Instance.m_Clip[4]);
                    PlayerStats.Instance.Addsoul();
                    item.ObjectReturn();
                    break;
            }
            //SoundManager.instance.SFXPlay("ItemGet", clip);
        }

        if(other.tag == "FirstTutorial")
        {
            GameManager.Instance.Tutorial(0);
            Destroy(other.gameObject);
        }
        if (other.tag == "SecondTutorial")
        {
            GameManager.Instance.Tutorial(1);
            Destroy(other.gameObject);
        }
        if (other.tag == "ThirdTutorial")
        {
            GameManager.Instance.Tutorial(2);
            Destroy(other.gameObject);
        }
        if (other.tag == "FirstGate")
        {
            GameManager.Instance.ObjectCtrl(1, true);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        if (other.tag == "BossGate")
        {
            m_Player.isLoading = true;
            GameManager.Instance.BossStage();
            Destroy(other.gameObject);
        }

        if(other.tag == "VillagePortal")
        {
            //GameManager.Instance.NextField(1);
            m_Player.isLoading = true;
            FadeInOutManager.Instance.InStartFadeAnim("VillageStage" , 1);
        }

        if (other.tag == "StagePortal")
        {
            //GameManager.Instance.NextField(1);
            m_Player.isLoading = true;
            FadeInOutManager.Instance.InStartFadeAnim("MainStage", 2);
        }

        /*if(other.tag == "GateTrigger")
        {
            UIManager.Instance.SmithySystem(true, "Gate Keeper");
            AllGameManager.Instance.isWeaponShop = true;
            m_Player.MoveStop();
        }*/
    }
}
