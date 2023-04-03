using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformPooler platformPooler;

    //private float simulatedSpawnHeight;
    private float platformSpawnX;
    private float minX = 1f;
    private float maxX = 1.75f;
    private float platformSpawnY;
    private float minY = 2f;
    private float maxY = 2.5f;
    private float nextPlatformSpawnHeightTrigger;
    private float deltaHeightChangeSinceLastSpawn;

    void Start()
    {
        Actions.OnDeltaHeightChanged += ScrollActivePlatforms;
        Actions.OnDeltaHeightChanged += IncreaseDeltaHeightChange;
        //simulatedSpawnHeight = 0f;
        platformSpawnX = 0f;
        platformSpawnY = 0f;
        for (int i = 0; i < 10; i++)
        {
            NewRandomSpawnX();
            //float newRandomValue = Random.Range(minY, maxY);
            //platformSpawnY += newRandomValue;
            //simulatedSpawnHeight += newRandomValue;
            platformSpawnY += Random.Range(minY, maxY);
            platformPooler.SpawnPlatform(Platform.PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
        }
        deltaHeightChangeSinceLastSpawn = 0;
        nextPlatformSpawnHeightTrigger = Random.Range(minY, maxY);
    }

    void OnDisable()
    {
        Actions.OnDeltaHeightChanged -= ScrollActivePlatforms;
        Actions.OnDeltaHeightChanged -= IncreaseDeltaHeightChange;
    }

    void Update()
    {
        if (deltaHeightChangeSinceLastSpawn <= nextPlatformSpawnHeightTrigger)
        {
            return;
        }
        deltaHeightChangeSinceLastSpawn = 0;
        NewRandomSpawnX();
        platformSpawnY = platformPooler.LastPlatformsPosition.position.y + nextPlatformSpawnHeightTrigger;
        platformPooler.SpawnPlatform(Platform.PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
        nextPlatformSpawnHeightTrigger = Random.Range(minY, maxY);
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

    private void IncreaseDeltaHeightChange(float deltaHeight) => deltaHeightChangeSinceLastSpawn += deltaHeight;

    private void NewRandomSpawnX()
    {
        platformSpawnX += Random.Range(minX, maxX);
        if (platformSpawnX < GlobalAttributes.LeftScreenEdge || GlobalAttributes.RightScreenEdge < platformSpawnX)
        {
            platformSpawnX -= GlobalAttributes.ScreenWorldWidth;
        }
    }
}
