using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrderSystem : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer renderer in renderers)
        {
            Debug.Log(renderer.gameObject.name);
        }
    }
    private void Update()
    {
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();

        foreach(SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }
    }
}
