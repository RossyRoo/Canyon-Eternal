using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingWall : MonoBehaviour
{

    //This invisible wall follows an NPC around to block the player.
    //It also follows the player around to block NPCs.

    public Transform followTransform;

    public void Init(Transform targetTrans)
    {
        followTransform = targetTrans;
        transform.position = followTransform.position;
        transform.rotation = followTransform.rotation;
    }


    private void FixedUpdate()
    {
        transform.position = followTransform.position;
        transform.rotation = followTransform.rotation;
    }

    private void LateUpdate()
    {
        if (followTransform == null)
            Destroy(gameObject);
    }
}
