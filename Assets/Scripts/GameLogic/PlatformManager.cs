using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformPooler platformPooler;

    public bool IsSpawningPlatforms { get; private set; } = false;

    private float platformSpawnX;
    private const float minX = 1f;
    private const float maxX = 5f;
    private float platformSpawnY;
    private const float minY = 1.25f;
    private const float maxY = 3.5f;
    private float nextPlatformSpawnHeightTrigger;
    private float deltaHeightChangeSinceLastSpawn;

    /// <summary>
    /// <list type="bullet">
    /// <item><c>int[0]</c> - Platform.PlatformType enumerator as an integer.</item>
    /// <item><c>int[1]</c> - Chance's weight for this type to be returned.</item>
    /// </list>
    /// </summary>
    private List<int[]> platformTypeSpawnChances;

    void Start()
    {
        Actions.OnDeltaHeightChanged += ScrollActivePlatforms;
        Actions.OnDeltaHeightChanged += IncreaseDeltaHeightChange;
        Actions.OnGameLost += platformPooler.DespawnAllActivePlatforms;
        platformTypeSpawnChances = new List<int[]>()
        {
            { new int[]{ 0, 75 } },
            { new int[]{ 1, 25 } },
        };

    }

    void Update()
    {
        if (deltaHeightChangeSinceLastSpawn <= nextPlatformSpawnHeightTrigger || !IsSpawningPlatforms) return;
        deltaHeightChangeSinceLastSpawn = 0;

        NewRandomSpawnX();
        platformSpawnY = platformPooler.LastPlatformsPosition.position.y + nextPlatformSpawnHeightTrigger;

        platformPooler.SpawnPlatform(RandomizeNextPlatformType(), new Vector2(platformSpawnX, platformSpawnY));
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
        int sumOfAllChances = 0;
        int lowerBound = 0;

        for (int i = 0; i < platformTypeSpawnChances.Count; i++)
            sumOfAllChances += platformTypeSpawnChances[i][1];
        
        int randomResult = Random.Range(0, sumOfAllChances - 1);

        for (int i = 0; i < platformTypeSpawnChances.Count; i++)
        {
            if (lowerBound <= randomResult && randomResult < (lowerBound + platformTypeSpawnChances[i][1]))
                return (Platform.PlatformType)platformTypeSpawnChances[i][0];
            lowerBound += platformTypeSpawnChances[i][1];
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
        platformSpawnX += Random.Range(minX, maxX);
        if (platformSpawnX < GlobalAttributes.LeftScreenEdge || GlobalAttributes.RightScreenEdge < platformSpawnX)
            platformSpawnX -= GlobalAttributes.ScreenWorldWidth;
    }
}
