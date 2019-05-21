using System;
using System.Collections.Generic;
using System.Text;

using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace IECS.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class Win32Helper
    {
        #region Win32

        /// <summary></summary>
        public const string Win32_REG_NET_CARDS_KEY = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkCards";
        /// <summary></summary>
        public const uint Win32_GENERIC_READ = 0x80000000;
        /// <summary></summary>
        public const uint Win32_GENERIC_WRITE = 0x40000000;
        /// <summary></summary>
        public const uint Win32_FILE_SHARE_READ = 0x00000001;
        /// <summary></summary>
        public const uint Win32_FILE_SHARE_WRITE = 0x00000002;
        /// <summary></summary>
        public const uint Win32_OPEN_EXISTING = 3;
        /// <summary></summary>
        public const uint Win32_INVALID_HANDLE_VALUE = 0xffffffff;
        /// <summary></summary>
        public const uint Win32_IOCTL_NDIS_QUERY_GLOBAL_STATS = 0x00170002;
        /// <summary></summary>
        public const int Win32_PERMANENT_ADDRESS = 0x01010101;

        /// <summary></summary>
        [DllImport("kernel32.dll")]
        public static extern int Win32_CloseHandle(uint hObject);

        /// <summary></summary>
        [DllImport("kernel32.dll")]
        public static extern int Win32_DeviceIoControl(uint hDevice,
                                                  uint dwIoControlCode,
                                                  ref int lpInBuffer,
                                                  int nInBufferSize,
                                                  byte[] lpOutBuffer,
                                                  int nOutBufferSize,
                                                  ref uint lpbytesReturned,
                                                  int lpOverlapped);

        /// <summary></summary>
        [DllImport("kernel32.dll")]
        public static extern uint Win32_CreateFile(string lpFileName,
                                              uint dwDesiredAccess,
                                              uint dwShareMode,
                                              int lpSecurityAttributes,
                                              uint dwCreationDisposition,
                                              uint dwFlagsAndAttributes,
                                              int hTemplateFile);

        #endregion

        #region 获取硬件的序号

        /// <summary>
        /// cpu序列号
        /// </summary>
        /// <returns></returns>
        public static String GetCPUID()
        {
            string cpuInfo = string.Empty;//cpu序列号   
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = (string)mo.Properties["ProcessorId"].Value;
                if (cpuInfo != null)
                    break;
                mo.Dispose();
            }
            if (cpuInfo == null) return "";

            return cpuInfo;
        }

        /// <summary>
        /// 获取网卡硬件地址
        /// </summary>
        /// <returns></returns>
        public static String GetMACID()
        {
            string tmp = "";

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();
            foreach (ManagementObject mo in moc2)
            {
                if ((bool)mo["IPEnabled"] == true && (bool)mo["DHCPEnabled"] == true)
                {
                    tmp = mo["MacAddress"].ToString();
                    //break;
                }
                mo.Dispose();

            }
            return tmp;
        }

        /// <summary>
        /// 获取硬盘ID
        /// </summary>
        /// <returns></returns>
        public static String GetDriverID()
        {
            String HDid;
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                HDid = (string)mo.Properties["Model"].Value;
                return HDid.ToString();
            }
            return "";
        }

        #endregion

        #region NetCard

        //调用方法
        //string macAddr = Win32Helper.GetNetCardLocalMac();
        //Console.WriteLine(macAddr);
        //Console.ReadLine();

        /// <summary>
        /// 给出网卡地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardLocalMac()
        {
            List<string> netCardList = GetNetCardList();
            List<string>.Enumerator enumNetCard = netCardList.GetEnumerator();

            string macAddr = string.Empty;
            while (enumNetCard.MoveNext())
            {
                macAddr = GetNetCardPhysicalAddr(enumNetCard.Current);
                if (macAddr != string.Empty)
                {
                    break;
                }
            }
            return macAddr;
        }

        /// <summary>
        /// 得到网卡列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetNetCardList()
        {
            List<string> cardList = new List<string>();
            try
            {
                RegistryKey regNetCards = Registry.LocalMachine.OpenSubKey(Win32_REG_NET_CARDS_KEY);
                if (regNetCards != null)
                {
                    string[] names = regNetCards.GetSubKeyNames();
                    RegistryKey subKey = null;
                    foreach (string name in names)
                    {
                        subKey = regNetCards.OpenSubKey(name);
                        if (subKey != null)
                        {
                            object o = subKey.GetValue("ServiceName");
                            if (o != null)
                            {
                                cardList.Add(o.ToString());
                            }
                        }
                    }
                }
            }
            catch { }

            return cardList;
        }

        /// <summary>
        /// 由网卡号得出网卡 MAC 地址
        /// </summary>
        /// <param name="sCardId"></param>
        /// <returns></returns>
        private static string GetNetCardPhysicalAddr(string sCardId)
        {
            string macAddress = string.Empty;
            uint device = 0;
            try
            {
                string driveName = "\\\\.\\" + sCardId;
                device = Win32_CreateFile(driveName,
                                         Win32_GENERIC_READ | Win32_GENERIC_WRITE,
                                         Win32_FILE_SHARE_READ | Win32_FILE_SHARE_WRITE,
                                         0, Win32_OPEN_EXISTING, 0, 0);
                if (device != Win32_INVALID_HANDLE_VALUE)
                {
                    byte[] outBuff = new byte[6];
                    uint bytRv = 0;
                    int intBuff = Win32_PERMANENT_ADDRESS;

                    if (0 != Win32_DeviceIoControl(device, Win32_IOCTL_NDIS_QUERY_GLOBAL_STATS,
                                        ref intBuff, 4, outBuff, 6, ref bytRv, 0))
                    {
                        string temp = string.Empty;
                        foreach (byte b in outBuff)
                        {
                            temp = Convert.ToString(b, 16).PadLeft(2, '0');
                            macAddress += temp;
                            temp = string.Empty;
                        }
                    }
                }
            }
            finally
            {
                if (device != 0)
                {
                    Win32_CloseHandle(device);
                }
            }

            return macAddress;
        }

        #endregion
    }


}
