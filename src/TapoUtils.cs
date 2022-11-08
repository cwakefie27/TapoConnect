namespace TapoConnect
{
    public static class TapoUtils
    {
        public const string TapoPlugDeviceType = "SMART.TAPOPLUG";
        public const string TapoBulbDeviceType = "SMART.TAPOBULB";
        public const string TapoIpCameraDeviceType = "SMART.IPCAMERA";

        public static bool IsTapoDevice(string deviceType)
        {
            if (deviceType == null)
            {
                throw new ArgumentNullException(nameof(deviceType));
            }

#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (deviceType.ToUpper())
            {
                case TapoPlugDeviceType:
                case TapoBulbDeviceType:
                case TapoIpCameraDeviceType:
                    return true;
                default:
                    return false;
            }
#pragma warning restore IDE0066 // Convert switch statement to expression
        }

        private static string FormatMacAddress(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length == 12)
            {
                return text.Insert(10, "-")
                    .Insert(8 , "-")
                    .Insert(6 , "-")
                    .Insert(4 , "-")
                    .Insert(2 , "-")
                    .ToLower();
            }
            else
            {
                return text.ToLower();
            }
        }

        public static bool? TryGetIpAddressByMacAddress(string macAddress, out string? ipAddress)
        {
            var result = GetIpAddressByMacAddress(macAddress);

            if (result != null)
            {
                ipAddress = result;
                return true;
            }
            else
            {
                ipAddress = null;
                return false;
            }
        }

        public static string? GetIpAddressByMacAddress(string macAddress)
        {
            if (macAddress == null)
            {
                throw new ArgumentNullException(nameof(macAddress));
            }

            System.Diagnostics.Process pProcess = new();

            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();

            string strOutput = pProcess.StandardOutput.ReadToEnd();

            var tidyMac = FormatMacAddress(macAddress);

            var lineWithCriteria = strOutput
                .Split('\n')
                .Where(x => x.Contains(tidyMac))
                .FirstOrDefault();

            if (lineWithCriteria != null)
            {
                var lineParts = lineWithCriteria
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                return lineParts[0];
            }
            else
            {
                return null;
            }
        }
    }
}
