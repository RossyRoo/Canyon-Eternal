using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Contact")]
public class Contact : Item
{
    public List<GameObject> incomingPhoneCalls;
    public List<GameObject> outgoingPhoneCalls;
}
