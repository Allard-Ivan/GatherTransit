using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatherTransit.Server.Model
{
    /// <summary>
    /// 实时数据对象
    /// </summary>
    [Serializable]
    public class RealData
    {
        public string key { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 采集数值 
        /// </summary>
        public string value { get; set; }

        public string status { get; set; }

    }
}
