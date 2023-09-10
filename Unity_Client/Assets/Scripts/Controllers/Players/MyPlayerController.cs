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
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                SendMovePacket();
            }
        }
    }

    private void SendMovePacket()
    {
        C_Move movePacket = new C_Move();
        SVector3 destination = new SVector3();
        destination.X = Destination.x;
        destination.Y = Destination.y;
        destination.Z = Destination.z;
        movePacket.Destination = destination;
        Managers.Network.Send(movePacket);
    }
}
