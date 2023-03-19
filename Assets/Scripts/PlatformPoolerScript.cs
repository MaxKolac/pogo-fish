using System.Collections.Generic;
using UnityEngine;

public class PlatformPoolerScript : MonoBehaviour
{
    [SerializeField] private HeightRecorderScript heightRecorder;
    /// <summary>What type of an platform will be held by the pool.</summary>
    [SerializeField] private GameObject gameObjectToPool;
    /// <summary>A list of all instantiated platforms.</summary>
    private List<GameObject> pooledPlatforms;
    /// <summary>Once a platform reaches this Y coordinate, it's teleported upwards and reused.</summary>
    private float platformTeleportBarrier;

    private int poolSize;
    private float lowerScreenBound;
    private float upperScreenBound;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    private float maxWidth;
    private float minWidth;

    // Start is called before the first frame update
    void Start()
    {
        poolSize = 10;
        lowerScreenBound = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        upperScreenBound = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        platformTeleportBarrier = lowerScreenBound - 5f;
        maxHeight = heightRecorder.heightBarrier - 1f;
        minHeight = 2f;
        maxWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        minWidth = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
        pooledPlatforms = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < poolSize; i++)
        {
            temp = Instantiate(gameObjectToPool);
            pooledPlatforms.Add(temp);
        }
        float height = 0;
        foreach (GameObject obj in pooledPlatforms)
        {
            height += Random.Range(minHeight, maxHeight);
            obj.transform.position = new Vector2(Random.Range(minWidth, maxWidth), height);
        }
    }

    private void FixedUpdate()
    {
        //platformTeleportBarrier
        Debug.DrawLine(new Vector2(-10, platformTeleportBarrier), new Vector2(10, platformTeleportBarrier), Color.red);

        foreach (GameObject obj in pooledPlatforms)
        {
            if (obj.transform.position.y < platformTeleportBarrier)
                obj.transform.position = new Vector2(Random.Range(minWidth, maxWidth), upperScreenBound);
        }
    }

    public GameObject GetFirstInactivePooledPlatform()
    {
        for (int i = 0; i < pooledPlatforms.Count; i++)
        {
            if (!pooledPlatforms[i].activeInHierarchy)
                return pooledPlatforms[i];
        }
        return null;
    }

    public List<GameObject> GetActivePooledPlatforms()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject obj in pooledPlatforms)
        {
            if (obj.activeInHierarchy)
                list.Add(obj);
        }
        return list;
    }
}
