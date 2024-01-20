using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<EnemyStateMachine.EnemyState>, IHittable /*, IGrabbable*/
{
    private int _enemyHealth;

    private Vector3 _hitDirection;


    private Rigidbody _rb;


    [SerializeField] private Transform _player;
    private float _playerDistance;

    [SerializeField] float _chasingDistance, _attackingDistance, _searchDistance;


    [Header("Attack Modifiers"), SerializeField]
    private float _preparingAttackTimer;
    [SerializeField] private float _attackTimer;

    [SerializeField] private float _attackForce;

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
    public float PreparingAttackTimer { get => _preparingAttackTimer;  }
    public float AttackTimer { get => _attackTimer;  }
    public float AttackForce { get => _attackForce; }






    #endregion


    #region grab for troll
    
    /*
    public Transform leftHandPos { get => _leftHandPos; set => _leftHandPos = value; }
    public Transform rightHandPos { get => _rightHandPos; set => _rightHandPos = value; }
    public Vector2 Offset { get => _offset; set => _offset = value; }


    [SerializeField] Transform _leftHandPos, _rightHandPos;
    [SerializeField] Vector2 _offset;

    
    public (Quaternion, Quaternion, Vector2 Offset) Grab()
    {
        
        return (leftHandPos.rotation, rightHandPos.rotation, Offset);
    }

    public void UnGrab()
    {
        
    }
    */

    #endregion


    protected virtual void Awake() {
        _enemyHealth = 100;

        
        setStates();

        _rb = GetComponent<Rigidbody>();
    }


    protected virtual void setStates()
    {
        Debug.Log("oioi");
        
        States.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
        States.Add(EnemyState.Chasing, new EnemyChasingState(EnemyState.Chasing, this));
        States.Add(EnemyState.Attacking, new EnemyAttackingState(EnemyState.Attacking, this));
        States.Add(EnemyState.Hitted, new EnemyHittedState(EnemyState.Hitted, this));
        _currentState = States[EnemyState.Idle];
    }

    protected override void Update()
    {
        _playerDistance = Vector3.Distance(_player.position, transform.position);
        base.Update();
    }

    public void Hit(GameObject coisoQueAtacou, Vector3 direction, Vector3 inpactPos, int damage, bool impact = false)
    {
        _enemyHealth -= damage;

        if (impact) {
            _rb.AddForce(direction * 10, ForceMode.Impulse);

        }

        if (_enemyHealth < 0) Killed();
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
    private bool DebugAttackRanges;
    

    protected virtual void OnDrawGizmosSelected()
    {
        if (!DebugAttackRanges) return;
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
