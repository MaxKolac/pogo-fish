using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private GameObject pooledGameObjectType;

    private List<GameObject> pooledPlatforms;
    private int poolSize;
    private float maxHeight;
    private float minHeight;
    private float maxWidth;
    private float minWidth;

    void Start()
    {
        poolSize = 10;
        maxHeight = globalAttributes.heightBarrier - 1f;
        minHeight = 2f;
        maxWidth = globalAttributes.rightScreenEdge;
        minWidth = globalAttributes.leftScreenEdge;

        GameObject temp;
        pooledPlatforms = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            temp = Instantiate(pooledGameObjectType);
            pooledPlatforms.Add(temp);
        }
        float height = 0;
        foreach (GameObject obj in pooledPlatforms)
        {
            height += Random.Range(minHeight, maxHeight);
            obj.transform.position = new Vector2(Random.Range(minWidth, maxWidth), height);
        }
    }

    void Update()
    {
        foreach (GameObject obj in pooledPlatforms)
        {
            if (obj.transform.position.y < globalAttributes.platformTeleportBarrier)
                obj.transform.position = new Vector2(Random.Range(minWidth, maxWidth), globalAttributes.upperScreenEdge);
        }
    }

    public void ScrollPooledPlatformsDown(float deltaHeight)
    {
        foreach (GameObject obj in pooledPlatforms)
        {
            if (obj.activeInHierarchy)
                obj.transform.position = new Vector2(obj.transform.position.x, Mathf.Max(0, obj.transform.position.y - deltaHeight));
        }
    }
}
