using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : GenericPooler<PlatformType>
{
    public Transform LastPlatformsPosition { get; private set; } = null;

    void Start()
    {
        Actions.OnPlatformDespawn += DespawnObject;
        InitializePooler("Platform", "PlatformPooler");
    }

    public new void SpawnObject(PlatformType platformType, Vector2 position)
    {
        base.SpawnObject(platformType, position);
        LastPlatformsPosition = activeObjects[platformType][^1].transform;
    }
}
