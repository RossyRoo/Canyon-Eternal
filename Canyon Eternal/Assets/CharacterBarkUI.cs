using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBarkUI : MonoBehaviour
{
    // 0 Exclamation
    // 1 Love
    // 2 Sad
    // 3 Angry
    // 4 Pause
    // 5 Happy

    public Image barkIcon;

    public IEnumerator DisplayBarkIcon(int barkIconNum)
    {
        barkIcon.gameObject.SetActive(true);
        barkIcon.GetComponent<Animator>().Play("Bark");
        yield return new WaitForSeconds(1f);
        barkIcon.gameObject.SetActive(false);
    }
}
