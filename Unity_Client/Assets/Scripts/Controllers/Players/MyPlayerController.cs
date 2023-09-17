using Cinemachine;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : PlayerController
{
    protected override void Init()
    {
        base.Init();

        FindAnyObjectByType<CinemachineVirtualCamera>().Follow = transform;
    }

    protected override void UpdateController()
    {
        base.UpdateController();

        GetMouseInput();
    }

    private void SetStateInfo(ObjectState state, Vector3 pos, Vector3 des, GameObject target = null, Action action = null)
    {
        StateInfo info = new StateInfo();
        info.State = state;
        info.Position = new SVector3();
        info.Position.X = pos.x;
        info.Position.Y = pos.y;
        info.Position.Z = pos.z;
        info.Destination = new SVector3();
        info.Destination.X = des.x;
        info.Destination.Y = des.y;
        info.Destination.Z = des.z;
        info.TargetId = target == null ? -1 : target.GetComponent<BaseController>().Id;

        StateInfo = info;

        action?.Invoke();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject != gameObject)
            {
                if (hit.collider.gameObject.GetComponent<PlayerController>())
                {
                    SetStateInfo(ObjectState.Moving, transform.position, hit.collider.gameObject.transform.position, hit.collider.gameObject, () => { SendStatePacket(); });
                }
                else
                {
                    SetStateInfo(ObjectState.Moving, transform.position, hit.point, target: null, () => { SendStatePacket(); });
                }
            }
        }
    }

    private float movingPacketTimer = 0.0f;
    private const float MOVINGPACKET_DELAY = 0.5f;

    protected override void UpdateMoving()
    {
        base.UpdateMoving();

        if (Target == null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, Destination);
            if (distanceToTarget <= ARRIVAL_THRESHOLD)
            {
                SetStateInfo(ObjectState.Idle, transform.position, transform.position, target: null, () => { SendStatePacket(); });
            }
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            if (distanceToTarget <= ATTACK_RANGE)
            {
                SetStateInfo(ObjectState.Attack, transform.position, transform.position, Target, () => { SendStatePacket(); });
            }
            else
            {
                movingPacketTimer += Time.deltaTime;
                if (movingPacketTimer > MOVINGPACKET_DELAY)
                {
                    movingPacketTimer = 0.0f;
                    SetStateInfo(ObjectState.Moving, transform.position, Target.transform.position, Target, () => { SendStatePacket(); });
                }
            }
        }
    }

    protected override void UpdateAttack()
    {
        base.UpdateAttack();

        if (Target == null)
        {
            SetStateInfo(ObjectState.Idle, transform.position, transform.position, target: null, () => { SendStatePacket(); });
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            if (distanceToTarget > ATTACK_RANGE)
            {
                SetStateInfo(ObjectState.Moving, transform.position, Target.transform.position, Target, () => { SendStatePacket(); });
            }
        }
    }

    private void SendStatePacket()
    {
        C_State statePacket = new C_State();
        statePacket.StatInfo = StateInfo;

        Managers.Network.Send(statePacket);
    }

    private void SendNormalHitPacket()
    {
        C_NormalHit normalHitPacket = new C_NormalHit();
        Managers.Network.Send(normalHitPacket);
    }

    #region Animation Event
    public override void Attack()
    {
        SendNormalHitPacket();
    }
    #endregion
}
