using Ky;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
}

public enum GameState
{
    InPuzzle,
    BrokeContainmentWalls,
    InBreakout,
    RewindingTime,
    Dead,
}