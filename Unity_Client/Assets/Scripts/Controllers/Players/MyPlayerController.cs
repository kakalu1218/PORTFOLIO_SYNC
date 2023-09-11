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

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<PlayerController>())
                {
                    StateInfo info = new StateInfo();
                    info.State = ObjectState.Moving;
                    info.Position = new SVector3();
                    info.Position.X = transform.position.x;
                    info.Position.Y = transform.position.y;
                    info.Position.Z = transform.position.z;
                    info.Destination = new SVector3();
                    info.Destination.X = hit.collider.gameObject.transform.position.x;
                    info.Destination.Y = hit.collider.gameObject.transform.position.y;
                    info.Destination.Z = hit.collider.gameObject.transform.position.z;
                    info.TargetId = hit.collider.gameObject.GetComponent<BaseController>().Id;

                    StateInfo = info;

                    SendStatePacket();
                }
                else
                {
                    StateInfo info = new StateInfo();
                    info.State = ObjectState.Moving;
                    info.Position = new SVector3();
                    info.Position.X = transform.position.x;
                    info.Position.Y = transform.position.y;
                    info.Position.Z = transform.position.z;
                    info.Destination = new SVector3();
                    info.Destination.X = hit.point.x;
                    info.Destination.Y = hit.point.y;
                    info.Destination.Z = hit.point.z;
                    info.TargetId = -1;

                    StateInfo = info;

                    SendStatePacket();
                }
            }
        }
    }

    protected override void UpdateMoving()
    {
        if (Target == null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, Destination);
            if (distanceToTarget <= ARRIVAL_THRESHOLD)
            {
                StateInfo info = new StateInfo();
                info.State = ObjectState.Idle;
                info.Position = new SVector3();
                info.Position.X = transform.position.x;
                info.Position.Y = transform.position.y;
                info.Position.Z = transform.position.z;
                info.Destination = new SVector3();
                info.Destination.X = transform.position.x;
                info.Destination.Y = transform.position.y;
                info.Destination.Z = transform.position.z;
                info.TargetId = -1;

                StateInfo = info;

                SendStatePacket();
            }
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            if (distanceToTarget <= ATTACK_RANGE)
            {
                StateInfo info = new StateInfo();
                info.State = ObjectState.Attack;
                info.Position = new SVector3();
                info.Position.X = transform.position.x;
                info.Position.Y = transform.position.y;
                info.Position.Z = transform.position.z;
                info.Destination = new SVector3();
                info.Destination.X = transform.position.x;
                info.Destination.Y = transform.position.y;
                info.Destination.Z = transform.position.z;
                info.TargetId = Target.GetComponent<BaseController>().Id;

                StateInfo = info;

                SendStatePacket();
            }
        }
    }

    protected override void UpdateAttack()
    {
        if (Target == null)
        {
            StateInfo info = new StateInfo();
            info.State = ObjectState.Idle;
            info.Position = new SVector3();
            info.Position.X = transform.position.x;
            info.Position.Y = transform.position.y;
            info.Position.Z = transform.position.z;
            info.Destination = new SVector3();
            info.Destination.X = transform.position.x;
            info.Destination.Y = transform.position.y;
            info.Destination.Z = transform.position.z;
            info.TargetId = -1;

            StateInfo = info;

            SendStatePacket();
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);
            if (distanceToTarget > ATTACK_RANGE)
            {
                StateInfo info = new StateInfo();
                info.State = ObjectState.Moving;
                info.Position = new SVector3();
                info.Position.X = transform.position.x;
                info.Position.Y = transform.position.y;
                info.Position.Z = transform.position.z;
                info.Destination = new SVector3();
                info.Destination.X = Target.transform.position.x;
                info.Destination.Y = Target.transform.position.y;
                info.Destination.Z = Target.transform.position.z;
                info.TargetId = Target.GetComponent<BaseController>().Id;

                StateInfo = info;

                SendStatePacket();
            }
        }
    }

    private void SendStatePacket()
    {
        C_State statePacket = new C_State();
        statePacket.StatInfo = StateInfo;

        Managers.Network.Send(statePacket);
    }

    #region Animation Event
    public override void Attack()
    {
    }
    #endregion
}
