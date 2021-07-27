using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static Environment Instance { private set; get; }

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

    public List<EnvironmentPoint> environmentPoints = new List<EnvironmentPoint>();

    public List<Transform> points = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        foreach (var point in points)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point.position, 0.3f);
        }
    }
}

[System.Serializable]
public class EnvironmentPoint
{
    public EnvironmentPoints type = EnvironmentPoints.AvatarRoomMainCenter;
    public Transform point = null;
}

public enum EnvironmentPoints
{
    Player,
    AvatarRoomMainCenter,
    AvatarRoomMainCRDoor,
    ControlRoomCenter,
    ControlRoomARDoor,
    ControlRoomGLDoor,
    GameLobbyCenter,
    GameLobbyCRDoor,
    GameLobbyBowlingDoor,
    GameLobbyGame2Door,
    GameLobbyGame3Door,
    GameLobbyARDoor,
    BowlingCenter,
    BowlingGLDoor,
    Game2Center,
    Game2GLDoor,
    Game3Center,
    Game3GLDoor,
    AvatarRoomCenter,
    AvatarRoomGLDoor
}
