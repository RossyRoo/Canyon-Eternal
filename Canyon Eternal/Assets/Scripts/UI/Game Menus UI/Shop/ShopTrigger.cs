using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public string shopkeeperName;
    public List<Item> shopInventory;
    public GameObject shopkeeperGoodbye;

    public void TriggerOpenShop()
    {
        FindObjectOfType<ShopUI>().OpenShop(shopkeeperName, shopInventory, this.gameObject);
    }
}
