using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class SquatGameManagement : GameManagement
{
    #region Singleton

    public static SquatGameManagement Instance { private set; get; }

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

    private SquatGameSessionData sessionData = null;

    [Header("Enemy Spawning")]
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> enemyGoToPoints = new List<Transform>();
    private bool isSpawning = false;
    private bool proceedToNext = false;

    [Header("Doors and Lights")]
    [SerializeField] private List<GameObject> doors = new List<GameObject>();
    [SerializeField] private List<GameObject> lights = new List<GameObject>();
    [SerializeField] private Material redLight;
    [SerializeField] private Material greenLight;
    private MoveStatus moveStatus = MoveStatus.None;
    private bool isDoorMovable = false;

    [Header("Levers")]
    [SerializeField] private GameObject leftLever;
    [SerializeField] private GameObject rightLever;

    private AlienMovement alien = null;
    private int index = 0;

    #endregion

    #region Initialize

    public override void InitializeGame()
    {
        //StartGame(new SquatGameSessionData(), () => { });

        foreach (var door in doors)
        {
            door.transform.DOMoveY(-1.1f, 0.5f);
        }

        foreach (var light in lights)
        {
            light.GetComponent<Renderer>().material = redLight;
        }

        isSpawning = true;
        StartCoroutine(SpawningCour());
    }

    #endregion

    #region Game Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        pnlStartGame.gameObject.SetActive(true);
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            sessionData = (SquatGameSessionData)data;
            pnlStartGame.gameObject.SetActive(false);

            /*foreach (var door in doors)
            {
                door.transform.DOMoveY(-1.1f, 0.5f);
            }

            foreach (var light in lights)
            {
                light.GetComponent<Renderer>().material = redLight;
            }

            isSpawning = true;
            StartCoroutine(SpawningCour());*/
        });
    }

    public override void StopGame()
    {

    }

    #endregion

    #region Enemy Spawning

    IEnumerator SpawningCour()
    {
        while (isSpawning)
        {
            SpawnEnemy(enemySpawnPoints[index], enemyGoToPoints[index]);
            yield return new WaitUntil(() => proceedToNext);
        }
    }

    private void SpawnEnemy(Transform spawnPoint, Transform enemyGoal)
    {
        proceedToNext = false;
        GameObject clone = ObjectPooling.Instance.GetFromPool(TypeOfObject.EnemyAlien);
        clone.transform.position = spawnPoint.position;
        clone.transform.rotation = spawnPoint.rotation;
        clone.SetActive(true);

        alien = clone.GetComponent<AlienMovement>();
        alien.SetMovementSpeed(0.5f);
        Vector3 targetDestination = new Vector3(alien.transform.position.x, alien.transform.position.y, enemyGoal.position.z);
        alien.AlienAgent.SetDestination(targetDestination);

        alien.OnDeath.AddListener(() =>
        {
            if (isSpawning && index == 4)
            {
                index = 0;
                foreach (var door in doors)
                {
                    door.transform.DOMoveY(-1.1f, 0.5f);
                }

                foreach (var light in lights)
                {
                    light.GetComponent<Renderer>().material = redLight;
                }
            }
            else
            {
                index++;
            }

            alien.gameObject.SetActive(false);
            proceedToNext = true;
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            foreach (var door in doors)
            {
                door.transform.DOMoveY(1f, 0.5f);
            }

            foreach (var light in lights)
            {
                light.GetComponent<Renderer>().material = redLight;
            }

            isSpawning = false;
            StopCoroutine(SpawningCour());
            alien.OnReachDestination.RemoveAllListeners();
        });
    }

    #endregion

    #region Lever Pulling

    private void UpdateLeverPulling(bool engageLever, Vector3 leftLever, Vector3 rightLever, float maximumLift, float minimumDrop)
    {
        if (engageLever)
        {
            if (leftLever.y >= maximumLift && rightLever.y >= maximumLift)
            {
                if (moveStatus == MoveStatus.Half)
                {
                    isDoorMovable = true;
                    doors[index].transform.DOMoveY(0f, 0.5f);
                }
                if (moveStatus == MoveStatus.Whole)
                {
                    isDoorMovable = false;
                    lights[index].GetComponent<Renderer>().material = greenLight;
                    doors[index].transform.DOMoveY(1f, 0.5f).OnComplete(() =>
                    {
                        alien.OnDeath.Invoke();
                    });
                    moveStatus = MoveStatus.None;
                }
            }

            if (leftLever.y <= minimumDrop && rightLever.y <= minimumDrop)
            {
                if (!isDoorMovable)
                {
                    moveStatus = MoveStatus.Half;
                }
                if (isDoorMovable)
                {
                    moveStatus = MoveStatus.Whole;
                }
            }
        }
    }

    #endregion

    #region Update

    private void Update()
    {
        UpdateLeverPulling(isSpawning, leftLever.transform.position, rightLever.transform.position, 1f, 0.3f);
    }

    #endregion
}

public class SquatGameSessionData : SessionData
{
    public float enemySpeed = 0.5f;
    public int enemyReachedTheDoor = 1;
}

public enum MoveStatus
{
    None,
    Half,
    Whole,
}