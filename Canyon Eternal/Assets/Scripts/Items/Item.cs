using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;

    public Sprite itemIcon;

    public int itemValue;
}
