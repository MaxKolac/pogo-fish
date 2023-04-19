using System;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=tdSmKaJvCoA
public class PickableObjectPooler : MonoBehaviour
{
    [Serializable]
    internal class PickableObjectPool
    {
        public PickableObject.PickableObjectType objectType;
        public GameObject objectPrefab;
        public int size;
    }

    [SerializeField] private List<PickableObjectPool> objectPools;

    private Dictionary<PickableObject.PickableObjectType, Queue<GameObject>> poolDictionary;
    private Dictionary<PickableObject.PickableObjectType, List<GameObject>> activeObjects;

    void Start()
    {
        Actions.OnPickableObjectDespawn += DespawnPickableObject;
        poolDictionary = new();
        activeObjects = new();
        foreach (PickableObjectPool pool in objectPools)
        {
            Queue<GameObject> queue = new();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject platform = Instantiate(pool.objectPrefab);
                platform.SetActive(false);
                platform.transform.SetParent(transform);
                queue.Enqueue(platform);
            }
            poolDictionary.Add(pool.objectType, queue);
            activeObjects.Add(pool.objectType, new List<GameObject>());
        }
    }

    public void SpawnPickableObject(PickableObject.PickableObjectType objectType, Vector2 position)
    {
        if (!poolDictionary.ContainsKey(objectType))
        {
            Debug.LogWarning("PoolDictionary doesn't contain a PickableObjectPool of " + objectType + " type.");
            return;
        }

        GameObject objectToDespawn = 
            poolDictionary[objectType].Count > 0 ?
            poolDictionary[objectType].Dequeue() :
            InstantiateAdditionalPlatform(objectType);

        objectToDespawn.SetActive(true);
        objectToDespawn.transform.SetParent(null);
        objectToDespawn.transform.position = position;

        activeObjects[objectType].Add(objectToDespawn);
    }

    private void DespawnPickableObject(PickableObject objectScript, GameObject objectToDespawn)
    {
        objectToDespawn.SetActive(false);
        objectToDespawn.transform.SetParent(transform);
        objectToDespawn.transform.position = Vector2.zero;

        activeObjects[objectScript.Type].Remove(objectToDespawn);
        poolDictionary[objectScript.Type].Enqueue(objectToDespawn);
    }

    private GameObject InstantiateAdditionalPlatform(PickableObject.PickableObjectType objectType)
    {
        Debug.LogWarning($"PickableObjectPooler ran out of {objectType} pickable objects! Instantiating additional GameObject...");
        foreach (PickableObjectPool pool in objectPools)
        {
            if (pool.objectType == objectType)
                return Instantiate(pool.objectPrefab);
        }
        Debug.Log($"Couldn't find the {objectType} pool when instantiating additional platform!");
        return null;
    }

    public void DespawnAllActivePickableObjects()
    {
        foreach (List<GameObject> platformList in activeObjects.Values)
        {
            for (int i = 0; i < platformList.Count; i++)
            {
                platformList[i].transform.position = new Vector2(0, GlobalAttributes.LowerScreenEdge - 5f);
                //This triggers the If statement in Platform.Update(), which invokes the platform's despawn Action.
            }
        }
    }

    public List<GameObject> GetActivePickableObjects(PickableObject.PickableObjectType objectType) => activeObjects[objectType];

    public List<GameObject> GetAllActivePickableObjects()
    {
        List<GameObject> allActivePickableObjects = new();
        foreach (List<GameObject> list in activeObjects.Values)
            allActivePickableObjects.AddRange(list);
        return allActivePickableObjects;
    }
}
