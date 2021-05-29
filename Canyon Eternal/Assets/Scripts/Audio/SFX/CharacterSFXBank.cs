using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Banks/Character SFX")]
public class CharacterSFXBank : AudioBank
{
    [Header("Default Sounds")]
    public AudioClip defaultAttack;

    [Header("Movement")]
    public AudioClip dash;

    [Header("Taking Damage")]
    public AudioClip takeDamage;
    public AudioClip deathRattle;

    [Header("Healing")]
    public AudioClip[] consumeHealItem;

}
