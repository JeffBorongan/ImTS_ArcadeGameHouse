using System.Collections;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public GameObject alienPrefab;

    private void Start()
    {
        StartCoroutine(AlienPopulator(3f));
    }

    private IEnumerator AlienPopulator(float spawnTime)
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            GameObject alien = Instantiate(alienPrefab);
            Vector3 spawnPosition = new Vector3(Random.Range(0f, 4f), 2.5f, Random.Range(10f, 14f));
            alien.transform.position = spawnPosition;
        }
    }
}