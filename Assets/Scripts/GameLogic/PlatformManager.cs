using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformPooler platformPooler;

    public bool IsSpawningPlatforms { get; private set; } = false;

    private float platformSpawnX;
    private const float minX = 1f;
    private const float maxX = 1.75f;
    private float platformSpawnY;
    private const float minY = 2f;
    private const float maxY = 2.5f;
    private Dictionary<Platform.PlatformType, int> initialPlatformTypeSpawnChances;
    private float nextPlatformSpawnHeightTrigger;
    private float deltaHeightChangeSinceLastSpawn;

    void Start()
    {
        Actions.OnDeltaHeightChanged += ScrollActivePlatforms;
        Actions.OnDeltaHeightChanged += IncreaseDeltaHeightChange;
        Actions.OnGameLost += platformPooler.DespawnAllActivePlatforms;
        initialPlatformTypeSpawnChances = new()
        {
            { Platform.PlatformType.Default, 75 },
            { Platform.PlatformType.OneJump, 25 }
        };
    }

    void Update()
    {
        if (deltaHeightChangeSinceLastSpawn <= nextPlatformSpawnHeightTrigger || !IsSpawningPlatforms) return;
        deltaHeightChangeSinceLastSpawn = 0;

        NewRandomSpawnX();
        platformSpawnY = platformPooler.LastPlatformsPosition.position.y + nextPlatformSpawnHeightTrigger;

        platformPooler.SpawnPlatform(Platform.PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
        nextPlatformSpawnHeightTrigger = Random.Range(minY, maxY);
    }

    void SpawnInitialSetOfPlatforms()
    {
        platformSpawnX = 0f;
        platformSpawnY = 0f;
        for (int i = 0; i < 10; i++)
        {
            NewRandomSpawnX();
            platformSpawnY += Random.Range(minY, maxY);
            platformPooler.SpawnPlatform(Platform.PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
        }
        deltaHeightChangeSinceLastSpawn = 0;
        nextPlatformSpawnHeightTrigger = Random.Range(minY, maxY);
    }

    public void EnablePlatformSpawning()
    {
        IsSpawningPlatforms = true;
        SpawnInitialSetOfPlatforms();
    }

    public void DisablePlatformSpawning()
    {
        IsSpawningPlatforms = false;
        platformPooler.DespawnAllActivePlatforms();
    }

    private Platform.PlatformType RandomizeNextPlatformType()
    {
        //TODO
        int lowerBound = 0;
        foreach 
        int randomResult = Random.Range(0, 100);
        for (int i = 0; i < initialPlatformTypeSpawnChances.Count - 1; i++)
        {
            if (lowerBound <= randomResult && randomResult < initialPlatformTypeSpawnChances.ToArray()[i])
        }
    }

    private void ScrollActivePlatforms(float deltaHeight)
    {
        foreach (GameObject activePlatform in platformPooler.GetAllActivePlatforms())
        {
            activePlatform.transform.position =
                new Vector2(
                    activePlatform.transform.position.x,
                    activePlatform.transform.position.y - deltaHeight
                );
        }
    }

    private void IncreaseDeltaHeightChange(float deltaHeight)
    {
        if (!IsSpawningPlatforms) return;
        deltaHeightChangeSinceLastSpawn += deltaHeight;
    }

    private void NewRandomSpawnX()
    {
        platformSpawnX += Random.Range(minX, maxX);
        if (platformSpawnX < GlobalAttributes.LeftScreenEdge || GlobalAttributes.RightScreenEdge < platformSpawnX)
        {
            platformSpawnX -= GlobalAttributes.ScreenWorldWidth;
        }
    }
}
