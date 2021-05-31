using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { private set; get; }


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


    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    [SerializeField] private GameObject alien = null;

    Dictionary<KeyValuePair<Side, Side>, bool> currentLanesStatus = new Dictionary<KeyValuePair<Side, Side>, bool>();
    private bool isSpawning = false;

    System.Random SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
    List<int> spawnPointsGenerated = new List<int>();

    private void Start()
    {
        List<Side> lanesToSpawn = new List<Side>();
        lanesToSpawn.Add(Side.Left);
        //lanesToSpawn.Add(Side.Middle);
        //lanesToSpawn.Add(Side.Right);

        List<GameObject> aliens = new List<GameObject>();
        aliens.Add(alien);

        StartSpawning(new Level(1f, 5f, 10, lanesToSpawn, aliens));
    }

    public void StartSpawning(Level level)
    {
        isSpawning = true;

        SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
        spawnPointsGenerated.Clear();
        currentLanesStatus.Clear();

        foreach (var lane in level.lanesToSpawn)
        {
            for (int i = 0; i < (int)Side.Count; i++)
            {
                currentLanesStatus.Add(new KeyValuePair<Side, Side>(lane, (Side)i), false);
            }
        }

        StartCoroutine(SpawnCour(level));
    }


    IEnumerator SpawnCour(Level level)
    {
        while (isSpawning)
        {

            if(spawnPointsGenerated.Count != currentLanesStatus.Count)
            {
                int randomSpawnPointIndex = SpawnPointGenerator.Next(currentLanesStatus.Count);
                while (spawnPointsGenerated.Contains(randomSpawnPointIndex))
                {
                    randomSpawnPointIndex = SpawnPointGenerator.Next(currentLanesStatus.Count);
                }

                spawnPointsGenerated.Add(randomSpawnPointIndex);

                var random = currentLanesStatus.ElementAt(randomSpawnPointIndex);

                if (!random.Value)
                {
                    SpawnPoint newSpawnPoint = spawnPoints.Where(s => s.lane == random.Key.Key && s.side == random.Key.Value).FirstOrDefault();

                    GameObject clone = Instantiate(level.aliens[0], newSpawnPoint.point.position, newSpawnPoint.point.rotation);
                    clone.name = random.Key.Key.ToString() + " : " + random.Key.Value.ToString();

                    SampleAlien alien = clone.GetComponent<SampleAlien>();
                    alien.OnDeath.AddListener(() =>
                    {
                        currentLanesStatus[new KeyValuePair<Side, Side>(random.Key.Key, random.Key.Value)] = false;
                        spawnPointsGenerated.Remove(randomSpawnPointIndex);
                    });

                    currentLanesStatus[new KeyValuePair<Side, Side>(random.Key.Key, random.Key.Value)] = true;
                }
            }

            yield return new WaitForSeconds(level.alienSpawnTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnPoints[0].point.position, 1f);
        Gizmos.DrawSphere(spawnPoints[1].point.position, 1f);
        Gizmos.DrawSphere(spawnPoints[2].point.position, 1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawnPoints[3].point.position, 1f);
        Gizmos.DrawSphere(spawnPoints[4].point.position, 1f);
        Gizmos.DrawSphere(spawnPoints[5].point.position, 1f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(spawnPoints[6].point.position, 1f);
        Gizmos.DrawSphere(spawnPoints[7].point.position, 1f);
        Gizmos.DrawSphere(spawnPoints[8].point.position, 1f);
    }
}

[System.Serializable]
public class SpawnPoint
{
    public Side lane = Side.Left;
    public Side side = Side.Left;
    public Transform point = null;

    public SpawnPoint(Side p_lane, Side p_side)
    {
        lane = p_lane;
        side = p_side;
    }
}

public enum Side
{
    Left,
    Middle,
    Right,
    Count
}