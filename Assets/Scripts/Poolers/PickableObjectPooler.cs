using System;
using System.Collections.Generic;
using UnityEngine;

public class PickableObjectPooler : GenericPooler<PickableObjectType>
{
    protected override void Start()
    {
        Actions.OnPickableObjectDespawn += DespawnObject;
        InitializePooler("PickableObject", "PickableObjPooler");
    }

    protected override void OnDestroy()
    {
        Actions.OnPickableObjectDespawn -= DespawnObject;
        DespawnAllActiveObjects();
    }
}
