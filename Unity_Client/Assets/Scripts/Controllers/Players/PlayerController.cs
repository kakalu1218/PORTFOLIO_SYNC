using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void FadeAnimation()
    {
        if (_animator == null)
        {
            return;
        }

        switch (State)
        {
            case ObjectState.Idle:
                _animator.CrossFade("Idle01", 0.1f);
                break;

            case ObjectState.Moving:
                _animator.CrossFade("Run", 0.1f);
                break;

            case ObjectState.Attack:
                _animator.CrossFade("Attack01", 0.1f);
                break;
        }
    }

    protected override void UpdateMoving()
    {
        if (Target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            if (distanceToTarget > ATTACK_RANGE)
            {
                // TODO :
                // 해당 부분은 동기화 어떻게 할지 생각해보세요.
                _navMeshAgent.destination = Target.transform.position;
            }
        }
    }

    protected override void UpdateAttack()
    {
        if (Target != null)
        {
            transform.LookAt(Target.transform);
        }
    }

    #region Animation Event
    public virtual void Attack()
    {
    }
    #endregion
}
