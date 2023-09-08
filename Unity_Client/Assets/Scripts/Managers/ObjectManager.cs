using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayerController { get; private set; }

    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public void Add(PlayerInfo info, bool isMyPlayer = false)
    {
        if (isMyPlayer)
        {
            GameObject gameObject = Managers.Resource.Instantiate("Objects/Player/MyPlayer");
            gameObject.name = info.Name;
            gameObject.transform.position = new Vector3(info.PosX, info.PosY, info.PosZ);
            _objects.Add(info.PlayerId, gameObject);

            MyPlayerController = gameObject.GetComponent<MyPlayerController>();
            MyPlayerController.Id = info.PlayerId;
        }
        else
        {
            GameObject gameObject = Managers.Resource.Instantiate("Objects/Player/Player");
            gameObject.name = info.Name;
            gameObject.transform.position = new Vector3(info.PosX, info.PosY, info.PosZ);
            _objects.Add(info.PlayerId, gameObject);

            PlayerController playerController = gameObject.GetComponent<PlayerController>();
            playerController.Id = info.PlayerId;
        }
    }

    public void Add(int id, GameObject gameObject)
    {
        _objects.Add(id, gameObject);
    }

    public void RemoveMyPlayer()
    {
        if (MyPlayerController != null)
        {
            return;
        }

        Remove(MyPlayerController.Id);
        MyPlayerController = null;
    }

    public void Remove(int id)
    {
        _objects.Remove(id);
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            if (condition.Invoke(obj))
            { 
                return obj;
            }
        }

        return null;
    }

    public void Clear()
    {
        _objects.Clear();
    }
}
