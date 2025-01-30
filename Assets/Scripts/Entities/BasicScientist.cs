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
    [SerializeField] private float moveSpeed = 5f;
    private float engagedTimer;
    bool alive = true;
    AnimatedCharacter animationController;
    [SerializeField]int hp = 5;
    [SerializeField]bool canMove, canAttack;
    [SerializeField]GameObject body;
    Rigidbody2D rb;
    [SerializeField]EnemyState initialState = EnemyState.Idle;
    [ShowInInspector] private EnemyState State => fsm?.State ?? EnemyState.Idle;
    [ShowInInspector, ReadOnly] public int LevelIndex { get; set; }
    [SerializeField]PlayerTag pTag;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(initialState);
        animationController = gameObject.GetComponent<AnimatedCharacter>();
        if (animationController == null){
            print (name+" has no Animator");
        }
    }

    private void Update()
    {
        if (!alive)
        {
            return;
        }
        CheckRotation();
        fsm.Driver.Update.Invoke();
        
    }

    [Button]
    public void Alarm()
    {
        fsm.ChangeState(CanAttack() ? EnemyState.Attacking : EnemyState.Following);
    }

    private bool CanAttack()
    {
        if (!canAttack)
        {
            return false;
        }
        if (canAttackTester)
        {
            canAttackTester = false;
            return true;
        }

        if (!HasLineOfSight()) return false;
        var distanceToPlayer = Vector3.Distance(transform.position, Player.I.transform.position);
        return distanceToPlayer < attackDistance;
    }

    private bool HasLineOfSight()
    {
        var obstacleMask = GameManager.I.obstacleMask;
        var playerTr = Player.I.transform;
        Vector2 directionToPlayer = (playerTr.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTr.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

        return hit.collider == null;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!alive)
        {
            return;
        }
        var player = other.transform.GetComponent<PlayerMovement>();
        if (player != null)
        {
            if (TakeDamage(player.GetSpeed()))
            {
                PlayerTag.AddToList(other.transform.GetComponent<Player>().GetTagList(),pTag);
            }
        }
    }
    bool TakeDamage (float impactSpeed)
    {

        hp -= (int)impactSpeed*2;
        if (hp <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    void Die ()
    {
        alive = false;
        
        float timeToDie = animationController.StartAnimation(Animation.AnimationId.die,Animation.Direction.none, false);
        Destroy(gameObject.GetComponent<Collider2D>());
        rb.velocity = Vector2.zero;
        if (body != null)
        {
            FadeInItem corpse = Instantiate (body, transform.position+new Vector3(0,0,0.01f), Quaternion.identity).GetComponent<FadeInItem>();
            if (corpse != null)
            {
                corpse.Setup(timeToDie);
            }
        }

        Destroy(gameObject, timeToDie);
        
    }
    #region Finite State Machine
    private void Following_Update()
    {
        if (HasLineOfSight())
        {
            engagedTimer = 1f;
        }

        engagedTimer -= Time.deltaTime;
        if (engagedTimer > 0)
        {
            if (canMove){
                transform.position = Vector3.MoveTowards(transform.position, Player.I.transform.position, moveSpeed * Time.deltaTime);
                animationController.StartAnimation(Animation.AnimationId.walk,Animation.Direction.none, false);
            }
        }

        if (CanAttack())
        {
            fsm.ChangeState(EnemyState.Attacking);
        }
    }

    private IEnumerator Attacking_Enter()
    {
        
        yield return new WaitForSeconds(2f);
        animationController.StartAnimation(Animation.AnimationId.attack,Animation.Direction.none, false);
        var projectile = Instantiate(basicProjectilePrefab, transform.position, Quaternion.identity, ProjectileParent.I);
        projectile.Init(Player.I.Pos - transform.position);
        projectile.transform.position += Vector3.back;
        fsm.ChangeState(EnemyState.Following);
    }
    #endregion
    void CheckRotation()
    {
        
        animationController.Turn(AnimatedCharacter.VectorToDirection((Vector2)Player.I.Pos-(Vector2)transform.position));
    }
}