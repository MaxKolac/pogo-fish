using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPooler : MonoBehaviour
{ 
    public static PlatformPooler platformPool;
    public List<GameObject> pooledPlatforms;
    public GameObject objectToPool;
    public int poolSize;

    void Awake()
    {
        platformPool = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledPlatforms = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < poolSize; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledPlatforms.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledPlatforms.Count; i++)
        {
            if (!pooledPlatforms[i].activeInHierarchy)
                return pooledPlatforms[i];
        }
        return null;
    }
}
