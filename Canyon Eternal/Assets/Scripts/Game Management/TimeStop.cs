using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Class Description

/* Lives under the "DONT DESTROY" game object and is transferred between scenes
 * Controls stop time effects for pausing the game, checking inventory, or getting hit
 */

#endregion

public class TimeStop : MonoBehaviour
{
    public float speed;
    public bool restoreTime;

    private void Start()
    {
        restoreTime = false;
    }

    private void Update()
    {
        if(restoreTime)
        {
            if(Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        Debug.Log("Stopping time");
        speed = restoreSpeed;

        if(delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }

        Time.timeScale = changeTime;
    }

    IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        restoreTime = true;
    }
}
