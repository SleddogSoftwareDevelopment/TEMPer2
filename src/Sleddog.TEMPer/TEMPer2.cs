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

            return TemperatureReading.Failed;
        }
    }
}