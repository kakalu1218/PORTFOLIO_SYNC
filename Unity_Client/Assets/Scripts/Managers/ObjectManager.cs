using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyPlayerController MyPlayerController { get; private set; }

    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public void Add(ObjectInfo info, bool isMyPlayer = false)
    {
        ObjectType type = GetObjectTypeById(info.ObjectId);

        switch (type)
        { 
            case ObjectType.Player:
                {
                    if (isMyPlayer)
                    {
                        GameObject gameObject = Managers.Resource.Instantiate("Objects/Player/MyPlayer");
                        gameObject.name = info.Name;
                        _objects.Add(info.ObjectId, gameObject);

                        MyPlayerController = gameObject.GetComponent<MyPlayerController>();
                        MyPlayerController.Id = info.ObjectId;
                        MyPlayerController.StateInfo = info.StateInfo;
                        MyPlayerController.transform.position = new Vector3(info.StateInfo.Position.X, info.StateInfo.Position.Y, info.StateInfo.Position.Z);
                    }
                    else
                    {
                        GameObject gameObject = Managers.Resource.Instantiate("Objects/Player/Player");
                        gameObject.name = info.Name;
                        _objects.Add(info.ObjectId, gameObject);

                        PlayerController playerController = gameObject.GetComponent<PlayerController>();
                        playerController.Id = info.ObjectId;
                        playerController.StateInfo = info.StateInfo;
                        playerController.transform.position = new Vector3(info.StateInfo.Position.X, info.StateInfo.Position.Y, info.StateInfo.Position.Z);
                    }
                }
                break;

            case ObjectType.Monster:
                {
                }
                break;

            case ObjectType.Projectile:
                {
                }
                break;
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

    public ObjectType GetObjectTypeById(int objectId)
    {
        int type = (objectId >> 24) & 0x7F;
        return (ObjectType)type;
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
