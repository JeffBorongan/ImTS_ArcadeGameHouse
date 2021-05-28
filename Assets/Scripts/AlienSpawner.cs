using System.Collections;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public GameObject alienPrefab;
    public bool spawnAliens = true;
    public float spawnTime = 3f;

    private void Start()
    {
        StartCoroutine(AlienPopulator(spawnTime));
    }

    private IEnumerator AlienPopulator(float spawnTime)
    {
        while(spawnAliens == true)
        {
            yield return new WaitForSeconds(spawnTime);
            GameObject alienClone = Instantiate(alienPrefab);
            Vector3 spawnPosition = new Vector3(Random.Range(0f, 4f), 2.5f, Random.Range(10f, 14f));
            alienClone.transform.position = spawnPosition;
        }
    }
}