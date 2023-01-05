using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int _maxShield = 100;
    public int currentShield;
    [SerializeField] private int _maxArmour = 100;
    public int currentArmour;
    [SerializeField] private int _maxIntegrity = 100;
    public int currentIntegrity;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;
    //[SerializeField] private GunSystem.GunType _gunType;

    public static LayerMask WhatIsEnvironment;
    private NavMeshAgent _agent;
    private Vector3 _destination;

    private States _currentState = States.Initialise;
    [SerializeField] private PlayerData _playerData;
    private float _distanceToPlayer;
    
    [SerializeField] private float attackDelay = 1f;
    private float attackTimer;

    private enum States
    {
        Initialise,
        MoveTowards,
        Engage,
        PerformAttack,
        Evade,
        Death
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (_currentState)
        {
            case States.Initialise:
                HandleInitialise();
                break;
            case States.MoveTowards:
                HandleMoveTowards();
                break;
            case States.Engage:
                HandleEngage();
                break;
            case States.PerformAttack:
                HandlePerformAttack();
                break;
            case States.Evade:
                HandleEvade();
                break;
            case States.Death:
                HandleDeath();
                break;
        }
    }

    private void EnterState()
    {
        switch (_currentState)
        {
            case States.Initialise:
                InitialiseEnter();
                break;
            case States.MoveTowards:
                MoveTowardsEnter();
                break;
            case States.Engage:
                EngageEnter();
                break;
            case States.PerformAttack:
                PerformAttackEnter();
                break;
            case States.Evade:
                EvadeEnter();
                break;
            case States.Death:
                DeathEnter();
                break;
        }
    }

    #region INITIALISE
    
    private void InitialiseEnter()
    {
        
    }

    private void HandleInitialise()
    {
        _distanceToPlayer = CalculateDistanceToPlayer();
        
        //Set movement destination
        _destination = _playerData.position;
        _agent.destination = _destination;
        
        SetState(States.MoveTowards);
        return;
    }
    
    #endregion

    #region MOVE_TOWARDS

    private void MoveTowardsEnter()
    {
        
    }

    private void HandleMoveTowards()
    {
        _distanceToPlayer = CalculateDistanceToPlayer();

        //If the player has moved enough, update the agent's movement target
        if (Vector3.Magnitude(_destination - _playerData.position) >= 1f)
        {
            _destination = _playerData.position;
            _agent.destination = _destination;
        }
        
        if (_distanceToPlayer <= _attackRange)
        {
            SetState(States.Engage);
            return;
        }
    }

    #endregion
    
    #region ENGAGE

    private void EngageEnter()
    {
        
    }

    private void HandleEngage()
    {
        _distanceToPlayer = CalculateDistanceToPlayer();
        
        //Checks if a raycast towards the player hits any environment object.
        var directionTowardsPlayer = _playerData.position - transform.position;
        var isSightLineBlocked = Physics.Raycast(transform.position, directionTowardsPlayer,
            _distanceToPlayer, WhatIsEnvironment);

        if (_distanceToPlayer > _attackRange)
        {
            SetState(States.MoveTowards);
            return;
        }
        else if (!isSightLineBlocked)
        {
            SetState(States.PerformAttack);
            attackTimer = Time.deltaTime;
            return;
        }

        if ((Time.deltaTime > attackTimer + attackDelay) && /*Is being attacked*/)
        {
            SetState(States.Evade);
            return;
        }

        //Move in Circle around player between **PerformAttack**
        _destination = _agent.
    }

    #endregion
    
    #region PERFORM_ATTACK

    private void PerformAttackEnter()
    {
        
    }

    private void HandlePerformAttack()
    {
        //Use GunType and Attack
        
        SetState(States.Engage);
        return;
    }

    #endregion
    
    #region EVADE

    private void EvadeEnter()
    {
        
    }

    private void HandleEvade()
    {
        //Unique for each enemy
        //Base: Dash around player towards Flank
        //Slerp
        //Once Evade is finished Goto -> **MoveTowards** or **Engage**
        if (_distanceToPlayer > _attackRange)
        {
            SetState(States.MoveTowards);
            return;
        }
        else
        {
            SetState(States.Engage);
            return;
        }
    }

    #endregion
    
    #region DEATH

    private void DeathEnter()
    {
        
    }

    private void HandleDeath()
    {
        
    }

    #endregion
    
    private float CalculateDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, _playerData.position);
    }

    private void SetState(States newState)
    {
        _currentState = newState;
        EnterState();
    }
}
