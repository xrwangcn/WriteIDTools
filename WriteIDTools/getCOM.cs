using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace WriteIDTools
{
    public class getCOM
    {
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
        public static string GetSerialPort()
        {
            //USB\VID_03EB&PID_2425&MI_00\6&39C4CAA2&0&0000###COM4  pav串口
            //USB\VID_03EB&PID_2425&MI_02\6&39C4CAA2&0&0002###COM6  控制串口
            string result = MulGetHardwareInfo(HardwareEnum.Win32_SerialPort, "PNPDeviceID", "DeviceID");
            //result = result.Replace("USB\\VID_03EB&PID_2425&MI_0", "");
            //result = result.Replace("\\6&39C4CAA2&0&000", "");
            //0\\6&270B19FD&0&0000
            //0\\7&27517C37&0&0000

            string[] info = result.Split(new char[6] { '*', '*', '*', '*', '*', '*' }, StringSplitOptions.RemoveEmptyEntries);
            //string[] info = new string[6];
            //info[0] = "a123******";
            //info[1] = "b123******";
            //info[2] = "USB\\VID_03EB&PID_2425&MI_00******";
            //info[3] = "c123******";
            //info[4] = "USB\\VID_03EB&PID_2425&MI_00******";
            //info[5] = "d123******";
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].IndexOf("USB\\VID_03EB&PID_2425&MI_02") == 0)
                {
                    info[i] = info[i].Replace("USB\\VID_03EB&PID_2425&MI_0", "");
                    i++;
                }
                else
                {

                    info[i++] = "";
                    info[i] = "";
                }
            }
            List<string> temp = info.ToList();
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (temp[i] == "")
                {
                    temp.RemoveAt(i);
                }
            }
            info = temp.ToArray();
            if (info.Count() == 0)
                return null;
            return info[1];
        }
        public static string MulGetHardwareInfo(HardwareEnum hardType, string propKey, string propKey2)
        {
            string strs = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType);
            var hardInfos = searcher.Get();
            foreach (var hardInfo in hardInfos)
            {
                if (hardInfo.Properties[propKey] != null)
                {
                    strs = strs + hardInfo.Properties[propKey].Value.ToString() + "******" + hardInfo.Properties[propKey2].Value.ToString() + "******";
                }

            }
            return strs;
        }
    }
}
