﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("CHARACTER DATA CARD")]
    public CharacterData characterData;

    [HideInInspector]public float currentHealth;

    public bool isPoisoned;
    public bool isFrozen;
    public bool isHypnotized;
}
