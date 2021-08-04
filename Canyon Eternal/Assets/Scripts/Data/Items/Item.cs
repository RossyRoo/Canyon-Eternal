using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : DataObject
{
    [Header("ITEM")]
    public int itemValue;

    public static implicit operator Item(DataSlotUI v)
    {
        throw new NotImplementedException();
    }
}
