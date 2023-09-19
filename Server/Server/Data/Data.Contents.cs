using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Data
{
    public class Data
    {
        #region Stat
        [Serializable]
        public class StatData : ILoader<int, StatInfo>
        {
            public List<StatInfo> stats = new List<StatInfo>();

            public Dictionary<int, StatInfo> MakeDict()
            {
                Dictionary<int, StatInfo> dict = new Dictionary<int, StatInfo>();
                foreach (StatInfo stat in stats)
                {
                    stat.Hp = stat.MaxHp;
                    dict.Add(stat.Level, stat);
                }

                return dict;
            }
        }
        #endregion
    }
}
