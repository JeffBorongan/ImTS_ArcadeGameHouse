using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public float alienSpeed = 0f;
    public float alienSpawnTime = 0f;
    public int pointsToEarn = 0;
    public int timeToBeat = 0;
    public List<GameObject> aliens = new List<GameObject>();
    public List<Side> lanesToSpawn = new List<Side>();

    public bool isStarted = false;

    public Level(float p_alienSpeed, float p_alienSpawnTime, int p_pointsToEarn, int p_timeToBeat, List<Side> p_lanesToSpawn, List<GameObject> p_aliens)
    {
        alienSpeed = p_alienSpeed;
        alienSpawnTime = p_alienSpawnTime;
        pointsToEarn = p_pointsToEarn;
        lanesToSpawn = p_lanesToSpawn;
        aliens = p_aliens;
        timeToBeat = p_timeToBeat;
    }

    public void StartLevel()
    {
        isStarted = true;
        SpawnManager.Instance.StartSpawning(this);
    }

    public void StopLevel()
    {
        isStarted = false;
        SpawnManager.Instance.StopSpawning();
    }
}
