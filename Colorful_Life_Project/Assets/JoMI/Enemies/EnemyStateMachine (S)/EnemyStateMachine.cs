using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<EnemyStateMachine.EnemyState>, IHittable 
{
    [SerializeField] private Transform _player; //Setted in an enemy Spawn Manager
    [SerializeField] private GameObject _enemyOrb;


    [SerializeField] private LayerMask _playerLayerMask;

    private Rigidbody _rb;

    private int _enemyHealth;
    private float _playerDistance;

    [SerializeField] private float _enemyHitBox;



    [SerializeField] float _chasingDistance, _attackingDistance, _searchDistance;


    [Header("Attack Modifiers"), SerializeField]
    private float _preparingAttackTimer;
    [SerializeField] private float _attackTimer;
    [SerializeField] private float _attackForce;

    #region gets & sets
    //public string Name { get; private set; }
    //public GameObject coisoQueAtacou { get; private set; }

    public Rigidbody Rb { get => _rb; }
    public Transform Player { get => _player;  }
    public float ChasingDistance { get => AttackingDistance + _chasingDistance ; }
    public float AttackingDistance { get => _attackingDistance;  }
    public float SearchDistance { get => ChasingDistance + _searchDistance;  }
    public float PlayerDistance { get => _playerDistance;  }
    public float PreparingAttackTimer { get => _preparingAttackTimer;  }
    public float AttackTimer { get => _attackTimer;  }
    public float AttackForce { get => _attackForce; }
    public LayerMask PlayerLayerMask { get => _playerLayerMask;}






    #endregion


    protected virtual void Awake() {
        _enemyHealth = 100;

        
        setStates();

        _rb = GetComponent<Rigidbody>();
    }


    protected virtual void setStates()
    {
        States.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
        States.Add(EnemyState.Chasing, new EnemyChasingState(EnemyState.Chasing, this));
        States.Add(EnemyState.Attacking, new EnemyAttackingState(EnemyState.Attacking, this));
        States.Add(EnemyState.Hitted, new EnemyHittedState(EnemyState.Hitted, this));
        _currentState = States[EnemyState.Idle];
    }

    protected override void Update()
    {
        _playerDistance = Vector3.Distance(_player.position, transform.position);
        if (_playerDistance < _chasingDistance) bodyHitPlayer();
        base.Update();
    }

    public void Hit(GameObject hittedBy, Vector3 hitDirectionWithForce, Vector3 inpactPosition, int damage)
    {
        _enemyHealth -= damage;
        _rb.AddForce(hitDirectionWithForce , ForceMode.Impulse);
        if (_enemyHealth < 0) Killed();
        TransitionToState(EnemyState.Hitted);
        
    }

    public void Killed()
    {
        Instantiate(_enemyOrb, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected virtual void bodyHitPlayer()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, _enemyHitBox, _playerLayerMask);

        foreach (var collider in coll)
        {
            collider.GetComponent<PlayerContext>()?.Hit(this.gameObject,(collider.ClosestPoint(transform.position) - transform.position).normalized,transform.position, 5); // SO bodyDamage
        }

    }


    #region clearView

    [SerializeField] private LayerMask  _layerMask;

    public bool CanSeePlayer()
    {  
        if(Physics.SphereCast(new (transform.position, _player.position - transform.position )
            , 0.3f , out RaycastHit hit, _playerDistance, _layerMask)) return false; return true;
    }

    #endregion

    #region Gizmos

    [SerializeField, Header("Gizmos")]
    private bool DebugAttackRanges;
    

    protected virtual void OnDrawGizmosSelected()
    {
        if (!DebugAttackRanges) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _enemyHitBox);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackingDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChasingDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, SearchDistance);
    }

    
    #endregion

    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Hitted
    }
}
