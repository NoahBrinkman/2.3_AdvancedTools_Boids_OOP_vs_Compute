using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    [Serializable]
    public class LogList
    {
        public List<Logger.LogData> lD;

        public LogList()
        {
            lD = new List<Logger.LogData>();
        }
    }
}