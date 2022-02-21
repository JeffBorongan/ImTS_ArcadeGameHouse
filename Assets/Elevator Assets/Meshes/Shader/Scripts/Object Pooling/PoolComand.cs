using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolComand : MonoBehaviour
{
    public TypeOfObject type = TypeOfObject.BowlingBall;
    public GameObject objectToPool = null;
    public int poolCount = 0;

    private void Start()
    {
        ObjectPooling.Instance.AddPool(type, objectToPool, poolCount, transform);
    }
}