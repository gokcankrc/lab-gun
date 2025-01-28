using System;
using System.Collections.Generic;
using Ky;
using UnityEngine;
using Logger = Ky.Logger;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public Action<int> onContainmentWallBroken;
    public LayerMask obstacleMask;
    public Dictionary<int, Level> levels = new();

    public void ContainmentWallBroken(int levelIndex)
    {
        if (gameState != GameState.InPuzzle) return;
        ChangeState(GameState.InBreakout);
        onContainmentWallBroken?.Invoke(levelIndex);
    }

    private void ChangeState(GameState state)
    {
        Logger.Log($"Game state: " +
                   $"{Color.white.EncapsulateString(gameState.ToString())} -> " +
                   $"{Color.white.EncapsulateString(state.ToString())}.", Logger.DomainType.System);
        gameState = state;

        if (MusicManager.I != null)
        {
            MusicManager.I.GameStateChanged(state);
        }
        
    }

    public void AddLevel(int index, Level level)
    {
        levels.TryAdd(index, level);
        onContainmentWallBroken += level.TriggerAlarm;
    }

    public void RemoveLevel(int index)
    {
        levels.Remove(index);
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