using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private Transform _spawnLocation;
    
    [HideInInspector] public GameObject _spawnedObject;

    public virtual void Spawn()
    {
        _spawnedObject = Instantiate(_objectToSpawn, _spawnLocation);
    }
}
