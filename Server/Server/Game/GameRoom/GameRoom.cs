using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    public class GameRoom
    {
        public int RoomId { get; set; }

        private object _lock = new object();

        private Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        private Dictionary<int, Projectile> _projectiles = new Dictionary<int, Projectile>();

        public void Init()
        {
        }

        public void Update()
        {
            lock (_lock)
            {
                foreach (Monster monster in _monsters.Values)
                {
                    monster.Update();
                }

                foreach (Projectile projectile in _projectiles.Values)
                {
                    projectile.Update();
                }
            }
        }

        public Player FindPlayer(Func<GameObject, bool> condition)
        { 
            foreach(Player player in _players.Values)
            {
                if (condition.Invoke(player))
                { 
                    return player;
                }
            }

            return null;
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            ObjectType type = ObjectManager.Instance.GetObjectTypeById(gameObject.Id);

            lock (_lock)
            {
                switch (type)
                {
                    case ObjectType.Player:
                        {
                            Player player = gameObject as Player;
                            _players.Add(gameObject.Id, player);
                            player.Room = this;

                            // 본인한테 정보 전송
                            {
                                S_EnterGame enterPacket = new S_EnterGame();
                                enterPacket.Player = player.Info;
                                player.Session.Send(enterPacket);

                                S_Spawn spawnPacket = new S_Spawn();
                                foreach (Player p in _players.Values)
                                {
                                    if (player != p)
                                    {
                                        spawnPacket.Objects.Add(p.Info);
                                    }
                                }

                                foreach (Monster monster in _monsters.Values)
                                { 
                                    spawnPacket.Objects.Add(monster.Info);
                                }

                                foreach (Projectile projectile in _projectiles.Values)
                                { 
                                    spawnPacket.Objects.Add(projectile.Info);
                                }

                                player.Session.Send(spawnPacket);
                            }
                        }
                        break;

                    case ObjectType.Monster:
                        {
                            Monster monster = gameObject as Monster;
                            _monsters.Add(gameObject.Id, monster);
                            monster.Room = this;
                        }
                        break;

                    case ObjectType.Projectile:
                        {
                            Projectile projectile = gameObject as Projectile;
                            _projectiles.Add(gameObject.Id, projectile);
                            projectile.Room = this;
                        }
                        break;
                }

                // 타인한테 정보 전송
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.Objects.Add(gameObject.Info);
                    foreach (Player player in _players.Values)
                    {
                        if (player.Id != gameObject.Id)
                        {
                            player.Session.Send(spawnPacket);
                        }
                    }
                }
            }
        }

        public void LeaveGame(int objectId)
        {
            ObjectType type = ObjectManager.Instance.GetObjectTypeById(objectId);

            lock(_lock)
            {
                switch(type)
                {
                    case ObjectType.Player:
                        {
                            Player player = null;
                            if (_players.Remove(objectId, out player) == false)
                            { 
                                return;
                            }

                            player.Room = null;

                            // 본인한테 정보 전송
                            {
                                S_LeaveGame leavePacket = new S_LeaveGame();
                                player.Session.Send(leavePacket);
                            }
                        }
                        break;

                    case ObjectType.Monster:
                        {
                            Monster monster = null;
                            if (_monsters.Remove(objectId, out monster) == false)
                            { 
                                return;
                            }

                            monster.Room = null;
                        }
                        break;

                    case ObjectType.Projectile:
                        {
                            Projectile projectile = null;
                            if (_projectiles.Remove(objectId, out projectile) == false)
                            { 
                                return;
                            }

                            projectile.Room = null;
                        }
                        break;
                }

                // 타인한테 정보 전송
                {
                    S_Despawn despawnPacket = new S_Despawn();
                    despawnPacket.ObjectIds.Add(objectId);
                    foreach (Player p in _players.Values)
                    {
                        if (p.Id != objectId)
                        { 
                            p.Session.Send(despawnPacket);
                        }
                    }
                }
            }
        }

        public void HandleState(Player myPlayer, C_State statePacket)
        {
            if (myPlayer == null)
            {
                return;
            }

            lock (_lock)
            {
                ObjectInfo info = myPlayer.Info;
                info.StateInfo = statePacket.StatInfo;

                S_State resStatePacket = new S_State();
                resStatePacket.ObjectId = myPlayer.Info.ObjectId;
                resStatePacket.StatInfo = statePacket.StatInfo;

                Broadcast(resStatePacket);
            }
        }

        public void HandleNormalHit(Player myPlayer, C_NormalHit normalHitPacket)
        {
            if (myPlayer == null)
            {
                return;
            }

            lock (_lock)
            {
                if (myPlayer.Info.StateInfo.TargetId != -1)
                {
                    ObjectType targetType = ObjectManager.Instance.GetObjectTypeById(myPlayer.Info.StateInfo.TargetId);
                    switch (targetType)
                    {
                        case ObjectType.Player:
                            {
                                Player target = null;
                                if (_players.TryGetValue(myPlayer.Info.StateInfo.TargetId, out target))
                                {
                                    // TODO : 공격 거리 Check..
                                    target.OnDamaged(myPlayer, myPlayer.Info.StatInfo.Attack);
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void Broadcast(IMessage packet)
        {
            lock (_lock)
            {
                foreach (Player player in _players.Values)
                {
                    player.Session.Send(packet);
                }
            }
        }
    }
}
