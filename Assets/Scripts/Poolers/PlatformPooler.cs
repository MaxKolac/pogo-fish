using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : GenericPooler<PlatformType>
{
    public Transform LastPlatformsPosition { get; private set; } = null;

    protected override void Start()
    {
        Actions.OnPlatformDespawn += DespawnObject;
        InitializePooler("Platform", "PlatformPooler");
    }

    protected override void OnDestroy()
    {
        Actions.OnPlatformDespawn -= DespawnObject;
        DespawnAllActiveObjects();
    }

    public new void SpawnObject(PlatformType platformType, Vector2 position)
    {
        base.SpawnObject(platformType, position);
        LastPlatformsPosition = activeObjects[platformType][^1].transform;
    }
}
