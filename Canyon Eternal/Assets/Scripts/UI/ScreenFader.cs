using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{

    public void OnLoadScene()
    {
        FadeFromBlack();
    }

    private void FadeFromBlack()
    {
        GetComponent<Animator>().Play("FadeFromBlack");
    }

    public void FadeToBlack()
    {
        GetComponent<Animator>().Play("FadeToBlack");
    }
}