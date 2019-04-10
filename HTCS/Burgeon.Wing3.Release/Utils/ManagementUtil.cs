using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// 系统管理工具扩展工具
    /// </summary>
    public class ManagementUtil
    {

        /// <summary>
        /// 获取本地磁盘编号
        /// </summary>
        /// <param name="disk">盘符 默认C:</param>
        /// <returns></returns>
        public static string GetDiskIdentifier(string disk = "C:")
        {
            string diskIdentifier = null;
            System.Management.ManagementObjectCollection queryCollection = Search(ManagementSearchKeys.Win32_LogicalDisk);
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["DeviceID"] == null) { continue; }
                if (string.Equals(disk, mo["DeviceID"].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    diskIdentifier = mo["VolumeSerialNumber"].ToString();
                    break;
                }
            }
            if (string.IsNullOrWhiteSpace(diskIdentifier))
            {
                diskIdentifier = "UNKNOW";
            }

            return diskIdentifier;
        }

        /// <summary>
        /// 获取本次CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCpuIdentifier()
        {
            string cpuIdentifier = null;
            System.Management.ManagementObjectCollection queryCollection = Search(ManagementSearchKeys.Win32_Processor);
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["ProcessorId"] != null)
                {
                    cpuIdentifier = mo["ProcessorId"].ToString();
                    break;
                }
            }
            if (string.IsNullOrWhiteSpace(cpuIdentifier))
            {
                cpuIdentifier = "UNKNOW";
            }

            return cpuIdentifier;
        }

        /// <summary>
        /// 基于指定的查询检索管理对象的集合 返回搜索结果
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static System.Management.ManagementObjectCollection Search(ManagementSearchKeys sKey, string where = "")
        {
            string sql = string.Format("SELECT * FROM {0}", Enum.GetName(typeof(ManagementSearchKeys), sKey));

            if (!string.IsNullOrEmpty(where))
            {
                sql = string.Format("{0} WHERE {1}", sql, where);
            }

            System.Management.ManagementObjectSearcher querySearch = new System.Management.ManagementObjectSearcher(sql);
            System.Management.ManagementObjectCollection queryCollection = querySearch.Get();

            return queryCollection;
        }

        /// <summary>
        /// 获取当前所有进程的网络地址列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetProcessNewtworkAddress()
        {
            List<string> addressList = new List<string>();

            System.Management.ManagementObjectCollection queryCollection = Search(ManagementSearchKeys.Win32_NetworkAdapterConfiguration);

            string[] tempAddress = null;

            foreach (ManagementObject obj in queryCollection)
            {
                tempAddress = obj["IPAddress"] as string[];
                if (tempAddress != null)
                {
                    addressList.AddRange(tempAddress);
                }
            }

            return addressList;
        }
    }
}
