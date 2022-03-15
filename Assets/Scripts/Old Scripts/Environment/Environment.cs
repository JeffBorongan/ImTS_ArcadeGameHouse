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

    [SerializeField] private List<EnvironmentPoint> environmentPoints = new List<EnvironmentPoint>();

    [SerializeField] private TutorialActor captainRogers = null;
    [SerializeField] private GameObject playerCustomization = null;

    private Dictionary<EnvironmentPoints, EnvironmentPoint> pointsDictionary = new Dictionary<EnvironmentPoints, EnvironmentPoint>();

    private Dictionary<EnvironmentPoints, Door> doorDictionary = new Dictionary<EnvironmentPoints, Door>();
    public Dictionary<EnvironmentPoints, Door> DoorDictionary { get => doorDictionary; }

    private Dictionary<string, Vector3> currentAnatomy = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> CurrentAnatomy { get => currentAnatomy; set => currentAnatomy = value; }
    public Dictionary<EnvironmentPoints, EnvironmentPoint> PointsDictionary { get => pointsDictionary; }
    public TutorialActor CaptainRogers { get => captainRogers; }
    public GameObject PlayerCustomization { get => playerCustomization; }

    private void Start()
    {
        foreach (var point in environmentPoints)
        {
            pointsDictionary.Add(point.type, point);
        }
    }

    public void AddDoor(Door door)
    {
        doorDictionary.Add(door.Point, door);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var point in environmentPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point.point.position, 0.3f);
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
    AvatarRoomGLDoor,
    BowlingLeftPlayer
}


public enum RoomID
{
    AvatarRoomMain,
    ControlRoom,
    GameLobby,
    BowlingGame,
    Game2,
    Game3,
    AvatarRoom
}

[System.Serializable]
public class Game
{
    public GameID id = GameID.Bowling;
    public GameManagement manager = null;
}

public enum GameID
{
    Bowling,
    Game2,
    Game3,
    AvatarRoom
}