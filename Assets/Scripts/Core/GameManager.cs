using System;
using Ky;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public Action onContainmentWallBroken;

    public void ContainmentWallBroken()
    {
        ChangeState(GameState.BrokeContainmentWalls);
        onContainmentWallBroken?.Invoke();
    }

    private void ChangeState(GameState state)
    {
        Logger.Log($"Game state: <color=white>{gameState} -> {state}.", Logger.DomainType.System);
        gameState = state;
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