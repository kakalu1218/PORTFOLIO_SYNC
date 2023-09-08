using Cinemachine;
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
                _desPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                State = Define.ObjectState.Moving;
            }
        }
    }
}
