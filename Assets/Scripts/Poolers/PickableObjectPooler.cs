using System;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectPooler : GenericPooler<PickableObjectType>
{
    void Start()
    {
        Actions.OnPickableObjectDespawn += DespawnObject;
        InitializePooler("PickableObject", "PickableObjPooler");
    }
}
