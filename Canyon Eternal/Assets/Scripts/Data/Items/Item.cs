using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Generic Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;

    public Sprite itemIcon;

    public int itemValue;

}
