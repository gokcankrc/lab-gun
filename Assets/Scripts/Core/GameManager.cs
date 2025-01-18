using System;
using Ky;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public Action onContainmentWallBroken;

    public void ContainmentWallBroken()
    {
        gameState = GameState.BrokeContainmentWalls;
        onContainmentWallBroken?.Invoke();
    }
}

public enum GameState
{
    InPuzzle,
    BrokeContainmentWalls,
    InBreakout,
    RewindingTime,
    Dead,
}