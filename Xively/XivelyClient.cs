using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Xively
{
    public class XivelyClient
    {
        static readonly IPAddress XivelyIP = Dns.GetHostEntry("api.xively.com").AddressList[0];

        private string apiKey;
        private string userAgent;

        public XivelyClient(string apiKey, string userAgent = "NETMF")
        {
            this.apiKey = apiKey;
            this.userAgent = userAgent;
        }

        public void SetFeedLocation(int feedId, double lat, double lon, string disposition = null, string ele = null, string exposure = null, string domain = null)
        {
            var uri = @"http://api.xively.com/v2/feeds/" + feedId;
            var text = new StringBuilder("{\"version\":\"1.0.0\",\"location\":{");
            
            if (disposition != null)
                text.Append("\"disposition\":\"").Append(disposition).Append("\",");

            if (ele != null)
                text.Append("\"ele\":\"").Append(ele).Append("\",");

            if (exposure != null)
                text.Append("\"exposure\":\"").Append(exposure).Append("\",");

            if (domain != null)
                text.Append("\"domain\":\"").Append(domain).Append("\",");

            text.Append("\"lat\":\"").Append(lat).Append("\",\"lon\":\"").Append(lon).Append("\"}}");

            HttpPostJson(uri, text.ToString());
        }

        public void WriteToFeed(int feedId, XivelyDataPoint point)
        {
            var uri = @"http://api.xively.com/v2/feeds/" + feedId + "/datastreams/" + point.StreamId;
            var text = point.ToString();

            HttpPostJson(uri, text);
        }

        public void WriteToFeed(int feedId, params XivelyDataPoint[] values)
        {
            if (values.Length == 1)
            {
                WriteToFeed(feedId, values[0]);
            }

            var uri = @"http://api.xively.com/v2/feeds/" + feedId;
            var text = new StringBuilder("{\"version\":\"1.0.0\",\"datastreams\":[");
            for (int i = 0; i < values.Length; ++i)
            {
                if (i > 0)
                    text.Append(",");
                text.Append(values[i].ToString());
            }
            text.Append("]}");

            HttpPostJson(uri, text.ToString());
        }

        private void HttpPostJson(string uri, string content)
        {
            var request = "PUT " + uri + " HTTP/1.1\r\n";
            var headers = "X-ApiKey: " + apiKey + "\r\nUser-Agent: " + userAgent + "\r\nContent-Type: application/json\r\nContent-Length: " + content.Length + "\r\nConnection: Keep-Alive\r\nHost: app.xively.com\r\n\r\n";

            var remoteEndPoint = new IPEndPoint(XivelyIP, 80);
            using (var connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                connection.Connect(remoteEndPoint);

                Send(connection, request);
                Send(connection, headers);
                Send(connection, content);

                var buffer = new byte[256];
                connection.Receive(buffer, SocketFlags.None);

                connection.Close();
                return;
            }
        }

        private static void Send(Socket connection, string text)
        {
            //Microsoft.SPOT.Debug.Print(text);
            connection.Send(Encoding.UTF8.GetBytes(text));
        }
    }
}
