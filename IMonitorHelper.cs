using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitor
{
    public interface IMonitorHelper
    {
        double GetDiskWritesCounter();

        double GetDiskReadsCounter();

        /// <summary>
        /// 获取cpu占用量
        /// 单位%
        /// </summary>
        /// <returns></returns>
        double GetCpuUsage();

        /// <summary>
        /// 获取当前网络占用量
        /// 单位%
        /// </summary>
        /// <returns></returns>
        double GetNetworkInfo();
        
        /// <summary>
        /// 获取当前下载速度
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        double GetNetworkDownSpeed();
        
        /// <summary>
        /// 获取当前上传速度
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        double GetNetworkUpSpeed();

        /// <summary>
        /// 获取总共的内存量
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        double GetTotalMemory();

        /// <summary>
        /// 获取当前可用的内存量
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        double GetAvailableMemory();
    }
}
