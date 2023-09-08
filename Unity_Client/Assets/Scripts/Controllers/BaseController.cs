using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseController : MonoBehaviour
{
    public int Id { get; set; }

    //protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;
    protected Vector3 _desPoint;

    protected Define.ObjectState _state;
    protected virtual Define.ObjectState State
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
        //_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case Define.ObjectState.Idle:
                UpdateIdle();
                break;
            case Define.ObjectState.Moving:
                UpdateMoving();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
        _navMeshAgent.destination = transform.position;
    }

    protected virtual void UpdateMoving()
    {
        _navMeshAgent.destination = _desPoint;
    }
}
