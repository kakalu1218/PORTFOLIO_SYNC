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
        } 
    }
}
