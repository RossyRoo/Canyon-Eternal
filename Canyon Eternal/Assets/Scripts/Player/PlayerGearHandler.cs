using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGearHandler : MonoBehaviour
{
    PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();

        if(playerInventory.activeGear != null)
        {
            InitializeGearEffect(playerInventory.activeGear.gearID);
        }
    }

    public void InitializeGearEffect(int gearID)
    {
        if(gearID == 0)
        {
            ActivateGearZero();
        }
        else if(gearID == 1)
        {
            ActivateGearOne();
        }
        else if(gearID == 2)
        {
            ActivateGearTwo();
        }
        else if (gearID == 3)
        {
            ActivateGearThree();
        }
        else if (gearID == 4)
        {
            ActivateGearFour();
        }
        else if (gearID == 5)
        {
            ActivateGearFive();
        }
        else if (gearID == 6)
        {
            ActivateGearSix();
        }
    }

    #region Gear Effects

    private void ActivateGearZero()
    {
        //Increase player speed
        Debug.Log("Activating gear zero");
    }

    private void ActivateGearOne()
    {
        //Increase player crit chance
        Debug.Log("Activating gear one");
    }

    private void ActivateGearTwo()
    {
        //Increase base power
        Debug.Log("Activating gear two");
    }

    private void ActivateGearThree()
    {
        //Decrease enemy awareness
        //Debug.Log("Activating gear three");
    }

    private void ActivateGearFour()
    {
        //Increase resistance
        Debug.Log("Activating gear four");
    }

    private void ActivateGearFive()
    {
        //Increase fragments earned
        Debug.Log("Activating gear five");
    }

    private void ActivateGearSix()
    {
        //Decrease enemy health
        Debug.Log("Activating gear six");
    }

    #endregion
}
