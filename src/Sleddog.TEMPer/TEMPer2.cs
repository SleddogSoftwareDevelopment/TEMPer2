namespace Sleddog.TEMPer
{
    public class TEMPer2
    {
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

            return TemperatureReading.Failed;
        }
    }
}