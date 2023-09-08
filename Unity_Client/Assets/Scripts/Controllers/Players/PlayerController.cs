using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    protected override Define.ObjectState State
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
                case Define.ObjectState.Idle:
                    //_animator.CrossFade("IDLE", 0.1f);
                    break;

                case Define.ObjectState.Moving:
                    //_animator.CrossFade("MOVE", 0.1f);
                    break;
            }
        }
    }

    protected override void Init()
    {
        base.Init();
    }
}
