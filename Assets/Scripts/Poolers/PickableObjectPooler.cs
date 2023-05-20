using System;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectPooler : GenericPooler<PickableObject.PickableObjectType>
{
    void Start()
    {
        Actions.OnPickableObjectDespawn += DespawnObject;
        pooledObjectName = "PickableObject";
        selfName = "PickableObjPooler";
        InitializePooler();
    }
}

public class PickableObjPool : Pool<PickableObject.PickableObjectType>
{
}
