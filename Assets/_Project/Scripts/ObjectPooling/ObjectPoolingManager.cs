using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance { get; private set; }

    [SerializeField] private List<PooledObjectInfo> ObjectPools;

    private GameObject holder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetupEmties();
    }

    private void SetupEmties()
    {
        if (holder != null) Destroy(holder);

        holder = new GameObject("Holder");
        holder.transform.parent = transform;
    }

    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        string objectName = objectToSpawn.name.Replace("(Clone)", string.Empty);
        PooledObjectInfo pool = ObjectPools.Find(x => x.LookupString == objectName);

        if (pool == null)
        {
            pool = new PooledObjectInfo
            {
                LookupString = objectToSpawn.name,
            };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = null;
        foreach (var item in pool.InactiveObjects)
        {
            if (item != null)
            {
                spawnableObject = item;
                break;
            }
        }

        if (spawnableObject == null)
        {
            GameObject parent = holder;
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parent != null)
            {
                spawnableObject.transform.SetParent(parent.transform);
            }
            spawnableObject.SetActive(true);
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        string objectName = obj.name.Replace("(Clone)", string.Empty);
        PooledObjectInfo pool = ObjectPools.Find(x => x.LookupString == objectName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            if (!obj.activeSelf) return;

            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    public void ResetPool()
    {
        foreach (Transform child in holder.transform)
        {
            ReturnObjectToPool(child.gameObject);
        }
    }
}