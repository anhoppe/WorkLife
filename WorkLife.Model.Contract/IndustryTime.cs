using System.Globalization;

namespace WorkLife.Model.Contract
{
    public readonly struct IndustryTime
    {
        public double Value { get; }

        public IndustryTime(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            int hours = Math.Abs((int)Value);
            int minutes = (int)Math.Round((Math.Abs(Value) - hours) * 60);
            string prefix = Value < 0 ? "-" : "";
            return $"{prefix}{hours:D2}:{minutes:D2}";
        }

        public static implicit operator double(IndustryTime t) => t.Value;
        public static implicit operator IndustryTime(double d) => new IndustryTime(d);
        public static implicit operator IndustryTime(string str)
        {
            if (double.TryParse(str, CultureInfo.InvariantCulture, out var value))
            {
                return new IndustryTime(value);
            }

            return new IndustryTime(0);
        }
    }
}
