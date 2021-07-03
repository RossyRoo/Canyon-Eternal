using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInteger : StateMachineBehaviour
{
    public string integerName;
    public int integerValue;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(integerName, integerValue);
    }
}
