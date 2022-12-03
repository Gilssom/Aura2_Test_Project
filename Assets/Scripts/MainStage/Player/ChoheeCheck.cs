using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoheeCheck : MonoBehaviour
{
    private ChoheeController m_Player;

    [SerializeField]
    int m_CurCheckPoint = 0;

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

        if(other.tag == "CheckPoint")
        {
            if(m_Player.isChecking[m_CurCheckPoint] == false)
            {
                m_Player.m_Checkpoint[m_CurCheckPoint] = other.gameObject;
                m_CurCheckPoint++;
                other.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_Player.m_Checkpoint.Length; i++)
            {
                if(m_Player.m_Checkpoint[i] == other.gameObject)
                {
                    m_Player.isChecking[i] = true;
                }
            }
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

        if(other.tag == "VillagePortal") // 조형제 보스 처치 하고 스테이지 종료 시점
        {
            //GameManager.Instance.NextField(1);
            //m_Player.isLoading = true;
            //FadeInOutManager.Instance.InStartFadeAnim("VillageStage" , 1);
            m_Player.isGameEnd = true;
            UIManager.Instance.TestGameOver();
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
