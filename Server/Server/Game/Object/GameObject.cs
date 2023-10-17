using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameObject
    {
        public ObjectType ObjectType { get; protected set; } = ObjectType.None;
        public GameRoom Room { get; set; }
        public ObjectInfo Info { get; set; } = new ObjectInfo() { StateInfo = new StateInfo() { Position = new SVector3(), Destination = new SVector3() }, StatInfo = new StatInfo() };

        public int Id
        {
            get
            {
                return Info.ObjectId;
            }

            set
            { 
                Info.ObjectId = value;
            }
        }

        public virtual void Update()
        { 
        }

        public virtual void OnDamaged(GameObject instigator, int damage)
        {
            Info.StatInfo.Hp = Math.Max(Info.StatInfo.Hp - damage, 0);

            S_ChangeHp changeHpPacket = new S_ChangeHp();
            changeHpPacket.ObjectId = Id;
            changeHpPacket.Hp = Info.StatInfo.Hp;
            Room.Broadcast(changeHpPacket);

            if (Info.StatInfo.Hp <= 0)
            {
                OnDead(instigator);
            }
        }

        public virtual void OnDead(GameObject instigator)
        { 
            S_Die diePacket = new S_Die();
            diePacket.ObjectId = Id;
            diePacket.InstigatorId = instigator.Id;
            
            Room.Broadcast(diePacket);

            GameRoom room = Room;
            room.LeaveGame(Id);

            Info.StatInfo.Hp = Info.StatInfo.MaxHp;
            Info.StateInfo.State = ObjectState.Idle;
            Info.StateInfo.Position = new SVector3();
            Info.StateInfo.Position.X = 0.0f;
            Info.StateInfo.Position.Y = 0.0f;
            Info.StateInfo.Position.Z = 0.0f;
            Info.StateInfo.Destination = new SVector3();
            Info.StateInfo.Destination.X = 0.0f;
            Info.StateInfo.Destination.Y = 0.0f;
            Info.StateInfo.Destination.Z = 0.0f;
            Info.StateInfo.TargetId = -1;

            room.EnterGame(this);
        }
    }
}
