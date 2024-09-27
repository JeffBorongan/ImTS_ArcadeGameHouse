using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSpawner : Spawner
{
    private int pinsLeft = 10;

    public void RemovePins()
    {
        pinsLeft--;
        if (pinsLeft <= 0)
        {
            StartCoroutine(SpawnAfterXSeconds(5f));
            pinsLeft = 10;
        }
    }

    private IEnumerator SpawnAfterXSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Spawn();
    }
}
