using System;
using Ky;
using UnityEngine;
using Logger = Ky.Logger;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public Action onContainmentWallBroken;

    public void ContainmentWallBroken()
    {
        // ChangeState(GameState.BrokeContainmentWalls);
        ChangeState(GameState.InBreakout);
        onContainmentWallBroken?.Invoke();
    }

    private void ChangeState(GameState state)
    {
        Logger.Log($"Game state: " +
                   $"{Color.white.EncapsulateString(gameState.ToString())} -> " +
                   $"{Color.white.EncapsulateString(state.ToString())}.", Logger.DomainType.System);
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