using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Generic")]
public class DataObject : ScriptableObject
{
    public string dataName;
    [TextArea] public string dataDescription;

    public Sprite dataIcon;


}
