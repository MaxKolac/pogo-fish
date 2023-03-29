using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=tdSmKaJvCoA
public class PlatformPooler : MonoBehaviour
{
    [System.Serializable]
    public class PlatformPool
    {
        public Platform.PlatformType platformType;
        public GameObject platformPrefab;
        public int size;
    }

    public List<PlatformPool> platformPools;
    public Dictionary<Platform.PlatformType, Queue<GameObject>> poolDictionary;

    private List<GameObject> activePlatforms = new List<GameObject>();
    public Transform LastPlatform { get; private set; }

    void Start()
    {
        Actions.OnPlatformDespawn += DespawnPlatform;
        poolDictionary = new Dictionary<Platform.PlatformType, Queue<GameObject>>();
        foreach (PlatformPool pool in platformPools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject platform = Instantiate(pool.platformPrefab);
                platform.gameObject.SetActive(false);
                platform.transform.SetParent(transform);
                queue.Enqueue(platform);
            }
            poolDictionary.Add(pool.platformType, queue);
        }
    }

    public GameObject SpawnPlatform(Platform.PlatformType platformType, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(platformType))
        {
            Debug.LogWarning("PoolDictionary doesn't contain a PlatformPool of " + platformType + " type.");
            return null;
        }
        GameObject platformToSpawn = poolDictionary[platformType].Dequeue();
        platformToSpawn.gameObject.SetActive(true);
        platformToSpawn.transform.SetParent(null);
        platformToSpawn.transform.position = position;

        activePlatforms.Add(platformToSpawn);
        LastPlatform = platformToSpawn.transform;

        return platformToSpawn;
    }

    private void DespawnPlatform(Platform.PlatformType platformType, GameObject platformToDespawn)
    {
        platformToDespawn.gameObject.SetActive(false);
        platformToDespawn.transform.SetParent(transform);
        platformToDespawn.transform.position = Vector2.zero;
        activePlatforms.Remove(platformToDespawn);
        poolDictionary[platformType].Enqueue(platformToDespawn);
    }

    public List<GameObject> GetActivePlatforms(Platform.PlatformType platformType)
    {
        if (!poolDictionary.ContainsKey(platformType))
        {
            Debug.LogWarning("PoolDictionary doesn't contain a PlatformPool of " + platformType + " type.");
            return null;
        }
        //List<Platform> activePlatforms = new List<Platform>();
        List<GameObject> activePlatforms = new List<GameObject>();
        //foreach (Platform platform in poolDictionary[platformType])
        foreach (GameObject platform in poolDictionary[platformType])
        {
            if (platform.gameObject.activeSelf)
            {
                activePlatforms.Add(platform);
            }
        }
        return activePlatforms;
    }

    public List<GameObject> GetAllActivePlatforms()
    {
        return activePlatforms;
    }
}
