using System;
using System.Collections;
using System.Text;

namespace Assets.REST_Client.Scripts
{
    public enum BodyContents
    {
        Empty = 0,
        Json = 1,
        Audio = 2,
        Xml = 3,
        EmptyWithClientAuth = 4
    }

    public class RestBody
    {
        public BodyContents ContentType { get; private set; }
        public string XmlBody { get; private set; }
        public byte[] AudioSamples { get; private set; }
        public Hashtable JsonBody { get; private set; }

        public RestBody()
        {
            ContentType = BodyContents.Empty;
            JsonBody = new Hashtable();
        }

        public RestBody(string xmlBody)
        {
            ContentType = BodyContents.Xml;
            XmlBody = xmlBody;
            JsonBody = new Hashtable();
        }

        public RestBody(byte[] samples)
        {
            ContentType = BodyContents.Audio;
            AudioSamples = samples;
            JsonBody = new Hashtable();
        }

        public RestBody(Hashtable jsonBody)
        {
            ContentType = BodyContents.Json;
            JsonBody = jsonBody;
        }

        public byte[] GetBytes()
        {
            switch (ContentType)
            {
                case BodyContents.Audio:
                    return AudioSamples;
                case BodyContents.EmptyWithClientAuth:
                case BodyContents.Json:
                    return Encoding.UTF8.GetBytes(Json.JsonEncode(JsonBody));
                case BodyContents.Xml:
                    return Encoding.UTF8.GetBytes(XmlBody);
                default:
                    return null;
            }
        }

        public int Length()
        {
            switch (ContentType)
            {
                case BodyContents.Audio:
                    return AudioSamples.Length;
                case BodyContents.EmptyWithClientAuth:
                case BodyContents.Json:
                    return Encoding.UTF8.GetBytes(Json.JsonEncode(JsonBody)).Length;
                case BodyContents.Xml:
                    return Encoding.UTF8.GetBytes(XmlBody).Length;
                default:
                    return 0;
            }
        }
    }
}
