using System;
using System.Collections;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public GameObject bowlingBallPrefab;
    private BowlingBall currentBall;

    private void Start()
    {
        SpawnBowlingBall();
    }

    private void SpawnBowlingBall()
    {
        GameObject bowlingBallClone = ObjectPoolingManager.Instance.GetFromPool(TypeOfObject.BowlingBall);
        bowlingBallClone.SetActive(true);
        bowlingBallClone.transform.position = transform.position;
        bowlingBallClone.transform.rotation = transform.rotation;
        currentBall = bowlingBallClone.GetComponent<BowlingBall>();

        currentBall.OnRelease.RemoveAllListeners();
        currentBall.OnRelease.AddListener(HandleOnRelease);
    }

    private void HandleOnRelease()
    {
        if (!ObjectPoolingManager.Instance.HasObjectOnPool(TypeOfObject.BowlingBall)) 
        {
            StartCoroutine(SpawnCheckCour());
        }
        
        else
        {
            SpawnBowlingBall();
        }
    }

    IEnumerator SpawnCheckCour()
    {
        yield return new WaitUntil(() => ObjectPoolingManager.Instance.HasObjectOnPool(TypeOfObject.BowlingBall));
        SpawnBowlingBall();
    }
}