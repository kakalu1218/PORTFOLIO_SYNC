using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

public class PacketHandler
{
	public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;

        Console.WriteLine($"C_Move ({movePacket.Destination.X}, {movePacket.Destination.Y}, {movePacket.Destination.Z})");

		if (clientSession.MyPlayer == null)
		{
			return;
		}

		if (clientSession.MyPlayer.Room == null)
		{
			return;
		}

        // TODO : 검증

        // 일단 서버에서 목적지 설정
        PlayerInfo info = clientSession.MyPlayer.Info;
		info.Destination = movePacket.Destination;

		// 다른 플레이어한테도 알려준다.
		S_Move resMovePacket = new S_Move();
		resMovePacket.PlayerId = clientSession.MyPlayer.Info.PlayerId;
		resMovePacket.Destination = movePacket.Destination;

		clientSession.MyPlayer.Room.Broadcast(resMovePacket);
    }
}
