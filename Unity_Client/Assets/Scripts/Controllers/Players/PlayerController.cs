using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    public override ObjectState State
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

            switch (_state)
            {
                case ObjectState.Idle:
                    _animator.CrossFade("Idle01", 0.1f);
                    break;

                case ObjectState.Moving:
                    _animator.CrossFade("Run", 0.1f);
                    break;
            }
        }
    }

    protected override void Init()
    {
        base.Init();
    }
}
