using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoheeCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
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
                        Destroy(other.gameObject);
                    }
                    break;
                case ItemScript.Type.Soul:
                    PlayerStats.Instance.Addsoul();
                    Destroy(other.gameObject);
                    break;
            }
            //SoundManager.instance.SFXPlay("ItemGet", clip);
        }
    }
}
