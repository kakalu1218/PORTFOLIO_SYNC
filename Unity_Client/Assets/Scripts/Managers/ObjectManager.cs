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
            _objects.Add(info.PlayerId, gameObject);

            MyPlayerController = gameObject.GetComponent<MyPlayerController>();
            MyPlayerController.Id = info.PlayerId;
            MyPlayerController.StateInfo = info.StatInfo;
            MyPlayerController.transform.position = new Vector3(info.StatInfo.Position.X, info.StatInfo.Position.Y, info.StatInfo.Position.Z);
        }
        else
        {
            GameObject gameObject = Managers.Resource.Instantiate("Objects/Player/Player");
            gameObject.name = info.Name;
            _objects.Add(info.PlayerId, gameObject);

            PlayerController playerController = gameObject.GetComponent<PlayerController>();
            playerController.Id = info.PlayerId;
            playerController.StateInfo = info.StatInfo;
            playerController.transform.position = new Vector3(info.StatInfo.Position.X, info.StatInfo.Position.Y, info.StatInfo.Position.Z);
        }
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
        GameObject gameObject = FindById(id);
        if (gameObject == null)
        {
            return;
        }

        _objects.Remove(id);
        Managers.Resource.Destroy(gameObject);
    }

    public GameObject FindById(int id)
    {
        GameObject gameObject = null;
        _objects.TryGetValue(id, out gameObject);
        return gameObject;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject gameObject in _objects.Values)
        {
            if (condition.Invoke(gameObject))
            {
                return gameObject;
            }
        }

        return null;
    }

    public void Clear()
    {
        foreach (GameObject gameObject in _objects.Values)
        {
            Managers.Resource.Destroy(gameObject);
        }

        _objects.Clear();
    }
}
