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

    private StateInfo _stateInfo;
    public StateInfo StateInfo
    {
        get
        {
            return _stateInfo;
        }

        set
        {
            _stateInfo = value;

            State = StateInfo.State;
            Destination = new Vector3(StateInfo.Destination.X, StateInfo.Destination.Y, StateInfo.Destination.Z);
            Target = Managers.Object.FindById(StateInfo.TargetId);
        }
    }

    private ObjectState _state;
    public ObjectState State
    {
        get
        {
            return _state;
        }

        private set
        {
            if (_state == value)
            {
                return;
            }

            _state = value;

            FadeAnimation();
        }
    }

    protected virtual void FadeAnimation()
    {

    }

    private Vector3 _destination;
    public Vector3 Destination
    {
        get
        {
            return _destination;
        }

        private set
        {
            _destination = value;

            if (_navMeshAgent == null)
            {
                return;
            }

            _navMeshAgent.destination = _destination;
        }
    }

    private GameObject _target;
    public GameObject Target
    {
        get
        {
            return _target;
        }

        private set
        {
            if (_target == value)
            {
                return;
            }

            _target = value;
        }
    }

    private StatInfo _statInfo = new StatInfo();
    public StatInfo StatInfo
    {
        get
        {
            return _statInfo; 
        }
        
        set
        {
            if (_statInfo.Equals(value))
            { 
                return;
            }

            _statInfo.Hp = value.Hp;
            _statInfo.MaxHp = value.MaxHp;
            _statInfo.Attack = value.Attack;
            _statInfo.TotalExp = value.TotalExp;

            UpdateHpBar();
        }
    }

    public int Hp
    {
        get 
        { 
            return StatInfo.Hp; 
        }

        set
        {
            StatInfo.Hp = value;

            UpdateHpBar();
        }
    }

    protected const float ARRIVAL_THRESHOLD = 0.5f;
    protected const float ATTACK_RANGE = 3.0f;

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
        _animator = GetComponent<Animator>();
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

            case ObjectState.Attack:
                UpdateAttack();
                break;

            case ObjectState.Skill:
                UpdateSkill();
                break;
        }
    }

    private HpBar _hpBar;

    private void UpdateHpBar()
    {
        if (_hpBar == null)
        { 
            return;
        }

        float ratio = 0.0f;
        if (StatInfo.MaxHp > 0)
        { 
            ratio = ((float)Hp) / StatInfo.MaxHp;
        }

        _hpBar.SetHpBarRatio(ratio);
    }

    protected void AddHpBar()
    {
        GameObject gameObject = Managers.Resource.Instantiate("UI/WorldSpace/HpBar", transform);
        gameObject.transform.localPosition = new Vector3(0, 2.5f, 0);
        gameObject.name = "HpBar";
        _hpBar = gameObject.GetComponent<HpBar>();
        UpdateHpBar();
    }

    protected virtual void UpdateIdle()
    {

    }

    protected virtual void UpdateMoving()
    {
        
    }

    protected virtual void UpdateAttack()
    {

    }

    protected virtual void UpdateSkill()
    {

    }
}
