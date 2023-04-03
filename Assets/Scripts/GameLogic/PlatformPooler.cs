using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//https://www.youtube.com/watch?v=tdSmKaJvCoA
public class PlatformPooler : MonoBehaviour
{
    [Serializable]
    public class PlatformPool
    {
        public Platform.PlatformType platformType;
        public GameObject platformPrefab;
        public int size;
    }

    [SerializeField] private List<PlatformPool> platformPools;

    private Dictionary<Platform.PlatformType, Queue<GameObject>> poolDictionary;
    private Dictionary<Platform.PlatformType, List<GameObject>> activePlatforms;
    
    public Transform LastPlatformsPosition { get; private set; } = null;

    void Start()
    {
        Actions.OnPlatformDespawn += DespawnPlatform;
        poolDictionary = new();
        activePlatforms = new();
        foreach (PlatformPool pool in platformPools)
        {
            Queue<GameObject> queue = new();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject platform = Instantiate(pool.platformPrefab);
                platform.SetActive(false);
                platform.transform.SetParent(transform);
                queue.Enqueue(platform);
            }
            poolDictionary.Add(pool.platformType, queue);
            activePlatforms.Add(pool.platformType, new List<GameObject>());
        }
    }

    public GameObject SpawnPlatform(Platform.PlatformType platformType, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(platformType))
        {
            Debug.LogWarning("PoolDictionary doesn't contain a PlatformPool of " + platformType + " type.");
            return;
        }

        GameObject platformToSpawn = 
            poolDictionary[platformType].Count > 0 ?
            poolDictionary[platformType].Dequeue() :
            InstantiateAdditionalPlatform(platformType);

        platformToSpawn.gameObject.SetActive(true);
        platformToSpawn.transform.SetParent(null);
        platformToSpawn.transform.position = position;

        activePlatforms[platformType].Add(platformToSpawn);
        LastPlatformsPosition = platformToSpawn.transform;
    }

    private void DespawnPlatform(Platform platformScript, GameObject platformToDespawn)
    {
        platformToDespawn.gameObject.SetActive(false);
        platformToDespawn.transform.SetParent(transform);
        platformToDespawn.transform.position = Vector2.zero;

        activePlatforms[platformScript.type].Remove(platformToDespawn);
        poolDictionary[platformScript.type].Enqueue(platformToDespawn);
    }

    private GameObject InstantiateAdditionalPlatform(Platform.PlatformType platformType)
    {
        Debug.LogWarning($"PlatformPooler ran out of {platformType} platforms! Instantiating additional GameObject...");
        foreach (PlatformPool pool in platformPools)
        {
            if (pool.platformType == platformType)
                return Instantiate(pool.platformPrefab);
        }
        Debug.Log($"Couldn't find the {platformType} pool when instantiating additional platform!");
        return null;
    }

    public List<GameObject> GetActivePlatforms(Platform.PlatformType platformType) => activePlatforms[platformType];

    public List<GameObject> GetAllActivePlatforms()
    {
        List<GameObject> allActivePlatforms = new();
        foreach (List<GameObject> list in activePlatforms.Values)
            allActivePlatforms.AddRange(list);
        return allActivePlatforms;
    }
}
