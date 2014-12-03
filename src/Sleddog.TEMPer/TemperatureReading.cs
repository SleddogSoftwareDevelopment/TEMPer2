namespace Sleddog.TEMPer
{
    public class TemperatureReading
    {
        public static readonly TemperatureReading Failed = new TemperatureReading(ReadState.Fail);
        public static readonly TemperatureReading Disconnected = new TemperatureReading(ReadState.Disconnected);

        public ReadState ReadState { private get; set; }
        public float Value { private get; set; }

        private TemperatureReading(ReadState readState)
        {
            ReadState = readState;
        }

        public TemperatureReading(float value)
        {
            ReadState = ReadState.Success;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", ReadState, Value);
        }
    }
}