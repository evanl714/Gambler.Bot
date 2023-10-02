namespace KryGamesBot.MAUI.Blazor.Classes
{
    public class SimpleDataPoint
    {

        public double Argument { get; private set; }
        public double? Value { get; private set; }
        public string Color { get; set; }
        public SimpleDataPoint(double arg, double? val)
        {
            Argument = arg;
            Value = val;
        }
        public SimpleDataPoint(double arg, double? val,string color):this(arg,val)
        {
            Color = color;
        }
    }
}