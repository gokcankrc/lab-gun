using System;
using MonsterLove.StateMachine;
using UnityEngine;

public class Enemy : MonoBehaviour { }

public class BasicScientist : Enemy
{
    StateMachine<EnemyState> fsm;

    private void Awake()
    {
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(EnemyState.Idle);
    }

    #region Finite State Machine
    private void Idle_Enter() { }
    private void Idle_Update() { }
    private void Idle_Exit() { }
    private void Idle_Finally() { }

    private void Attacking_Enter() { }
    private void Attacking_Update() { }
    private void Attacking_Exit() { }
    private void Attacking_Finally() { }

    private void Following_Enter() { }
    private void Following_Update() { }
    private void Following_Exit() { }
    private void Following_Finally() { }
    #endregion
}

internal enum EnemyState
{
    Idle,
    Attacking,
    Following,
}