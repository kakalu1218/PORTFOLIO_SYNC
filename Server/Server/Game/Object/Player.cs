using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Player : GameObject
    {
        public ClientSession Session { get; set; }

        public Player()
        { 
            ObjectType = ObjectType.Player;
        }

        public override void OnDamaged(GameObject instigator, int damage)
        {
            base.OnDamaged(instigator, damage);
        }

        public override void OnDead(GameObject instigator)
        {
            base.OnDead(instigator);
        }
    }
}
