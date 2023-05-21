using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    //Poolers
    [SerializeField] private PlatformPooler platformPooler;
    [SerializeField] private PickableObjectPooler pickableObjectPooler;

    [SerializeField] private int platformWithObjectSpawnChance = 100; //[0, 100)
    [SerializeField] private List<SpawnChanceEntry<PlatformType>> platformSpawnChanceTable;
    [SerializeField] private List<SpawnChanceEntry<PickableObjectType>> pickableObjSpawnChanceTable;

    public bool IsSpawningPlatforms { get; private set; } = false;

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
        Actions.OnDeltaHeightChanged += ScrollActivePooledObjects;
        Actions.OnDeltaHeightChanged += CaptureDeltaHeightChange;
        Actions.OnGameLost += platformPooler.DespawnAllActiveObjects;
        Actions.OnGameLost += pickableObjectPooler.DespawnAllActiveObjects;
    }

    void OnDestroy()
    {
        Actions.OnDeltaHeightChanged -= ScrollActivePooledObjects;
        Actions.OnDeltaHeightChanged -= CaptureDeltaHeightChange;
        Actions.OnGameLost -= platformPooler.DespawnAllActiveObjects;
        Actions.OnGameLost -= pickableObjectPooler.DespawnAllActiveObjects;
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
                platformPooler.SpawnObject(RandomizeNextPlatformType(), new Vector2(platformSpawnX, platformSpawnY));
                if (UnityEngine.Random.Range(0, 100) < platformWithObjectSpawnChance)
                {
                    pickableObjectPooler.SpawnObject(
                        RandomizeNextPickableObjectType(),
                        new Vector2(
                            platformPooler.LastPlatformsPosition.position.x,
                            platformPooler.LastPlatformsPosition.position.y + 0.45f
                        )
                    );
                }
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
            platformPooler.SpawnObject(PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
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
        platformPooler.DespawnAllActiveObjects();
    }

    private PlatformType RandomizeNextPlatformType()
    {
        int sumOfAllChances = 0;
        int lowerBound = 0;

        foreach (SpawnChanceEntry<PlatformType> entry in platformSpawnChanceTable)
            sumOfAllChances += entry.chanceToSpawn;

        int randomResult = UnityEngine.Random.Range(0, sumOfAllChances - 1);

        foreach (SpawnChanceEntry<PlatformType> entry in platformSpawnChanceTable)
        {
            if (lowerBound <= randomResult && randomResult < (lowerBound + entry.chanceToSpawn))
                return entry.objectType;
            lowerBound += entry.chanceToSpawn;
        }

        Debug.LogWarning($"Randomizing the platform type failed! Returning Default type as a fallback...");
        return PlatformType.Default;
    }

    private PickableObjectType RandomizeNextPickableObjectType()
    {
        int sumOfAllChances = 0;
        int lowerBound = 0;

        foreach (SpawnChanceEntry<PickableObjectType> entry in pickableObjSpawnChanceTable)
            sumOfAllChances += entry.chanceToSpawn;

        int randomResult = UnityEngine.Random.Range(0, sumOfAllChances - 1);

        foreach (SpawnChanceEntry<PickableObjectType> entry in pickableObjSpawnChanceTable)
        {
            if (lowerBound <= randomResult && randomResult < (lowerBound + entry.chanceToSpawn))
                return entry.objectType;
            lowerBound += entry.chanceToSpawn;
        }

        Debug.LogWarning($"Randomizing the pickable object type failed! Returning Coin type as a fallback...");
        return PickableObjectType.Coin;
    }

    private void ScrollActivePooledObjects(float deltaHeight)
    {
        foreach (GameObject activePlatform in platformPooler.GetAllActiveObjects())
        {
            activePlatform.transform.position =
                new Vector2(
                    activePlatform.transform.position.x,
                    activePlatform.transform.position.y - deltaHeight
                );
        }
        foreach (GameObject activePickableObject in pickableObjectPooler.GetAllActiveObjects())
        {
            activePickableObject.transform.position =
                new Vector2(
                    activePickableObject.transform.position.x,
                    activePickableObject.transform.position.y - deltaHeight
                );
        }
    }

    private void CaptureDeltaHeightChange(float deltaHeight)
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

[Serializable]
internal class SpawnChanceEntry<T> where T : Enum
{
    public T objectType;
    public int chanceToSpawn;
}
