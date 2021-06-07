using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public GameObject bowlingBallPrefab;
    private BowlingBall currentBall;

    private void Start()
    {
        SpawnBowlingBall();
    }

    private void Update()
    {
        if(currentBall.CurrentState == BowlingBallStates.OnRelease)
        {
            SpawnBowlingBall();
        }
    }

    private void SpawnBowlingBall()
    {
        GameObject bowlingBallClone = ObjectPooling.Instance.GetFromPool(TypeOfObject.BowlingBall);
        bowlingBallClone.transform.position = transform.position;
        bowlingBallClone.transform.rotation = transform.rotation;
        currentBall = bowlingBallClone.GetComponent<BowlingBall>();
    }
}