using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> objectsToDestroy = new List<GameObject>();


    public void OnLoadScene()
    {
        for (int i = 0; i < objectsToDestroy.Count; i++)
        {
            Debug.Log("Destroying " + objectsToDestroy[i]);
            Destroy(objectsToDestroy[i]);
        }
        objectsToDestroy.Clear();
    }

    public void AddToObjectPool(GameObject objectToAdd, bool isPersisting)
    {
        objectToAdd.transform.parent = this.gameObject.transform;

        if (!isPersisting)
        {
            objectsToDestroy.Add(objectToAdd);
        }
    }

}
