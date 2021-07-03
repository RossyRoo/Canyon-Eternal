using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingPhoneCallTrigger : MonoBehaviour
{
    public float callChance = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerProgression playerProgression = collision.GetComponentInChildren<PlayerProgression>();

        if(playerProgression != null)
        {
            TriggerIncomingPhoneCall(playerProgression);
        }
    }

    public void TriggerIncomingPhoneCall(PlayerProgression playerProgression)
    {
        if(Random.value <= callChance) //Determine whether you will get a call
        {
            //Select a random contact
            Contact contactToCall = playerProgression.collectedContacts[Random.Range(0, playerProgression.collectedContacts.Count)];

            //Determine which phone calls are possible for that contact
            List<GameObject> possiblePhoneCalls = new List<GameObject>();
            for (int i = 0; i < contactToCall.incomingPhoneCalls.Count; i++)
            {
                if(playerProgression.playerVesselPercentage >= contactToCall.incomingPhoneCalls[i].GetComponent<PhoneCall>().minVesselPercentage
                    && playerProgression.playerVesselPercentage <= contactToCall.incomingPhoneCalls[i].GetComponent<PhoneCall>().maxVesselPercentage
                    && playerProgression.collectedEnemyIDs.Contains(contactToCall.incomingPhoneCalls[i].GetComponent<PhoneCall>().bossIDRequirement)
                    && !playerProgression.collectedPhoneCallIDs.Contains(contactToCall.incomingPhoneCalls[i].GetComponent<PhoneCall>().phoneCallID))
                {
                    possiblePhoneCalls.Add(contactToCall.incomingPhoneCalls[i]);
                }
            }

            //Pick a random possible call and remove it from the contacts phone calls

            if(possiblePhoneCalls.Count > 0)
            {
                int randomPhoneCallNum = Random.Range(0, possiblePhoneCalls.Count);

                playerProgression.collectedPhoneCallIDs.Add(possiblePhoneCalls[randomPhoneCallNum].GetComponent<PhoneCall>().phoneCallID);

                playerProgression.GetComponentInParent<PlayerManager>().EnterConversationState();
                Instantiate(possiblePhoneCalls[randomPhoneCallNum], transform.position, Quaternion.identity);
            }

        }

        Destroy(gameObject);
    }
}
