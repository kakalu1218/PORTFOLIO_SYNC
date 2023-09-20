using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Game;
using System.Numerics;
using Server.Data;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public Player MyPlayer { get; set; }
        public int SessionId { get; set; }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            // PROTO Test
            MyPlayer = ObjectManager.Instance.Add<Player>();
            {
                MyPlayer.Info.Name = $"Player_{MyPlayer.Info.ObjectId}";

                MyPlayer.Info.StateInfo = new StateInfo();
                MyPlayer.Info.StateInfo.State = ObjectState.Idle;
                MyPlayer.Info.StateInfo.Position = new SVector3();
                MyPlayer.Info.StateInfo.Position.X = 0.0f;
                MyPlayer.Info.StateInfo.Position.Y = 0.0f;
                MyPlayer.Info.StateInfo.Position.Z = 0.0f;
                MyPlayer.Info.StateInfo.Destination = new SVector3();
                MyPlayer.Info.StateInfo.Destination.X = 0.0f;
                MyPlayer.Info.StateInfo.Destination.Y = 0.0f;
                MyPlayer.Info.StateInfo.Destination.Z = 0.0f;
                MyPlayer.Info.StateInfo.TargetId = -1;

                StatInfo statInfo = null;
                DataManager.StatDict.TryGetValue(1, out statInfo);
                MyPlayer.Info.StatInfo.MergeFrom(statInfo);

                MyPlayer.Session = this;
            }

            RoomManager.Instance.Find(1).EnterGame(MyPlayer);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            RoomManager.Instance.Find(1).LeaveGame(MyPlayer.Info.ObjectId);

            SessionManager.Instance.Remove(this);

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
