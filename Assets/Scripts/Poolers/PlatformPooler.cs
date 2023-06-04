using UnityEngine;

public class PlatformPooler : GenericPooler<PlatformType, Platform>
{
    public Transform LastPlatformsPosition { get; private set; } = null;

    protected override void Awake()
    {
        base.Awake();
        Actions.OnPlatformDespawn += DespawnObject;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Actions.OnPlatformDespawn -= DespawnObject;
    }

    public new void SpawnObject(PlatformType platformType, Vector2 position)
    {
        base.SpawnObject(platformType, position);
        LastPlatformsPosition = activeObjects[platformType][^1].transform;
    }
}
