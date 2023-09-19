using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;

        Managers.Object.Add(enterGamePacket.Player, isMyPlayer: true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

        Managers.Object.RemoveMyPlayer();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (ObjectInfo obj in spawnPacket.Objects)
        {
            Managers.Object.Add(obj, isMyPlayer: false);
        }
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;

        foreach (int id in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(id);
        }
    }

    public static void S_StateHandler(PacketSession session, IMessage packet)
    {
        S_State statePacket = packet as S_State;

        GameObject gameObject =Managers.Object.FindById(statePacket.ObjectId);
        if (gameObject == null)
        {
            return;
        }

        BaseController controller = gameObject.GetComponent<BaseController>();
        if(controller == null)
        {
            return;
        }

        controller.StateInfo = statePacket.StatInfo;
    }

    public static void S_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        S_ChangeHp changeHpPacket = packet as S_ChangeHp;

        GameObject gameObject = Managers.Object.FindById(changeHpPacket.ObjectId);
        if (gameObject == null)
        { 
            return;
        }

        BaseController controller = gameObject.GetComponent<BaseController>();
        if (controller == null)
        {
            return;
        }

        controller.Hp = changeHpPacket.Hp;
    }
}
