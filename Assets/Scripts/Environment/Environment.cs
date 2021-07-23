using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static Environment Instance { private set; get; }

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

    public List<Transform> points = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        foreach (var point in points)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point.position, 0.3f);
        }
    }
}
