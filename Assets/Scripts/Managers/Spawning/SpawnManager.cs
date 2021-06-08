using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

    Dictionary<KeyValuePair<Side, Side>, bool> currentLanesStatus = new Dictionary<KeyValuePair<Side, Side>, bool>();
    private bool isSpawning = false;

    System.Random SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
    List<int> spawnPointsGenerated = new List<int>();

    [SerializeField] private GameObject alienEnemy = null;
    private List<GameObject> spawnedAliens = new List<GameObject>();
    private IEnumerator spawningCour = null;

    private void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(HandleOnGameStart);
        GameManager.Instance.OnGameUpdate.AddListener(HandleOnGameUpdate);
        GameManager.Instance.OnGameEnd.AddListener(HandleOnGameEnd);
    }

    private void HandleOnGameEnd(bool win)
    {
        StopSpawning();
        DestroyAllAliens();
    }

    private void HandleOnGameUpdate(Level level, UnityAction callback)
    {
        StopSpawning();
        StartSpawning(level);
    }

    private void HandleOnGameStart(Level level, UnityAction callback)
    {
        StartSpawning(level);
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

        spawningCour = SpawnCour(level);
        StartCoroutine(spawningCour);
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
                    GameObject clone = ObjectPooling.Instance.GetFromPool(TypeOfObject.EnemyAlien);
                    clone.transform.position = newSpawnPoint.point.position;
                    clone.transform.rotation = newSpawnPoint.point.rotation;
                    clone.SetActive(true);
                    spawnedAliens.Add(clone);

                    AlienMovement alien = clone.GetComponent<AlienMovement>();
                    alien.SetMovementSpeed(level.alienSpeed);

                    alien.OnDeath.AddListener(() =>
                    {
                        currentLanesStatus[new KeyValuePair<Side, Side>(random.Key.Key, random.Key.Value)] = false;
                        spawnPointsGenerated.Remove(randomSpawnPointIndex);
                        GameManager.Instance.AddPoint();
                    });

                    alien.OnReachDestination.AddListener(() =>
                    {
                        currentLanesStatus[new KeyValuePair<Side, Side>(random.Key.Key, random.Key.Value)] = false;
                        spawnPointsGenerated.Remove(randomSpawnPointIndex);
                        GameManager.Instance.AddAlienReachedCockpit();
                    });

                    currentLanesStatus[new KeyValuePair<Side, Side>(random.Key.Key, random.Key.Value)] = true;
                }
            }

            yield return new WaitForSeconds(level.alienSpawnTime);
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(spawningCour);
        isSpawning = false;        
    }

    void DestroyAllAliens()
    {
        foreach (var alien in spawnedAliens)
        {
            alien.SetActive(false);
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