using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<EnemyStateMachine.EnemyState>, IHittable
{
    private int _enemyHealth;

    private Vector3 _hitDirection;


    private Rigidbody _rb;


    [SerializeField] private Transform _player;
    private float _playerDistance;

    [SerializeField] float _chasingDistance, _attackingDistance, _searchDistance;

    #region gets & sets
    public string Name { get; private set; }
    public GameObject coisoQueAtacou { get; private set; }

    public Vector3 HitDirection { get => _hitDirection; }
    public Rigidbody Rb { get => _rb; }
    public Transform Player { get => _player;  }
    public float ChasingDistance { get => AttackingDistance + _chasingDistance ; }
    public float AttackingDistance { get => _attackingDistance;  }
    public float SearchDistance { get => ChasingDistance + _searchDistance;  }
    public float PlayerDistance { get => _playerDistance;  }
    #endregion


    private void Awake() {
        void setStates() {
            States.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
            States.Add(EnemyState.Chasing, new EnemyChasingState(EnemyState.Chasing, this));
            States.Add(EnemyState.Attacking, new EnemyAttackingState(EnemyState.Attacking, this));
            States.Add(EnemyState.Hitted, new EnemyHittedState(EnemyState.Hitted, this));
            _currentState = States[EnemyState.Idle];
        }
        setStates();

        _rb = GetComponent<Rigidbody>();
    }


    protected override void Update()
    {
        _playerDistance = Vector3.Distance(_player.position, transform.position);
        base.Update();
    }

    public void Hit(GameObject coisoQueAtacou, Vector3 direction, Vector3 inpactPos, int damage)
    {
        TransitionToState(EnemyState.Hitted);
        _hitDirection = direction;
        _rb.AddForce(_hitDirection * 50, ForceMode.Force);
        
    }

    public void Killed()
    {
        Destroy(this.gameObject);
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
    private bool Debug;
    

    protected virtual void OnDrawGizmosSelected()
    {
        if (!Debug) return;
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
