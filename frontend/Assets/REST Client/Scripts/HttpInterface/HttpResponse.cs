using System.Collections.Generic;

namespace Assets.REST_Client.Scripts.HttpInterface
{
    public class HttpResponse
    {
        public int Status { get; set; }
        public string Text { get { return System.Text.Encoding.Default.GetString(Bytes); } }
        public byte[] Bytes { get; set; }
        public string Error { get; set; }

        private Dictionary<string, string> _headers;

        public void SetHeaders(Dictionary<string, string> headers)
        {
            _headers = headers;
        }

        public string GetHeader(string key)
        {
            return _headers[key];
        }
    }
}
