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

    #region Animation Event
    public virtual void Attack()
    {
    }
    #endregion
}
