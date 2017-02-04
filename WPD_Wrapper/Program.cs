using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPD_Wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Device> devices = Device.GetPortableDevices();

            Console.WriteLine($"Device Count: {devices.Count}");
            foreach(Device device in devices)
            {
                Console.WriteLine($"Serial Number: {device.SerialNumber}");
            }
        }
    }
}
