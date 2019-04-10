using System;
using System.Collections.Generic;
using System.Text;

namespace Burgeon.Wing3.Release.Utils
{
    /// <summary>
    /// ManagementSearch 相关键 的宏列表
    /// </summary>
    public enum ManagementSearchKeys
    {
        /// <summary>
        /// 获取'CPU处理器'搜索项
        /// </summary>
        Win32_Processor,

        /// <summary>
        /// 获取'物理内存条'搜索项
        /// </summary>
        Win32_PhysicalMemory,

        /// <summary>
        /// 获取'键盘'搜索项
        /// </summary>
        Win32_Keyboard,

        /// <summary>
        /// 获取'点输入设备，包括鼠标。'搜索项
        /// </summary>
        Win32_PointingDevice,

        /// <summary>
        /// 获取'软盘驱动器'搜索项
        /// </summary>
        Win32_FloppyDrive,

        /// <summary>
        /// 获取'硬盘驱动器'搜索项
        /// </summary>
        Win32_DiskDrive,

        /// <summary>
        /// 获取'光盘驱动器'搜索项
        /// </summary>
        Win32_CDROMDrive,

        /// <summary>
        /// 获取'主板'搜索项
        /// </summary>
        Win32_BaseBoard,

        /// <summary>
        /// 获取'BIOS芯片'搜索项
        /// </summary>
        Win32_BIOS,

        /// <summary>
        /// 获取'并口'搜索项
        /// </summary>
        Win32_ParallelPort,

        /// <summary>
        /// 获取'串口'搜索项
        /// </summary>
        Win32_SerialPort,

        /// <summary>
        /// 获取'串口配置'搜索项
        /// </summary>
        Win32_SerialPortConfiguration,

        /// <summary>
        /// 获取'多媒体设置，一般指声卡。'搜索项
        /// </summary>
        Win32_SoundDevice,

        /// <summary>
        /// 获取'主板插槽(ISA&PCI&AGP)'搜索项
        /// </summary>
        Win32_SystemSlot,

        /// <summary>
        /// 获取'USB控制器'搜索项
        /// </summary>
        Win32_USBController,

        /// <summary>
        /// 获取'网络适配器'搜索项
        /// </summary>
        Win32_NetworkAdapter,

        /// <summary>
        /// 获取'网络适配器设置'搜索项
        /// </summary>
        Win32_NetworkAdapterConfiguration,

        /// <summary>
        /// 获取'打印机'搜索项
        /// </summary>
        Win32_Printer,

        /// <summary>
        /// 获取'打印机设置'搜索项
        /// </summary>
        Win32_PrinterConfiguration,

        /// <summary>
        /// 获取'打印机任务'搜索项
        /// </summary>
        Win32_PrintJob,

        /// <summary>
        /// 获取'打印机端口'搜索项
        /// </summary>
        Win32_TCPIPPrinterPort,

        /// <summary>
        /// 获取'MODEM'搜索项
        /// </summary>
        Win32_POTSModem,

        /// <summary>
        /// 获取'MODEM端口'搜索项
        /// </summary>
        Win32_POTSModemToSerialPort,

        /// <summary>
        /// 获取'显示器'搜索项
        /// </summary>
        Win32_DesktopMonitor,

        /// <summary>
        /// 获取'显卡'搜索项
        /// </summary>
        Win32_DisplayConfiguration,

        /// <summary>
        /// 获取'显卡设置'搜索项
        /// </summary>
        Win32_DisplayControllerConfiguration,

        /// <summary>
        /// 获取'显卡细节。'搜索项
        /// </summary>
        Win32_VideoController,

        /// <summary>
        /// 获取'显卡支持的显示模式。'搜索项
        /// </summary>
        Win32_VideoSettings,

        /// <summary>
        /// 获取'时区'搜索项
        /// </summary>
        Win32_TimeZone,

        /// <summary>
        /// 获取'驱动程序'搜索项
        /// </summary>
        Win32_SystemDriver,

        /// <summary>
        /// 获取'磁盘分区'搜索项
        /// </summary>
        Win32_DiskPartition,

        /// <summary>
        /// 获取'逻辑磁盘'搜索项
        /// </summary>
        Win32_LogicalDisk,

        /// <summary>
        /// 获取'逻辑磁盘所在分区及始末位置。'搜索项
        /// </summary>
        Win32_LogicalDiskToPartition,

        /// <summary>
        /// 获取'逻辑内存配置'搜索项
        /// </summary>
        Win32_LogicalMemoryConfiguration,

        /// <summary>
        /// 获取'系统页文件信息'搜索项
        /// </summary>
        Win32_PageFile,

        /// <summary>
        /// 获取'页文件设置'搜索项
        /// </summary>
        Win32_PageFileSetting,

        /// <summary>
        /// 获取'系统启动配置'搜索项
        /// </summary>
        Win32_BootConfiguration,

        /// <summary>
        /// 获取'计算机信息简要'搜索项
        /// </summary>
        Win32_ComputerSystem,

        /// <summary>
        /// 获取'操作系统信息'搜索项
        /// </summary>
        Win32_OperatingSystem,

        /// <summary>
        /// 获取'系统自动启动程序'搜索项
        /// </summary>
        Win32_StartupCommand,

        /// <summary>
        /// 获取'系统安装的服务'搜索项
        /// </summary>
        Win32_Service,

        /// <summary>
        /// 获取'系统管理组'搜索项
        /// </summary>
        Win32_Group,

        /// <summary>
        /// 获取'系统组帐号'搜索项
        /// </summary>
        Win32_GroupUser,

        /// <summary>
        /// 获取'用户帐号'搜索项
        /// </summary>
        Win32_UserAccount,

        /// <summary>
        /// 获取'系统进程'搜索项
        /// </summary>
        Win32_Process,

        /// <summary>
        /// 获取'系统线程'搜索项
        /// </summary>
        Win32_Thread,

        /// <summary>
        /// 获取'共享'搜索项
        /// </summary>
        Win32_Share,

        /// <summary>
        /// 获取'已安装的网络客户端'搜索项
        /// </summary>
        Win32_NetworkClient,

        /// <summary>
        /// 获取'已安装的网络协议'搜索项
        /// </summary>
        Win32_NetworkProtocol
    }
}
