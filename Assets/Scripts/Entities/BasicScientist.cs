using System.Collections;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class BasicScientist : Enemy, ILevelObject
{
    private StateMachine<EnemyState> fsm;
    public bool canAttackTester;
    public BasicProjectile basicProjectilePrefab;
    [SerializeField] private float attackDistance = 5f;

    [ShowInInspector] private EnemyState State => fsm?.State ?? EnemyState.Idle;
    [ShowInInspector, ReadOnly] public int LevelIndex { get; set; }

    private void Awake()
    {
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(EnemyState.Idle);
    }

    private void Update()
    {
        fsm.Driver.Update.Invoke();
    }

    public void Alarm()
    {
        fsm.ChangeState(CanAttack() ? EnemyState.Attacking : EnemyState.Following);
    }

    private bool CanAttack()
    {
        if (canAttackTester)
        {
            canAttackTester = false;
            return true;
        }

        var distanceToPlayer = Vector3.Distance(transform.position, Player.I.transform.position);
        return distanceToPlayer < attackDistance;
    }

    #region Finite State Machine
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
        var projectile = Instantiate(basicProjectilePrefab, transform.position, Quaternion.identity, ProjectileParent.I);
        projectile.Init(Player.I.Pos - transform.position);
        fsm.ChangeState(EnemyState.Following);
    }
    #endregion
}