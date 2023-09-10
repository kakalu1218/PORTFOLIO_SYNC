using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseController : MonoBehaviour
{
    public int Id { get; set; }

    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;

    protected Vector3 _destination;
    public Vector3 Destination
    {
        get
        {
            return _destination;
        }
        
        set
        {
            _destination = value;
            State = ObjectState.Moving;
            _navMeshAgent.destination = _destination;
        }
    }

    protected ObjectState _state;
    public virtual ObjectState State
    {
        get
        {
            return _state;
        }

        set
        {
            if (_state == value)
            {
                return;
            }

            _state = value;
        }
    }

    private const float ARRIVAL_THRESHOLD = 0.5f;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case ObjectState.Idle:
                UpdateIdle();
                break;
            case ObjectState.Moving:
                UpdateMoving();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
        //_navMeshAgent.destination = transform.position;
    }

    protected virtual void UpdateMoving()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _destination);
        if (distanceToTarget <= ARRIVAL_THRESHOLD)
        {
            State = ObjectState.Idle;
        }
    }
}
