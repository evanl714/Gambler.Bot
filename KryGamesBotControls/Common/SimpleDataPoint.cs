namespace KryGamesBotControls.Common
{
    public class SimpleDataPoint
    {

        public double Argument { get; private set; }
        public double Value { get; private set; }

        public SimpleDataPoint(double arg, double val)
        {
            Argument = arg;
            Value = val;
        }
    }
}