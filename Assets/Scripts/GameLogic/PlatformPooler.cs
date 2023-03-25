using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private GameObject pooledGameObjectType;
    [SerializeField] private Ground ground;

    private List<GameObject> pooledPlatforms;
    private int poolSize = 20;
    private float maxHeight = 4f;
    private float minHeight = 2f;
    private float platformSpawnHeight = 0f;

    void OnEnable()
    {
        InitializeNewPool();
    }

    public void InitializeNewPool()
    {
        pooledPlatforms = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject temp = Instantiate(pooledGameObjectType);
            pooledPlatforms.Add(temp);
        }
        foreach (GameObject obj in pooledPlatforms)
        {
            platformSpawnHeight += Random.Range(minHeight, maxHeight);
            obj.transform.position = new Vector2(Random.Range(globalAttributes.LeftScreenEdge, globalAttributes.RightScreenEdge), platformSpawnHeight);
        }
    }

    void Update()
    {
        foreach (GameObject obj in pooledPlatforms)
        {
            if (obj.transform.position.y < globalAttributes.LowerScreenEdge)
                RespawnPlatform(obj);
        }
    }

    public List<GameObject> GetAllPlatforms()
    {
        return pooledPlatforms;
    }

    public List<GameObject> GetActivePlatforms()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject obj in pooledPlatforms)
            if (obj.activeInHierarchy) list.Add(obj);
        return list;
    }

    void RespawnPlatform(GameObject platform)
    {
        platform.transform.position = new Vector2(Random.Range(globalAttributes.LeftScreenEdge, globalAttributes.RightScreenEdge), platformSpawnHeight);
        platform.SetActive(true);
    }

    public void ScrollPlatformsDown(float deltaHeight)
    {
        foreach (GameObject obj in pooledPlatforms)
            obj.transform.position = new Vector2(obj.transform.position.x, Mathf.Max(0, obj.transform.position.y - deltaHeight));
        if (ground.isActiveAndEnabled)
            ground.transform.position = new Vector2(ground.transform.position.x, Mathf.Max(0, ground.transform.position.y - deltaHeight));
    }
}
