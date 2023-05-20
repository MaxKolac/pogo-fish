using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : GenericPooler<PlatformType>
{
    public Transform LastPlatformsPosition { get; private set; } = null;

    void Start()
    {
        Actions.OnPlatformDespawn += DespawnObject;
        pooledObjectName = "Platform";
        selfName = "PlatformPooler";
        InitializePooler();
    }

    public override void SpawnObject(Platform.PlatformType platformType, Vector2 position)
    {
        base.SpawnObject(platformType, position);
        LastPlatformsPosition = activeObjects[platformType][activeObjects.Count - 1].transform;
    }
}
