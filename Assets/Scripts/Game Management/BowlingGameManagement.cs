using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BowlingGameManagement : GameManagement
{
    [SerializeField] private BowlingSessionData sessionData = null;
    private bool sessionIsStarted = false;

    [SerializeField] private List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();

    public override void StartGame(UnityAction OnEndGame)
    {
        sessionIsStarted = true;
        sessionData = new BowlingSessionData();
        StartCoroutine(SpawningCour());
    }

    IEnumerator SpawningCour()
    {
        while (sessionIsStarted)
        {
            SpawnEnemy(sessionData.lanes[Random.Range(0, sessionData.lanes.Count)], (Side)Random.Range(0, (int)Side.Count));
            yield return new WaitForSeconds(sessionData.enemySpawnTime);
        }
    }

    public void SpawnEnemy(Side main, Side secondary)
    {

    }

    public override void StopGame()
    {

    }

    public override void ResetGame()
    {

    }
}

public class BowlingSessionData : SessionData
{
    public float enemySpawnTime = 5f;
    public List<Side> lanes = new List<Side>() { Side.Left, Side.Middle, Side.Right };
    public float enemySpeed = 1f;
    public int pointPerEnemy = 1;

    public int pointsToEarn = 10;
    public int timeDuration = 240;

    public int enemyReachedTheCockpit = 1;
}
