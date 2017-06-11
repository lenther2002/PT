using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace PT.Common
{
    public class RegistryHelper
    {
        public static List<string> GetRegistrySubKeyNames(string regPath)
        {
            string regType = regPath.Substring(0, regPath.IndexOf('\\'));
            string keyPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            var ret = new List<string>();

            RegistryKey subKey;
            try
            {
                switch (regType)
                {
                    case "HKEY_LOCAL_MACHINE":
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CLASS_ROOT":
                        subKey = Registry.ClassesRoot.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_USER":
                        subKey = Registry.CurrentUser.OpenSubKey(keyPath);
                        break;
                    case "HKEY_USERS":
                        subKey = Registry.Users.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        subKey = Registry.CurrentConfig.OpenSubKey(keyPath);
                        break;
                    default:
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath);
                        break;
                }

                var names = subKey.GetSubKeyNames();
                if (names != null && names.Length > 0)
                {
                    ret.AddRange(names);
                }
                return ret;
            }
            catch
            {
                return ret;
            }
        }

        #region 判断某个键值是否存在
        public static bool IsRegKeyExists(string regPath, string regKey)
        {
            string regType;
            string keyPath;

            regType = regPath.Substring(0, regPath.IndexOf('\\'));
            keyPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            RegistryKey subKey;
            try
            {
                switch (regType)
                {
                    case "HKEY_LOCAL_MACHINE":
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CLASS_ROOT":
                        subKey = Registry.ClassesRoot.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_USER":
                        subKey = Registry.CurrentUser.OpenSubKey(keyPath);
                        break;
                    case "HKEY_USERS":
                        subKey = Registry.Users.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        subKey = Registry.CurrentConfig.OpenSubKey(keyPath);
                        break;
                    default:
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath);
                        break;
                }
                if (subKey == null) return false;

                if (regKey != null)
                {
                    if (subKey.GetValue(regKey) == null) return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true; ;
        }
        /// <summary>
        /// 读取注册表操作.
        /// </summary>
        /// <param name="regPath">注册表路径,示例"HKEY_LOCAL_MACHINE\\Software\\windows".</param>
        /// <param name="regKey">键值.</param>
        /// <param name="def">默认值,遇到错误时将返回该字符串.</param>
        /// <returns>返回所取键值.</returns>
        public static string GetRegistryKey(string regPath, string regKey, string def)
        {
            string regType;
            string keyPath;
            string strRtn;
            regType = regPath.Substring(0, regPath.IndexOf('\\'));
            keyPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            RegistryKey subKey;
            try
            {
                switch (regType)
                {
                    case "HKEY_LOCAL_MACHINE":
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CLASS_ROOT":
                        subKey = Registry.ClassesRoot.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_USER":
                        subKey = Registry.CurrentUser.OpenSubKey(keyPath);
                        break;
                    case "HKEY_USERS":
                        subKey = Registry.Users.OpenSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        subKey = Registry.CurrentConfig.OpenSubKey(keyPath);
                        break;
                    default:
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath);
                        break;
                }
                strRtn = subKey.GetValue(regKey).ToString();
            }
            catch
            {
                return def;
            }

            return strRtn;
        }
        #endregion 取某键值

        #region 修改或创建某个键值
        /// <summary>
        /// 设置指定注册表路径下的键值,如果键值不存在将创建它.运行用户必须有改注册表权限.
        /// </summary>
        /// <param name="regPath">注册表路径,示例"HKEY_LOCAL_MACHINE\\Software\\windows"</param>
        /// <param name="regKey">键名</param>
        /// <param name="val">值</param>
        /// <returns>1设置或创建成功, 0创建失败,可能路径不存在等原因.</returns>
        public static long SetRegistryKey(string regPath, string regKey, string val)
        {
            string regType;
            string keyPath;
            regType = regPath.Substring(0, regPath.IndexOf('\\'));
            keyPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            RegistryKey subKey;
            try
            {
                switch (regType)
                {
                    case "HKEY_LOCAL_MACHINE":
                        subKey = Registry.LocalMachine.CreateSubKey(keyPath);
                        break;
                    case "HKEY_CLASS_ROOT":
                        subKey = Registry.ClassesRoot.CreateSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_USER":
                        subKey = Registry.CurrentUser.CreateSubKey(keyPath);
                        break;
                    case "HKEY_USERS":
                        subKey = Registry.Users.CreateSubKey(keyPath);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        subKey = Registry.CurrentConfig.CreateSubKey(keyPath);
                        break;
                    default:
                        subKey = Registry.LocalMachine.CreateSubKey(keyPath);
                        break;
                }
                subKey.SetValue(regKey, val);
            }
            catch
            {
                return 0;
            }
            return 1;
        }
        #endregion 修改或创建某个键值

        #region 删除某个键值
        /// <summary>
        /// 删除指定路径下的某个键值.运行用户必须有改注册表权限.
        /// </summary>
        /// <param name="regPath">注册表路径,示例"HKEY_LOCAL_MACHINE\\Software\\windows".</param>
        /// <param name="regKey">要删除的键值.</param>
        /// <returns>1删除成功,0删除失败.</returns>
        public static long DelRegistryKey(string regPath, string regKey)
        {
            string regType;
            string keyPath;
            regType = regPath.Substring(0, regPath.IndexOf('\\'));
            keyPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            RegistryKey subKey;
            try
            {
                switch (regType)
                {
                    case "HKEY_LOCAL_MACHINE":
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_CLASS_ROOT":
                        subKey = Registry.ClassesRoot.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_CURRENT_USER":
                        subKey = Registry.CurrentUser.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_USERS":
                        subKey = Registry.Users.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        subKey = Registry.CurrentConfig.OpenSubKey(keyPath, true);
                        break;
                    default:
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
                        break;
                }
                subKey.DeleteValue(regKey);
            }
            catch
            {
                return 0;
            }
            return 1;
        }
        #endregion 删除某个键值

        #region 删除某个键
        /// <summary>
        /// 删除注册表指定路径下的子目录.运行用户必须有改注册表权限.
        /// </summary>
        /// <param name="regPath">注册表路径,示例"HKEY_LOCAL_MACHINE\\Software\\windows".</param>
        /// <param name="subDir">子目录.</param>
        /// <returns>1成功,0失败</returns>
        public static long DelRegistryTree(string regPath, string subDir)
        {
            string regType;
            string keyPath;
            regType = regPath.Substring(0, regPath.IndexOf('\\'));
            keyPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            RegistryKey subKey;
            try
            {
                switch (regType)
                {
                    case "HKEY_LOCAL_MACHINE":
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_CLASS_ROOT":
                        subKey = Registry.ClassesRoot.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_CURRENT_USER":
                        subKey = Registry.CurrentUser.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_USERS":
                        subKey = Registry.Users.OpenSubKey(keyPath, true);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        subKey = Registry.CurrentConfig.OpenSubKey(keyPath, true);
                        break;
                    default:
                        subKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
                        break;
                }
                subKey.DeleteSubKeyTree(subDir);
            }
            catch (Exception e)
            {
                throw e;
            }
            return 1;
        }
        #endregion 删除某个键

        #region 32位程序读写64注册表

        static UIntPtr HKEY_CLASSES_ROOT = (UIntPtr)0x80000000;
        static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
        static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
        static UIntPtr HKEY_USERS = (UIntPtr)0x80000003;
        static UIntPtr HKEY_CURRENT_CONFIG = (UIntPtr)0x80000005;

        // 关闭64位（文件系统）的操作转向
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        // 开启64位（文件系统）的操作转向
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        // 获取操作Key值句柄
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions,
                                 int samDesired, out IntPtr phkResult);
        //关闭注册表转向（禁用特定项的注册表反射）
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long RegDisableReflectionKey(IntPtr hKey);
        //使能注册表转向（开启特定项的注册表反射）
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long RegEnableReflectionKey(IntPtr hKey);
        //获取Key值（即：Key值句柄所标志的Key对象的值）
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved,
                                                  out uint lpType, System.Text.StringBuilder lpData,
                                                  ref uint lpcbData);

        private static UIntPtr TransferKeyName(string keyName)
        {
            switch (keyName)
            {
                case "HKEY_CLASSES_ROOT":
                    return HKEY_CLASSES_ROOT;
                case "HKEY_CURRENT_USER":
                    return HKEY_CURRENT_USER;
                case "HKEY_LOCAL_MACHINE":
                    return HKEY_LOCAL_MACHINE;
                case "HKEY_USERS":
                    return HKEY_USERS;
                case "HKEY_CURRENT_CONFIG":
                    return HKEY_CURRENT_CONFIG;
            }

            return HKEY_CLASSES_ROOT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentKeyName">myParentKeyName = "HKEY_LOCAL_MACHINE"</param>
        /// <param name="subKeyName">mySubKeyName = @"SOFTWARE\EricSun\MyTestKey"</param>
        /// <param name="keyName">myKeyName = "MyKeyName"</param>
        /// <returns></returns>
        public static string Get64BitRegistryKey(string parentKeyName, string subKeyName, string keyName)
        {
            int KEY_QUERY_VALUE = (0x0001);
            int KEY_WOW64_64KEY = (0x0100);
            int KEY_ALL_WOW64 = (KEY_QUERY_VALUE | KEY_WOW64_64KEY);

            try
            {
                //将Windows注册表主键名转化成为不带正负号的整形句柄（与平台是32或者64位有关）
                UIntPtr hKey = TransferKeyName(parentKeyName);

                //声明将要获取Key值的句柄
                IntPtr pHKey = IntPtr.Zero;

                //记录读取到的Key值
                StringBuilder result = new StringBuilder("".PadLeft(1024));
                uint resultSize = 1024;
                uint lpType = 0;

                //关闭文件系统转向 
                IntPtr oldWOW64State = new IntPtr();
                if (Wow64DisableWow64FsRedirection(ref oldWOW64State))
                {
                    //获得操作Key值的句柄
                    RegOpenKeyEx(hKey, subKeyName, 0, KEY_ALL_WOW64, out pHKey);

                    //关闭注册表转向（禁止特定项的注册表反射）
                    RegDisableReflectionKey(pHKey);

                    //获取访问的Key值
                    RegQueryValueEx(pHKey, keyName, 0, out lpType, result, ref resultSize);

                    //打开注册表转向（开启特定项的注册表反射）
                    RegEnableReflectionKey(pHKey);
                }

                //打开文件系统转向
                Wow64RevertWow64FsRedirection(oldWOW64State);

                //返回Key值
                return result.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
