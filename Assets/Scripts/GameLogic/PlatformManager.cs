using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformPooler platformPooler;

    public bool IsSpawningPlatforms { get; private set; } = false;

    [Serializable]
    private class SpawnChanceEntry
    {
        public Platform.PlatformType platformType;
        public int chanceToSpawn;
    }
    [SerializeField] private List<SpawnChanceEntry> spawnChanceTable;

    private float platformSpawnX;
    private float minX = 1f;
    private float maxX = 2f;
    private float platformSpawnY;
    private float minY = 1f;
    private float maxY = 1.75f;
    private float nextPlatformSpawnHeightTrigger;
    private float deltaHeightChangeSinceLastSpawn;

    void Start()
    {
        Actions.OnDeltaHeightChanged += ScrollActivePlatforms;
        Actions.OnDeltaHeightChanged += IncreaseDeltaHeightChange;
        Actions.OnGameLost += platformPooler.DespawnAllActivePlatforms;
    }

    void Update()
    {
        //Difficulty increasing script
        if ((int)GlobalAttributes.TotalGainedHeight + 1 % 25 == 0)
        {
            minX = Mathf.Clamp(minX + 0.05f, 1f, 2f);
            maxX = Mathf.Clamp(maxX + 0.1f, 2f, 5f);
            minY = Mathf.Clamp(minY + 0.05f, 1f, 1.25f);
            maxY = Mathf.Clamp(maxY + 0.1f, 1.75f, 2.25f);
            Debug.Log($"Difficulty increase (TotalGainedHeight: {GlobalAttributes.TotalGainedHeight}):" +
                $"X: ({minX} - {maxX}), Y: ({minY} - {maxY})");
        }

        //New platform spawning script
        if (deltaHeightChangeSinceLastSpawn > nextPlatformSpawnHeightTrigger && IsSpawningPlatforms)
        {
            deltaHeightChangeSinceLastSpawn = 0;
            do
            {
                NewRandomSpawnX();
                platformSpawnY = platformPooler.LastPlatformsPosition.position.y + nextPlatformSpawnHeightTrigger;
                platformPooler.SpawnPlatform(RandomizeNextPlatformType(), new Vector2(platformSpawnX, platformSpawnY));
            } while (platformPooler.LastPlatformsPosition.position.y < GlobalAttributes.UpperScreenEdge);
            nextPlatformSpawnHeightTrigger = UnityEngine.Random.Range(minY, maxY);
        }
    }

    void SpawnInitialSetOfPlatforms()
    {
        platformSpawnX = 0f;
        platformSpawnY = 0f;
        for (int i = 0; i < 10; i++)
        {
            NewRandomSpawnX();
            platformSpawnY += UnityEngine.Random.Range(minY, maxY);
            platformPooler.SpawnPlatform(Platform.PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
        }
        deltaHeightChangeSinceLastSpawn = 0;
        nextPlatformSpawnHeightTrigger = UnityEngine.Random.Range(minY, maxY);
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
        int sumOfAllChances = 0;
        int lowerBound = 0;

        foreach (SpawnChanceEntry entry in spawnChanceTable)
            sumOfAllChances += entry.chanceToSpawn;

        int randomResult = UnityEngine.Random.Range(0, sumOfAllChances - 1);

        foreach (SpawnChanceEntry entry in spawnChanceTable)
        {
            if (lowerBound <= randomResult && randomResult < (lowerBound + entry.chanceToSpawn))
                return entry.platformType;
            lowerBound += entry.chanceToSpawn;
        }

        Debug.LogWarning($"Randomizing the platform type failed! Returning Default type as a fallback...");
        return Platform.PlatformType.Default;
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
        platformSpawnX += UnityEngine.Random.Range(minX, maxX);
        if (platformSpawnX < GlobalAttributes.LeftScreenEdge || GlobalAttributes.RightScreenEdge < platformSpawnX)
            platformSpawnX -= GlobalAttributes.ScreenWorldWidth;
    }
}
