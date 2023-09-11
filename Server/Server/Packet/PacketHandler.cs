using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

public class PacketHandler
{
    public static void C_StateHandler(PacketSession session, IMessage packet)
    { 
        C_State statePacket = packet as C_State;
        ClientSession clientSession = session as ClientSession;

        Player myPlayer = clientSession.MyPlayer;
        if(myPlayer == null )
        {
            return;
        }

        GameRoom room = myPlayer.Room;
        if(room == null)
        {
            return;
        }

        room.HandleState(myPlayer, statePacket);
    }
}
