using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLobbyManager : MonoBehaviour
{
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