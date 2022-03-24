using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    #region Singleton

    public static ObjectPoolingManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Parameters

    private List<Pool> pool = new List<Pool>();

    #endregion

    #region Object Pooling Functions

    public void AddPool(TypeOfObject type, GameObject objectToPool, int poolCount, Transform parent)
    {
        List<GameObject> generatedPool = new List<GameObject>();

        for (int i = 0; i < poolCount; i++)
        {
            GameObject clone = Instantiate(objectToPool, parent);
            clone.SetActive(false);
            generatedPool.Add(clone);
        }

        pool.Add(new Pool(type, generatedPool));
    }

    public GameObject GetFromPool(TypeOfObject type)
    {
        Pool poolToGet = pool.Where(p => p.type == type).FirstOrDefault();
        GameObject selectedObject = poolToGet.poolOfObjects.Where(o => !o.activeSelf).FirstOrDefault();
        return selectedObject;
    }

    public bool HasObjectOnPool(TypeOfObject type)
    {
        Pool poolToGet = pool.Where(p => p.type == type).FirstOrDefault();
        return poolToGet.poolOfObjects.Where(o => !o.activeSelf).FirstOrDefault() != null;
    }

    #endregion
}

[System.Serializable]
public class Pool
{
    #region Parameters

    public TypeOfObject type = TypeOfObject.BowlingBall;
    public List<GameObject> poolOfObjects = new List<GameObject>();

    #endregion

    #region Pool Function

    public Pool(TypeOfObject p_type, List<GameObject> p_poolOfObjects)
    {
        type = p_type;
        poolOfObjects = p_poolOfObjects;
    }

    #endregion
}