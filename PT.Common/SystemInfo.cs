using System;
using System.Linq;
using System.Management;
using System.IO;

namespace PT.Common
{
    /// <summary>
    /// 操作系统信息
    /// </summary>
    public class SystemInfo
    {
        static ManagementClass mc = new ManagementClass("Win32_Processor");
        static ManagementClass osMC = new ManagementClass("Win32_OperatingSystem");
        static ManagementClass diskMC = new ManagementClass("Win32_PhysicalMedia");

        #region CPU

        /// <summary>
        /// CPU Id
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            string id = Convert.ToString(GetValue(mc, "ProcessorId"));
            return id;
        }
        /// <summary>
        /// CPU Name
        /// </summary>
        /// <returns></returns>
        public static string GetCpuName()
        {
            string name = Convert.ToString(GetValue(mc, "Name"));
            return name;
        }
        /// <summary>
        /// CPU 内核数量
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfCpuCores()
        {
            int n = Convert.ToInt32(GetValue(mc, "NumberOfCores"));
            return n;
        }

        /// <summary>
        /// CPU 逻辑处理器数量
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfCpuLogicalProcessors()
        {
            int n = Convert.ToInt32(GetValue(mc, "NumberOfLogicalProcessors"));
            return n;
        }

        /// <summary>
        /// CPU 负载率
        /// </summary>
        /// <returns></returns>
        public static decimal GetCpuLoadPercentage()
        {
            decimal n = Convert.ToDecimal(GetValue(mc, "LoadPercentage"));
            return n;
        }
        /// <summary>
        /// CPU 数据宽度（CPU位数）
        /// </summary>
        /// <returns></returns>
        public static int GetCpuDataWidth()
        {
            int n = Convert.ToInt32(GetValue(mc, "DataWidth"));
            return n;
        }
        /// <summary>
        /// CPU 时钟频率
        /// 单位（MHz）
        /// </summary>
        /// <returns></returns>
        public static decimal GetCpuClockSpeed()
        {
            decimal n = Convert.ToDecimal(GetValue(mc, "CurrentClockSpeed"));
            return n;
        }

        #endregion

        #region 内存
        /// <summary>
        /// 可用内存
        /// 单位（MB）
        /// </summary>
        /// <returns></returns>
        public static decimal GetAvailableMemory()
        {
            decimal n = Convert.ToDecimal(GetValue(osMC, "FreePhysicalMemory")) / 1024;
            return n;
        }
        /// <summary>
        /// 物理内存总容量
        /// 单位（MB）
        /// </summary>
        /// <returns></returns>
        public static decimal GetTotalPhyMemory()
        {
            decimal n = Convert.ToDecimal(GetValue(osMC, "TotalVisibleMemorySize")) / 1024;
            return n;
        }

        #endregion

        #region 硬盘
        /// <summary>
        /// 取第一块硬盘编号 
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskID()
        {
            return Convert.ToString(GetValue(diskMC, "SerialNumber")).Trim();
        }
        /// <summary>
        /// 驱动器可用容量
        /// 单位（GB）
        /// </summary>
        /// <param name="driverName">驱动器名称 例如：C 或 D</param>
        /// <returns></returns>
        public static decimal GetDiskFreeSpace(string driverName)
        {
            decimal n = 0;
            var lstDriver = DriveInfo.GetDrives().ToList();
            var dr = lstDriver.FirstOrDefault(e => e.Name == string.Format("{0}:\\", driverName));
            if (dr != null)
            {
                n = Math.Round(Convert.ToDecimal(dr.TotalFreeSpace) / (1024 * 1024 * 1024), 1);
            }
            return n;
        }
        /// <summary>
        /// 驱动器总容量
        /// 单位（GB）
        /// </summary>
        /// <param name="driverName">驱动器名称 例如：C 或 D</param>
        /// <returns></returns>
        public static decimal GetDiskTotalSpace(string driverName)
        {
            decimal n = 0;
            var lstDriver = DriveInfo.GetDrives().ToList();
            var dr = lstDriver.FirstOrDefault(e => e.Name == string.Format("{0}:\\", driverName));
            if (dr != null)
            {
                n = Math.Round(Convert.ToDecimal(dr.TotalSize) / (1024 * 1024 * 1024), 1);
            }
            return n;
        }
        #endregion

        #region 操作系统
        /// <summary>
        /// 是否64位操作系统
        /// </summary>
        /// <returns></returns>
        public static bool Is64BitOperatingSystem()
        {
            return Environment.Is64BitOperatingSystem;
        }
        /// <summary>
        /// 操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetOperatingSystemName()
        {
            return Convert.ToString(GetValue(osMC, "Name")).Split('|')[0];
        }

        /// <summary>
        /// 操作系统服务包版本 例如(SP1)
        /// </summary>
        /// <returns></returns>
        public static string GetOperatingSystemCSDVersion()
        {
            return Convert.ToString(GetValue(osMC, "CSDVersion"));
        }
        #endregion

        private static object GetValue(ManagementClass mc, string name)
        {
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                return mo[name];
            }

            return null;
        }
    }
}
