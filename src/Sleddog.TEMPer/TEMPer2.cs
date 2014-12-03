using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using HidLibrary;

namespace Sleddog.TEMPer
{
    public class TEMPer2 : IDisposable
    {
        private static readonly int VendorId = 0x0C45;
        private static readonly int ProductId = 0x7401;

        private static readonly byte[] ReadTemperateureCommand = {0x00, 0x01, 0x80, 0x33, 0x01, 0x00, 0x00, 0x00, 0x00};
        private readonly IHidDevice device;

        public ISubject<TemperatureReading> InternalSensor { get; private set; }
        public ISubject<TemperatureReading> ExternalSensor { get; private set; }

        public TEMPer2()
        {
            var hidDevices = new HidEnumerator().Enumerate(VendorId, ProductId);

            device = hidDevices.Single(hd => hd.Capabilities.UsagePage == -256);

            InternalSensor = new Subject<TemperatureReading>();
            ExternalSensor = new Subject<TemperatureReading>();
        }

        public void ReadTemperatures()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    device.Write(ReadTemperateureCommand);

                    var data = device.Read();

                    if (data.Status == HidDeviceData.ReadStatus.Success)
                    {
                        var internalTemperature = new[] {data.Data[3], data.Data[4]};
                        var externalTemperature = new[] {data.Data[5], data.Data[6]};

                        InternalSensor.OnNext(ConvertToTempearture(internalTemperature));
                        ExternalSensor.OnNext(ConvertToTempearture(externalTemperature));
                    }
                });
        }

        private TemperatureReading ConvertToTempearture(byte[] values)
        {
            if (values.Length != 2)
            {
                return TemperatureReading.Failed;
            }

            if (values[0] == 255)
            {
                return TemperatureReading.Disconnected;
            }

            if (values[0] > 128)
            {
                var temperature = -1*(256 - values[0]) + ~(values[1] >> 4)/16f;

                return new TemperatureReading(temperature);
            }
            else
            {
                var temperature = values[0] + (values[1] >> 4)/16f;

                return new TemperatureReading(temperature);
            }
        }

        public void Dispose()
        {
            if (device != null)
            {
                if (device.IsOpen)
                {
                    device.CloseDevice();
                }
            }
        }
    }
}