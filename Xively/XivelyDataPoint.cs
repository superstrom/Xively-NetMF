namespace Xively
{
    public struct XivelyDataPoint
    {
        public string StreamId { get; set; }
        public string CurrentValue { get; set; }

        public override string ToString()
        {
            return @"{""current_value"":""" + CurrentValue + @""", ""id"":""" + StreamId + @"""}";
        }
    }
}
