using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformPooler platformPooler;
    public GameObject ground;

    private float simulatedSpawnHeight;
    private float platformSpawnX;
    private float minX = 1f;
    private float maxX = 1.75f;
    private float platformSpawnY;
    private float minY = 2f;
    private float maxY = 2.5f;
    private float nextPlatformSpawnHeightTrigger;
    private float deltaHeightChangeSinceLastSpawn;

    void OnEnable()
    {
        Actions.OnDeltaHeightChanged += ScrollActivePlatforms;
        Actions.OnDeltaHeightChanged += IncreaseDeltaHeightChange;
        simulatedSpawnHeight = 0f;
        platformSpawnX = 0f;
        platformSpawnY = 0f;
        for (int i = 0; i < 10; i++)
        {
            NewRandomSpawnX();
            platformSpawnY += Random.Range(minY, maxY);
            simulatedSpawnHeight += platformSpawnY;
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
        if (deltaHeightChangeSinceLastSpawn <= nextPlatformSpawnHeightTrigger) return;
        deltaHeightChangeSinceLastSpawn = 0;
        NewRandomSpawnX();
        platformSpawnY = simulatedSpawnHeight + nextPlatformSpawnHeightTrigger;
        platformPooler.SpawnPlatform(Platform.PlatformType.Default, new Vector2(platformSpawnX, platformSpawnY));
        nextPlatformSpawnHeightTrigger = Random.Range(minY, maxY);
    }

    private void ScrollActivePlatforms(float deltaHeight)
    {
        //foreach (GameObject obj in platformPooler.GetAllActivePlatforms())
        //    obj.transform.position = new Vector2(obj.transform.position.x, Mathf.Max(0, obj.transform.position.y - deltaHeight));
        if (ground.gameObject.activeSelf)
            ground.transform.position = new Vector2(ground.transform.position.x, Mathf.Max(0, ground.transform.position.y - deltaHeight));
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
