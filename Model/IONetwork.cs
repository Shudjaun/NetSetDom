using System.Linq;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Windows.Forms;

namespace NetSetDom.Model
{
    public static class IONetwork
    {
        /// <summary>
        /// Get network adaptors "Ethernet" only on current machine
        /// </summary>
        /// <returns> NetworkInterface as array</returns>
        public static NetworkInterface[] GetInterfaces()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return new NetworkInterface[] { null };

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(
                type => type.NetworkInterfaceType.ToString().Equals("Ethernet")).ToArray<NetworkInterface>();

            /*foreach (NetworkInterface ni in interfaces)
            {
                Console.WriteLine("Interface Name: {0}", ni.Name);
                Console.WriteLine("    Description: {0}", ni.Description);
                Console.WriteLine("    ID: {0}", ni.Id);
                Console.WriteLine("    Type: {0}", ni.NetworkInterfaceType);
                Console.WriteLine("    Speed: {0}", ni.Speed);
                Console.WriteLine("    Status: {0}", ni.OperationalStatus);
            }*/
            return interfaces;
        }

        public static NetworkInterface GetInterfaceByName(string name)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var item in interfaces)
            {
                if (item.Name.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }

        public static void ResetWinsock()
        {
            // Run PowerShell script as administrator
            var newProcessInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                CreateNoWindow = true,
                Verb = "runas", // Define Run as administrator
                Arguments = "netsh int ip reset", //Define your powershell script
                UseShellExecute = false,
                RedirectStandardOutput = true, // This will enable to read Powershell run output. If not set to true you will show output in Console
                RedirectStandardError = true
            };
            Process proces = Process.Start(newProcessInfo);
            proces.WaitForExit();
            MessageBox.Show("L'ordinateur va redémarrer pour terminer le processus.\nVeuillez sauvegarder votre travail avant de cliquer sur OK");
            // Restart computer
            Process.Start("shutdown.exe", "-r -t 0");
        }
        public static void SetStaticIP(string nic, string ipAddress, string subnet, string gateway)
        {
            // Run PowerShell script as administrator. Requires app to Run As admin too
            var newProcessInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                CreateNoWindow = true, // WIll execute powershell script without opening any Window
                Verb = "runas", // Define Run as administrator
                Arguments = $"netsh interface ipv4 set address name='{nic}' static {ipAddress} {subnet} {gateway}", //Powershell script
                UseShellExecute = false,
                RedirectStandardOutput = true, // This will enable to read Powershell run output. If not set to true you will show output in Console
                RedirectStandardError = true
            };

            Process proces = Process.Start(newProcessInfo);
            proces.WaitForExit();
            // Output via powershell window for debug purpose
            /*StringBuilder output = new StringBuilder();
            output.Append("Started");
            while (!proces.StandardOutput.EndOfStream)
            {
                output.Append(proces.StandardOutput.ReadLine());
            }*/

            /*string matchingString = toDoaminValue + "\\" + toAccount + "  True"; // My codition
            bool isMatched = InspectPowerShellOutput(output.ToString(), matchingString);
            if (!isMatched)
            {
                // Do something if output is OK
            }*/
        }

        public static void SetDynamicIP(string nic)
        {
            // Run PowerShell script as administrator. Requires app to Run As admin too
            var newProcessInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                CreateNoWindow = true, // WIll execute powershell script without opening any Window
                Verb = "runas", // Define Run as administrator
                Arguments = $"netsh interface ip set address name='{nic}' dhcp", //Powershell script
                UseShellExecute = false,
                RedirectStandardOutput = true, // This will enable to read Powershell run output. If not set to true you will show output in Console
                RedirectStandardError = true
            };
            
            Process proces = Process.Start(newProcessInfo);
            proces.WaitForExit();
        }

        public static void SetStaticDNS(string nic, string ipAddress)
        {
            // Run PowerShell script as administrator. Requires app to Run As admin too
            var newProcessInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                CreateNoWindow = true, // WIll execute powershell script without opening any Window
                Verb = "runas", // Define Run as administrator
                Arguments = $"netsh interface ipv4 set dnsservers name='{nic}' static {ipAddress} primary", //Powershell script
                UseShellExecute = false,
                RedirectStandardOutput = true, // This will enable to read Powershell run output. If not set to true you will show output in Console
                RedirectStandardError = true
            };

            Process proces = Process.Start(newProcessInfo);
            proces.WaitForExit();
        }

        public static void SetDynamicDNS(string nic)
        {
            // Run PowerShell script as administrator. Requires app to Run As admin too
            var newProcessInfo = new ProcessStartInfo
            {
                FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                CreateNoWindow = true, // WIll execute powershell script without opening any Window
                Verb = "runas", // Define Run as administrator
                Arguments = $"netsh interface ipv4 set dnsservers name='{nic}' source=dhcp", //Powershell script
                UseShellExecute = false,
                RedirectStandardOutput = true, // This will enable to read Powershell run output. If not set to true you will show output in Console
                RedirectStandardError = true
            };

            Process proces = Process.Start(newProcessInfo);
            proces.WaitForExit();
        }
    }
}
