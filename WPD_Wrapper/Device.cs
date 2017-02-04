using System;
using System.Collections.Generic;
using PortableDeviceApiLib;
using PortableDeviceTypesLib;
using _tagpropertykey = PortableDeviceApiLib._tagpropertykey;
using IPortableDeviceValues = PortableDeviceApiLib.IPortableDeviceValues;

namespace WPD_Wrapper
{
    public class Device
    {
        public string DeviceId { get; protected set; }
        public string SerialNumber { get; protected set; }

        public bool IsConnected { get; protected set; }

        protected PortableDeviceClass _DeviceObject;

        public static List<Device> GetPortableDevices()
        {
            PortableDeviceManager manager = new PortableDeviceManager();
            List<Device> portableDevices = new List<Device>();

            manager.RefreshDeviceList();
            uint count = 1;
            manager.GetDevices(null, ref count);

            if (count > 0)
            {
                string[] deviceIds = new string[count];
                manager.GetDevices(ref deviceIds[0], ref count);

                foreach (string deviceId in deviceIds)
                {
                    portableDevices.Add(new Device(deviceId));
                }
            }

            return portableDevices;
        }

        protected Device(string deviceId)
        {
            this.DeviceId = deviceId;
            this._DeviceObject = new PortableDeviceClass();
            this.LoadDeviceProperty();
        }

        protected void Connect()
        {
            if (IsConnected)
            {
                return;
            }

            this._DeviceObject.Open(DeviceId, (IPortableDeviceValues)new PortableDeviceValuesClass());
            this.IsConnected = true;
        }

        protected void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            this._DeviceObject.Close();
            this.IsConnected = false;
        }

        protected void LoadDeviceProperty()
        {
            IPortableDeviceContent content;
            IPortableDeviceProperties properties;
            IPortableDeviceValues propertyValues;

            this.Connect();
            this._DeviceObject.Content(out content);
            content.Properties(out properties);
            properties.GetValues("DEVICE", null, out propertyValues);
            this.SerialNumber = LoadSerialNumber(propertyValues);
            this.Disconnect();
        }

        protected string LoadSerialNumber(IPortableDeviceValues deviceProperties)
        {
            string propertyValue;
            _tagpropertykey property = new _tagpropertykey()
            {
                fmtid = new Guid("26D4979A-E643-4626-9E2B-736DC0C92FDC"),
                pid = 9,
            };

            deviceProperties.GetStringValue(ref property, out propertyValue);

            return propertyValue;
        }
    }
}
