﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Based on https://www.youtube.com/watch?v=tdSmKaJvCoA
/// <summary>
/// A generic Pooler of Unity's GameObjects with basic functionality of keeping track of active and inactive objects.
/// </summary>
/// <typeparam name="ObjectType">The enum type of the GameObject which describes all variants of the pooled object class.</typeparam>
public abstract class GenericPooler<ObjectType, PoolableScript> : MonoBehaviour where ObjectType : Enum where PoolableScript : MonoBehaviour, IPoolable
{
    [Header("Debug Strings")]
    [SerializeField] protected string pooledObjectName;
    [SerializeField] protected string selfName;
    [Header("Pools")]
    [SerializeField] protected GameObject activeObjectsParent;
    [SerializeField] protected List<Pool<ObjectType>> objectPools;
    protected Dictionary<ObjectType, Queue<GameObject>> poolDictionary;
    protected Dictionary<ObjectType, List<GameObject>> activeObjects;

    protected virtual void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        poolDictionary = new();
        activeObjects = new();
        foreach (Pool<ObjectType> pool in objectPools)
        {
            Queue<GameObject> queue = new();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject newObject = Instantiate(pool.objectPrefab);
                newObject.SetActive(false);
                newObject.transform.SetParent(transform);
                newObject.transform.position = Vector2.zero;
                queue.Enqueue(newObject);
            }
            poolDictionary.Add(pool.objectType, queue);
            activeObjects.Add(pool.objectType, new List<GameObject>());
        }
    }

    public void SpawnObject(ObjectType objectType, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(objectType))
        {
            Debug.LogWarning($"{selfName} doesn't contain a Pool of {objectType} type.");
            return;
        }

        GameObject objectToSpawn = poolDictionary[objectType].Count > 0 ?
            poolDictionary[objectType].Dequeue() :
            InstantiateAdditionalObject(objectType);

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(activeObjectsParent.transform);
        objectToSpawn.transform.position = position;

        activeObjects[objectType].Add(objectToSpawn);
    }

    protected void DespawnObject(ObjectType objectType, GameObject objectToDespawn)
    {
        objectToDespawn.SetActive(false);
        objectToDespawn.transform.SetParent(this.transform);
        objectToDespawn.transform.position = Vector2.zero;

        activeObjects[objectType].Remove(objectToDespawn);
        poolDictionary[objectType].Enqueue(objectToDespawn);
    }

    //This is an ugly roundabout solution and i hate it... Too bad!
    protected void DespawnObject(PoolableScript script, GameObject objectToDespawn) => DespawnObject((ObjectType)script.GetPoolableType(), objectToDespawn);

    protected GameObject InstantiateAdditionalObject(ObjectType objectType)
    {
        Debug.LogWarning($"{selfName} ran out of {objectType} {pooledObjectName}s! Instantiating additional GameObject...");
        foreach (Pool<ObjectType> pool in objectPools)
        {
            if (pool.objectType.Equals(objectType))
                return Instantiate(pool.objectPrefab);
        }
        Debug.Log($"Couldn't find the {objectType} pool when instantiating additional {pooledObjectName}s!");
        return null;
    }

    public void DespawnAllActiveObjects()
    {
        foreach (KeyValuePair<ObjectType, List<GameObject>> kvp in activeObjects)
        {
            for (int i = kvp.Value.Count - 1; i >= 0; i--)
            {
                DespawnObject(kvp.Key, kvp.Value[i]);
            }
        }
    }

    public List<GameObject> GetActiveObjects(ObjectType objectType) => activeObjects[objectType];

    public List<GameObject> GetAllActiveObjects()
    {
        List<GameObject> allActiveObjects = new();
        foreach (List<GameObject> list in activeObjects.Values)
            allActiveObjects.AddRange(list);
        return allActiveObjects;
    }
}

/// <summary>
/// Basic Pool class. It holds together the Type of the object, its Unity Prefab and the initial amount of instantiated objects.
/// </summary>
/// <typeparam name="T">The enum type of the GameObject which describes all variants of the pooled object class.</typeparam>
[Serializable]
public class Pool<T> where T : Enum
{
    public T objectType;
    public GameObject objectPrefab;
    public int size;
}


