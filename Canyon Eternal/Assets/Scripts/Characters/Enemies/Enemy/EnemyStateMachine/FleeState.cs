using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : EnemyStateMachine
{
    //We go to this state if the player gets too close to the enemy for too long, or if the enemy's health gets low and they can use heal items

    public float leapForceMultiplier = 5000f;

    public override EnemyStateMachine Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector2 targetDirection = enemyManager.currentTarget.transform.position - transform.position;

        Vector2 force = (-targetDirection.normalized * 50f * Time.deltaTime) * leapForceMultiplier;

        enemyManager.rb.AddForce(force);

        //Leap back
        //If your health is good, pursue the target and look for a new attack
        //If your health is low, check to see if you can heal. If not, go for a new attack
        Debug.Log("Do a flee");
        return this;
    }

}
