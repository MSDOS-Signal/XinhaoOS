using System;
using System.Runtime.InteropServices;
using System.Management;

namespace ChromeOS.Controls
{
    public static class SystemControls
    {
        // 音量控制相关
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAudioEndpointVolume
        {
            int NotImpl1();
            int NotImpl2();
            int NotImpl3();
            int NotImpl4();
            int NotImpl5();
            int NotImpl6();
            int NotImpl7();
            int NotImpl8();
            int SetMasterVolumeLevelScalar(float fLevel, IntPtr pguidEventContext);
            int GetMasterVolumeLevelScalar(out float pfLevel);
            int SetMute(bool bMute, IntPtr pguidEventContext);
            int GetMute(out bool pbMute);
        }

        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDevice
        {
            int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        }

        [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDeviceEnumerator
        {
            int EnumAudioEndpoints(int dataFlow, int dwStateMask, out IntPtr ppDevices);
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice ppEndpoint);
        }

        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumerator { }

        private const int eRender = 0;
        private const int eMultimedia = 1;

        public static bool SetMasterVolume(float volume)
        {
            try
            {
                var deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
                deviceEnumerator.GetDefaultAudioEndpoint(eRender, eMultimedia, out var device);
                
                var iid = typeof(IAudioEndpointVolume).GUID;
                device.Activate(ref iid, 0, IntPtr.Zero, out var obj);
                var audioEndpoint = (IAudioEndpointVolume)obj;
                
                volume = Math.Max(0, Math.Min(1, volume));
                audioEndpoint.SetMasterVolumeLevelScalar(volume, IntPtr.Zero);
                
                Marshal.ReleaseComObject(audioEndpoint);
                Marshal.ReleaseComObject(device);
                Marshal.ReleaseComObject(deviceEnumerator);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static float GetMasterVolume()
        {
            try
            {
                var deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
                deviceEnumerator.GetDefaultAudioEndpoint(eRender, eMultimedia, out var device);
                
                var iid = typeof(IAudioEndpointVolume).GUID;
                device.Activate(ref iid, 0, IntPtr.Zero, out var obj);
                var audioEndpoint = (IAudioEndpointVolume)obj;
                
                audioEndpoint.GetMasterVolumeLevelScalar(out float volume);
                
                Marshal.ReleaseComObject(audioEndpoint);
                Marshal.ReleaseComObject(device);
                Marshal.ReleaseComObject(deviceEnumerator);
                
                return volume;
            }
            catch
            {
                return 0.7f;
            }
        }

        // 屏幕亮度控制
        public static bool SetBrightness(int brightness)
        {
            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorBrightnessMethods"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        ManagementObject managementObj = (ManagementObject)obj;
                        managementObj.InvokeMethod("WmiSetBrightness", new object[] { 1, brightness });
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int GetBrightness()
        {
            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorBrightness"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        return Convert.ToInt32(obj["CurrentBrightness"]);
                    }
                }
                return 80;
            }
            catch
            {
                return 80;
            }
        }
    }
}
