using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivateChildren : MonoBehaviour
{
    public List<GameObject> objectsToReactivate;
    // Start is called before the first frame update
    public void Reactivate()
    {
        foreach(var o in objectsToReactivate)
        {
            o.SetActive(true);
        }
    }
}
