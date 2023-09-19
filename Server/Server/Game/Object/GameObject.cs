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
        public ObjectInfo Info { get; set; } = new ObjectInfo();
        public StateInfo StateInfo { get; private set; } = new StateInfo() { Position = new SVector3(), Destination = new SVector3() };
        public StatInfo StatInfo { get; private set; } = new StatInfo();

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

        public GameObject()
        {
            Info.StateInfo = StateInfo;
            Info.StatInfo = StatInfo;
        }

        public virtual void OnDamaged(GameObject instigator, int damage)
        {
            StatInfo.Hp = Math.Max(StatInfo.Hp - damage, 0);

            S_ChangeHp changeHpPacket = new S_ChangeHp();
            changeHpPacket.ObjectId = Id;
            changeHpPacket.Hp = StatInfo.Hp;
            Room.Broadcast(changeHpPacket);

            if (StatInfo.Hp <= 0)
            {
                OnDead(instigator);
            }
        }

        public virtual void OnDead(GameObject instigator)
        { 
        }
    }
}
