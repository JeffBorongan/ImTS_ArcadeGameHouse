using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject spaceshipHologram = null;
    [SerializeField] private GameObject game1Trophy = null;
    [SerializeField] private GameObject game2Trophy = null;
    [SerializeField] private GameObject game3Trophy = null;

    private void Start()
    {
        if (TrophyManager.Instance.isGame1Accomplished)
        {
            game1Trophy.SetActive(true);
        }

        if (TrophyManager.Instance.isGame2Accomplished)
        {
            game2Trophy.SetActive(true);
        }

        if (TrophyManager.Instance.isGame3Accomplished)
        {
            game3Trophy.SetActive(true);
        }
    }

    private void Update()
    {
        spaceshipHologram.transform.Rotate(0, 0, 10 * Time.deltaTime);
    }

    public void openElevatorDoor()
    {
        ElevatorFloorManager.Instance.openElevatorDoor();
    }

    public void getElevatorPart(MeshRenderer meshRenderer)
    {
        ElevatorFloorManager.Instance.getElevatorPart(meshRenderer);
    }

    public void enableEmissive(int number)
    {
        ElevatorFloorManager.Instance.enableEmissive(number);
    }
}