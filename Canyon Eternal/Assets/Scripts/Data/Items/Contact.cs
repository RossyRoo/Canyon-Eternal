using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Contact")]
public class Contact : DataObject
{
    public List<GameObject> incomingPhoneCalls;
    public List<GameObject> outgoingPhoneCalls;
}
