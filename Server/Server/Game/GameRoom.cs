using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameRoom
    {
        public int RoomId { get; set; }

        private object _lock = new object();
        private List<Player> _players = new List<Player>();

        public void EnterGame(Player newPlayer)
        {
            if (newPlayer == null)
            {
                return;
            }

            lock (_lock)
            {
                _players.Add(newPlayer);
                newPlayer.Room = this;

                // 본인한테 정보 전송
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.Player = newPlayer.Info;
                    newPlayer.Session.Send(enterPacket);

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player player in _players)
                    {
                        if (player != newPlayer)
                        {
                            spawnPacket.Players.Add(player.Info);
                        }
                    }

                    newPlayer.Session.Send(spawnPacket);
                }

                // 타인한테 정보 전송
                {
                    S_Spawn spawnPakcet = new S_Spawn();
                    spawnPakcet.Players.Add(newPlayer.Info);
                    foreach (Player player in _players)
                    {
                        if (player != newPlayer)
                        {
                            player.Session.Send(spawnPakcet);
                        }
                    }
                }
            }
        }

        public void LeaveGame(int playerId)
        {
            lock (_lock)
            {
                Player leavePlayer = _players.Find(leavePlayer => leavePlayer.Info.PlayerId == playerId);
                if (leavePlayer != null)
                {
                    return;
                }

                _players.Remove(leavePlayer);
                leavePlayer.Room = null;

                // 본인한테 정보 전송
                {
                    S_LeaveGame leavePacket = new S_LeaveGame();
                    leavePlayer.Session.Send(leavePacket);
                }

                // 타인한테 정보 전송
                {
                    S_Despawn despawnPacket = new S_Despawn();
                    despawnPacket.PlayerIds.Add(leavePlayer.Info.PlayerId);
                    foreach (Player player in _players)
                    {
                        if (player != leavePlayer)
                        {
                            player.Session.Send(despawnPacket);
                        }
                    }
                }
            }
        }
    }
}
