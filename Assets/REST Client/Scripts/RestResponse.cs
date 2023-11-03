using System.Collections;
using Assets.REST_Client.Scripts.HttpInterface;

namespace Assets.REST_Client.Scripts
{
    /// <summary>
    /// The Response received after a REST call is completed
    /// </summary>
    public class RestResponse
    {
        /// <summary>
        /// True if this is an ArrayList stored in ResourceList, False if this is a Hashtable stored in Resource
        /// </summary>
        public bool IsList { get; private set; }

        /// <summary>
        /// Ture if this results contains an error, otherwise false
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// The resource returned from the call formatted for easy consumption
        /// </summary>
        public Hashtable Resource { get; private set; }

        /// <summary>
        /// The resource returned from the call formatted for easy consumption
        /// </summary>
        public ArrayList ResourceList { get; private set; }

        /// <summary>
        /// The resource returned from the call formatted for easy consumption
        /// </summary>
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// Body of the response
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// Exception generate by request if this resulted in an error
        /// </summary>
        public RestException Exception { get; private set; }

        /// <summary>
        /// HTTP Status Code of the response
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// Tag associated with original request
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Original Request that this is the response to
        /// </summary>
        public IHttpRequest HttpRequest { get; private set; }

        /// <summary>
        /// Constructor used to create this RestResponse object
        /// </summary>
        /// <param name="httpRequest">Original Request that this is the response to</param>
        /// <param name="body">Body of the response</param>
        /// <param name="result">The resource returned from the call formatted for easy consumption</param>
        /// <param name="tag">Tag associated with original request</param>
        public RestResponse(IHttpRequest httpRequest, string body, Hashtable result, string tag) : this(httpRequest, body, tag)
        {
            IsList = false;
            Resource = result;
        }

        /// <summary>
        /// Constructor used to create this RestResponse object
        /// </summary>
        /// <param name="httpRequest">Original Request that this is the response to</param>
        /// <param name="body">Body of the response</param>
        /// <param name="result">The resource returned from the call formatted for easy consumption</param>
        /// <param name="tag">Tag associated with original request</param>
        public RestResponse(IHttpRequest httpRequest, string body, ArrayList result, string tag) : this(httpRequest, body, tag)
        {
            IsList = true;
            ResourceList = result;
        }

        /// <summary>
        /// Constructor used to create this RestResponse object if the response contains a byte array
        /// </summary>
        /// <param name="httpRequest">Original Request that this is the response to</param>
        /// <param name="body">Body of the response</param>
        /// <param name="result">The resource returned from the call formatted for easy consumption</param>
        /// <param name="tag">Tag associated with original request</param>
        public RestResponse(IHttpRequest httpRequest, string body, byte[] result, string tag) : this(httpRequest, body, tag)
        {
            Bytes = result;
        }

        /// <summary>
        /// Constructor used to create this RestResponse object
        /// </summary>
        /// <param name="httpRequest">Original Request that this is the response to</param>
        /// <param name="body">Body of the response</param>
        /// <param name="tag">Tag associated with original request</param>
        public RestResponse(IHttpRequest httpRequest, string body, string tag) : this(httpRequest, tag)
        {
            Body = body;
        }

        /// <summary>
        /// Constructor used to create this RestResponse object if response does not contain a body
        /// </summary>
        /// <param name="httpRequest">Original Request that this is the response to</param>
        /// <param name="tag">Tag associated with original request</param>
        public RestResponse(IHttpRequest httpRequest, string tag)
        {
            Status = httpRequest.Response.Status;
            HasError = false;
            HttpRequest = httpRequest;
            Tag = tag;
        }

        /// <summary>
        /// Constructor used to create this RestResponse object
        /// </summary>
        /// <param name="httpRequest">Original Request that this is the response to</param>
        /// <param name="exception">exception returned instead of a valid response</param>
        /// <param name="tag">Tag associated with original request</param>
        public RestResponse(IHttpRequest httpRequest, RestException exception, string tag) : this(httpRequest, tag)
        {
            HasError = true;
            Exception = exception;
        }
    }
}
