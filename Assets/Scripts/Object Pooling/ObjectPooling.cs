using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private List<Pool> pool = new List<Pool>();

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
        selectedObject.SetActive(true);

        return selectedObject;
    }

    public bool hasObjectOnPool(TypeOfObject type)
    {
        Pool poolToGet = pool.Where(p => p.type == type).FirstOrDefault();
        return poolToGet.poolOfObjects.Where(o => !o.activeSelf).FirstOrDefault() == null;
    }
}

[System.Serializable]
public class Pool
{
    public TypeOfObject type = TypeOfObject.BowlingBall;
    public List<GameObject> poolOfObjects = new List<GameObject>();

    public Pool(TypeOfObject p_type, List<GameObject> p_poolOfObjects)
    {
        type = p_type;
        poolOfObjects = p_poolOfObjects;
    }
}

public enum TypeOfObject
{
    BowlingBall,
    EnemyAlien
}