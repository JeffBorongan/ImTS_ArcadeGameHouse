public enum Floors
{ 
    None = -1,
    Elevator,
    InventoryRoom,
    SquatGame,
    SpaceLobby,
    BowlingGame,
    Game3,
}

public enum GameNumber
{
    None,
    Game1,
    Game2,
    Game3,
}

public enum AnatomyCapturePanel
{
    Start,
    LegSelection,
    Instruction,
    BodyMeasurement,
}

public enum DoorStatus
{
    None,
    Half,
    Whole,
}

public enum TypeOfObject
{
    BowlingBall,
    Alien1,
    Alien2,
    Alien3,
    Alien4,
    DeadAlien1,
    Tiles,
    Spaceship,
}

public enum LastButtonSelected
{
    None,
    Any,
    SpaceLobbyToElevator,
    BowlingGame,
    WelcomeGame1,
    Game1Instruction,
    GamesToElevator,
    SpaceLobby,
    LockEmUp,
    WelcomeGame2,
    Game2Controls,
    LockEmUpToElevator,
    WalkeyMoley,
    GoToPlatform,
    WelcomeGame3,
    WalkeyMoleyToInventoryRoom,
}

public enum TileColor
{
    None = -1,
    Green,
    Orange,
    Pink,
    Count,
}