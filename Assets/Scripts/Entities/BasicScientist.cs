using System.Collections;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasicScientist : Enemy
{
    private StateMachine<EnemyState> fsm;
    public bool canAttackTester;
    public BasicProjectile basicProjectilePrefab;

    [ShowInInspector] private EnemyState State => fsm?.State ?? EnemyState.Idle;

    private void Awake()
    {
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(EnemyState.Idle);
    }

    private void Update()
    {
        fsm.Driver.Update.Invoke();
    }

    private bool CanAttack()
    {
        if (canAttackTester)
        {
            canAttackTester = false;
            return true;
        }

        return false;
    }

    #region Finite State Machine
    private void Idle_Update()
    {
        if (GameManager.I.gameState == GameState.InBreakout)
        {
            fsm.ChangeState(CanAttack() ? EnemyState.Attacking : EnemyState.Following);
        }
    }

    private void Following_Update()
    {
        if (CanAttack())
        {
            fsm.ChangeState(EnemyState.Attacking);
        }
    }

    private IEnumerator Attacking_Enter()
    {
        yield return new WaitForSeconds(2f);
        var projectile = Instantiate(basicProjectilePrefab, transform.position, Quaternion.identity, ProjectileParent.I.transform);
        projectile.Init(Player.I.Pos - transform.position);
        fsm.ChangeState(EnemyState.Following);
    }
    #endregion
}