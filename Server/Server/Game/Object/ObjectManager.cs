using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class ObjectManager
    {
        public static ObjectManager Instance { get; } = new ObjectManager();

        private object _lock = new object();
        private Dictionary<int, Player> _players = new Dictionary<int, Player>();

        // [UNUSED(1)| TYPE(7)  | ID(24)             ]
        // [........ | ........ | ........ | ........]
        private int _counter = 0;

        public T Add<T>() where T : GameObject, new()
        {
            T gameObject = new T();

            lock (_lock)
            {
                gameObject.Id = GeneratedId(gameObject.ObjectType);

                if (gameObject.ObjectType == ObjectType.Player)
                {
                    _players.Add(gameObject.Id, gameObject as Player);
                }
            }

            return gameObject;
        }

        private int GeneratedId(ObjectType type)
        {
            lock (_lock)
            {
                return ((int)type << 24) | (_counter++);
            }
        }

        public bool Remove(int objectId)
        {
            ObjectType objectType = GetObjectTypeById(objectId);

            lock (_lock)
            {
                if (objectType == ObjectType.Player)
                {
                    return _players.Remove(objectId);
                }

                return false;
            }
        }

        public Player Find(int objectId)
        {
            ObjectType objectType = GetObjectTypeById(objectId);

            lock (_lock)
            {
                if (objectType == ObjectType.Player)
                {
                    Player player = null;
                    if (_players.TryGetValue(objectId, out player))
                    {
                        return player;
                    }
                }

                return null;
            }
        }

        public ObjectType GetObjectTypeById(int objectId)
        {
            int type = (objectId >> 24) & 0x7F;
            return (ObjectType)type;
        }
    }
}
