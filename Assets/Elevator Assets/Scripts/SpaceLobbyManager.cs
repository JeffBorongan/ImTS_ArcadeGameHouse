using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject spaceshipHologram = null;

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