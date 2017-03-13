using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;


namespace SystemMonitor
{
    public class WindowsMonitorHelper:IMonitorHelper
    {

        private static PerformanceCounter cpuPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private static PerformanceCounter diskReadsPerformanceCounter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
        private static PerformanceCounter diskWritesformanceCounter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
        private static PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
        private static string[] netCardInstancename = category.GetInstanceNames();

        private static ComputerInfo ci = new ComputerInfo();

        /// <summary>
        /// 获取磁盘读取速度 单位Byte/s
        /// </summary>
        /// <returns></returns>
        public double GetDiskReadsCounter()
        {
            return diskReadsPerformanceCounter.NextValue();
        }

        /// <summary>
        /// 获取磁盘写入速度 单位Byte/s
        /// </summary>
        /// <returns></returns>
        public double GetDiskWritesCounter()
        {
            return diskWritesformanceCounter.NextValue();
        }

        /// <summary>
        /// 获取cpu占用量
        /// 单位%
        /// </summary>
        /// <returns></returns>
        public double GetCpuUsage()
        {
            var result = cpuPerformanceCounter.NextValue();
            return result;
        }

        /// <summary>
        /// 获取当前网络占用量
        /// 单位%
        /// </summary>
        /// <returns></returns>
        public double GetNetworkInfo()
        {
            double result = 0;
            foreach (var inst in netCardInstancename)
            {
                var num = GetNetworkUtilization(inst);
                result += num;
            }
            return result;
        }

        /// <summary>
        /// 获取当前下载速度
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        public double GetNetworkDownSpeed()
        {
            double result = 0;
            foreach (var inst in netCardInstancename)
            {
                var num = GetCurrentNetworkDownSpeed(inst);
                result += num;
            }
            return result;
        }

        /// <summary>
        /// 获取当前上传速度
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        public double GetNetworkUpSpeed()
        {
            double result = 0;
            foreach (var inst in netCardInstancename)
            {
                var num = GetCurrentNetworkUpSpeed(inst);
                result += num;
            }
            return result;
        }

        /// <summary>
        /// 获取总共的内存量
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        public double GetTotalMemory()
        {
            ulong me = ci.TotalPhysicalMemory;
            return Convert.ToDouble(me);
        }

        /// <summary>
        /// 获取当前可用的内存量
        /// 单位Byte
        /// </summary>
        /// <returns></returns>
        public double GetAvailableMemory()
        {
            ulong av = ci.AvailablePhysicalMemory;
            return Convert.ToDouble(av);
        }

        /// <summary>
        /// 根据网卡名称获取各个网卡的占用量
        /// </summary>
        /// <param name="networkCard">网卡名称</param>
        /// <returns></returns>
        private double GetNetworkUtilization(string networkCard)
        {

            const int numberOfIterations = 10;

            PerformanceCounter bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkCard);
            float bandwidth = bandwidthCounter.NextValue();//valor fixo 10Mb/100Mn/

            PerformanceCounter dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkCard);

            PerformanceCounter dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkCard);

            float sendSum = 0;
            float receiveSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }
            float dataSent = sendSum;
            float dataReceived = receiveSum;


            double utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
            return utilization;
        }

        /// <summary>
        /// 根据网卡名称获取各个网卡当前的下载速度
        /// </summary>
        /// <param name="networkCard">网卡名称</param>
        /// <returns></returns>
        private double GetCurrentNetworkDownSpeed(string networkCard)
        {
            const int numberOfIterations = 10;

            PerformanceCounter dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkCard);

            float receiveSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                receiveSum += dataReceivedCounter.NextValue();
            }
            float dataReceived = receiveSum;

            double utilization = 8 * dataReceived / numberOfIterations;
            return utilization;
        }

        /// <summary>
        /// 根据网卡名称获取各个网卡当前的上传速度
        /// </summary>
        /// <param name="networkCard">网卡名称</param>
        /// <returns></returns>
        private double GetCurrentNetworkUpSpeed(string networkCard)
        {
            const int numberOfIterations = 10;
            PerformanceCounter dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkCard);

            float sendSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
            }
            float dataSent = sendSum;

            double utilization = 8 * dataSent / numberOfIterations;
            return utilization;
        }
    }
}
