using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetSetDom.Model
{
    public static class IONetwork
    {
        /// <summary>
        /// Get network adaptors on current machine
        /// </summary>
        /// <returns> NetworkInterface as array</returns>
        public static NetworkInterface[] GetInterfaces()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return new NetworkInterface[] { null };

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

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
    }
}
