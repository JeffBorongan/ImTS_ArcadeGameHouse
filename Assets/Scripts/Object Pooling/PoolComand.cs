using UnityEngine;

public class PoolComand : MonoBehaviour
{
    #region Parameters

    public TypeOfObject type = TypeOfObject.BowlingBall;
    public GameObject objectToPool = null;
    public int poolCount = 0;

    #endregion

    #region Start

    private void Start()
    {
        ObjectPoolingManager.Instance.AddPool(type, objectToPool, poolCount, transform);
    }

    #endregion
}