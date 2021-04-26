using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCommonEntities : MonoBehaviour
{
    public static GetCommonEntities instance;

    public LDCharacterController controller;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

}
