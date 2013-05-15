using Xively;

namespace XivelyConsole
{
    public class Program
    {
        public static void Main()
        {
            var feedId = 68475;

            var xively = new XivelyClient(apiKey: @"YOUR_KEY_HERE");

            xively.SetFeedLocation(feedId, lat: 38.90, lon: -77.26);

            xively.WriteToFeed(feedId, values: new[] { 
                new XivelyDataPoint { StreamId = "x", CurrentValue = "210" },
                new XivelyDataPoint { StreamId = "y", CurrentValue = "124" },
                new XivelyDataPoint { StreamId = "z", CurrentValue = "125" }
            });
        }
    }
}
