using System;
using System.Collections;
using Assets.REST_Client.Scripts.HttpInterface;
using UnityEngine;

namespace Assets.REST_Client.Scripts
{
    /// <summary>
    /// Authorization Type Used when connecting to REST Service
    /// </summary>
    public enum AuthType
    {
        /// <summary>
        /// No Authorization
        /// </summary>
        None,

        /// <summary>
        /// Client Authentication, used for 2-legged Authentication in OAuth 2
        /// </summary>
        Client,

        /// <summary>
        /// Bearer Authorization in the Authorization Header for this call
        /// </summary>
        Bearer,

        /// <summary>
        /// Bare Authorization used for custom Authorization Scheme in the Authorization Header
        /// </summary>
        Bare
    }

    /// <summary>
    /// A Request to Send over the selected HttpInterface
    /// </summary>
    public class RestRequest
    {
        /// <summary>
        /// Full Uri used to access the resource
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Tag associated with this request that will appear in the response event
        /// </summary>
        public string RequestTag { get; private set; }

        /// <summary>
        /// Audio Sample Rate when body contains Audio Sample
        /// </summary>
        public int AudioSampleRate { get; private set; }

        /// <summary>
        /// Object to recieve event on completion of Rest Request
        /// </summary>
        public GameObject ObjectToNotify { get; private set; }

        private Hashtable _filterParams;
        private Hashtable _extraQuery;
        private Hashtable _headers;
        private AuthType _authType = AuthType.None;
        private Action<RestResponse> _callback;
        private readonly RestManager _restParams;

        /// <summary>
        /// Used by the REST Manager to create a REST Request object.  You should not need to create this directly.
        /// </summary>
        /// <param name="restParams">params used to populate the REST Request object</param>
        /// <param name="objectToNotify">GameObject that received events for this REST Request</param>
        /// <param name="baseUrl">Base URL of the REST service being called</param>
        /// <param name="resourceName">Resource Name of the REST service being called</param>
        public RestRequest(RestManager restParams, GameObject objectToNotify, string baseUrl, string resourceName)
        {
            _restParams = restParams;
            ObjectToNotify = objectToNotify;

            Path = baseUrl;

            if (!string.IsNullOrEmpty(resourceName))
            {
                Path += "/" + resourceName;
            }
        }

        #region Fluent Config

        /// <summary>
        /// Use Client Authentication for this call, details specified by the REST Manager.  
        /// </summary>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest WithClientAuth()
        {
            _authType = AuthType.Client;

            return this;
        }

        /// <summary>
        /// Use Authorization Header for this call with  custom Authorization Scheme, details specified by the REST Manager.
        /// </summary>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest WithBareClientAuth()
        {
            _authType = AuthType.Bare;

            return this;
        }

        /// <summary>
        /// Use Bearer Authorization in the Authorization Header for this call, details specified by the REST Manager.
        /// </summary>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest WithBearerAuth()
        {
            _authType = AuthType.Bearer;

            return this;
        }

        /// <summary>
        /// Tag associated with this request that is returned with the response, used to differentiate between multiple REST Request Responses
        /// </summary>
        /// <param name="requestTag">string containing Tag to associated with call</param>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest WithTag(string requestTag)
        {
            RequestTag = requestTag;

            return this;
        }

        /// <summary>
        /// Audio Sample Rate when body contains Audio Sample
        /// </summary>
        /// <param name="rate">Waveform sample rate</param>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest SetSampleRate(int rate)
        {
            AudioSampleRate = rate;

            return this;
        }

        /// <summary>
        /// Use this to add filter paramter to your query string, each will appear as part of the filter=[key]=[value] parameter on the query string
        /// </summary>
        /// <param name="key">Key of the filter parameter to add</param>
        /// <param name="value">Value of the filter parameter to add</param>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest AddFilterParameter(string key, object value)
        {
            if (_filterParams == null)
            {
                _filterParams = new Hashtable();
            }
            _filterParams[key] = value;

            return this;
        }

        /// <summary>
        /// Use this to add paramter to your query string, each will appear as part of the &[key]=[value] parameter on the query string
        /// </summary>
        /// <param name="key">Key of the parameter to add</param>
        /// <param name="value">Value of the parameter to add</param>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest AddQueryStringParameter(string key, object value)
        {
            if (_extraQuery == null)
            {
                _extraQuery = new Hashtable();
            }
            _extraQuery[key] = value;

            return this;
        }

        /// <summary>
        /// Add a header to the REST Request
        /// </summary>
        /// <param name="key">Key of the header to add</param>
        /// <param name="value">Value of the header to add</param>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest AddHeader(string key, object value)
        {
            if (_headers == null)
            {
                _headers = new Hashtable();
            }
            _headers[key] = value;

            return this;
        }

        /// <summary>
        /// Request a resource by id
        /// </summary>
        /// <param name="id">id of the resource requested</param>
        /// <returns>This RestRequest allowing for Fluent API consumption</returns>
        public RestRequest ResourceById(string id)
        {
            Path += "/" + id;
            return this;
        }

        #endregion

        #region HTTP Verbs

        /// <summary>
        /// Execute a Get to the REST service use the parameters specified in this object
        /// </summary>
        /// <param name="callback">Method to call on execution completion</param>
        /// <returns>IEnumerator suitable for call using StartCoroutine</returns>
        public IEnumerator Get(Action<RestResponse> callback = null)
        {
            _callback = callback;

            return PerformRequest(RestMethod.Get, new RestBody());
        }

        /// <summary>
        /// Execute a Post to the REST service use the parameters specified in this object
        /// </summary>
        /// <param name="body">Body of the request</param>
        /// <param name="callback">Method to call on execution completion</param>
        /// <returns>IEnumerator suitable for call using StartCoroutine</returns>
        public IEnumerator Post(RestBody body = null, Action<RestResponse> callback = null)
        {
            _callback = callback;

            if (body == null) body = new RestBody();
            return PerformRequest(RestMethod.Post, body);
        }

        /// <summary>
        /// Execute a Put to the REST service use the parameters specified in this object
        /// </summary>
        /// <param name="body">Body of the request</param>
        /// <param name="callback">Method to call on execution completion</param>
        /// <returns>IEnumerator suitable for call using StartCoroutine</returns>
        public IEnumerator Put(RestBody body = null, Action<RestResponse> callback = null)
        {
            _callback = callback;

            if (body == null) body = new RestBody();
            return PerformRequest(RestMethod.Put, body);
        }

        /// <summary>
        /// Execute a Delete to the REST service use the parameters specified in this object
        /// </summary>
        /// <param name="body">Body of the request</param>
        /// <param name="callback">Method to call on execution completion</param>
        /// <returns>IEnumerator suitable for call using StartCoroutine</returns>
        public IEnumerator Delete(RestBody body = null, Action<RestResponse> callback = null)
        {
            _callback = callback;

            if (body == null) body = new RestBody();
            return PerformRequest(RestMethod.Delete, body);
        }

        #endregion

        private IEnumerator PerformRequest(RestMethod method, RestBody body)
        {
            string url = Path + BuildQueryString();

            var request = BuildRequest(ObjectToNotify, method, body, url);

            // Perform httpRequest
            yield return request.Send();
            ResponseReceived(request);

            RestFactory.DestroyRequest(request);
        }

        private void ResponseReceived(IHttpRequest httpRequest)
        {
            if (httpRequest.Response == null)
            {
                RestException exception = RestExceptionFactory.Create(RestExceptionFactory.ServerNotAvailable, null, RequestTag);

                ObjectToNotify.SendMessage("OnRestError", exception);
                _callback(null);

                return;
            }

            if (!String.IsNullOrEmpty(httpRequest.Response.Error))
            {
                RestException exception = RestExceptionFactory.Create(RestExceptionFactory.HttpError, httpRequest.Response.Error, RequestTag);

                ObjectToNotify.SendMessage("OnRestError", exception);
                _callback(null);

                return;
            }


            if (httpRequest.Response.Status >= 400)
            {
                RestException exception = RestExceptionFactory.Create(httpRequest.Response.Status, httpRequest.Response.Text, RequestTag);

                ObjectToNotify.SendMessage("OnRestError", exception);
                _callback(null);
                return;
            }

            bool result = false;
            object responseResult;
            if (httpRequest.Response.GetHeader("Content-Type").StartsWith("application/json"))
            {
                responseResult = Json.JsonDecode(httpRequest.Response.Text, ref result);
            }
            else if (httpRequest.Response.GetHeader("Content-Type").StartsWith("audio/x-wav") || httpRequest.Response.GetHeader("Content-Type").StartsWith("audio/basic"))
            {
                responseResult = httpRequest.Response.Bytes;
                result = true;
            }
            else
            {
                responseResult = null;
                if (httpRequest.Response.Status == 204)
                {
                    result = true;
                }
            }
            if (!result)
            {
                RestException exception = RestExceptionFactory.Create(RestExceptionFactory.WrongResponseFormat, httpRequest.Response.Text, RequestTag);

                ObjectToNotify.SendMessage("OnRestError", exception);
                _callback(null);
                return;
            }

            bool hasError = httpRequest.Response.Status >= 300;
            if (hasError)
            {
                RestException exception = RestExceptionFactory.Create(httpRequest.Response.Status, responseResult, RequestTag);

                ObjectToNotify.SendMessage("OnRestError", exception);
                _callback(new RestResponse(httpRequest, exception, RequestTag));
            }
            else if (responseResult is ArrayList)
            {
                _callback(new RestResponse(httpRequest, httpRequest.Response.Text, (ArrayList)responseResult, RequestTag));
            }
            else if (responseResult is Hashtable)
            {
                _callback(new RestResponse(httpRequest, httpRequest.Response.Text, (Hashtable)responseResult, RequestTag));
            }
            else if (responseResult is byte[])
            {
                _callback(new RestResponse(httpRequest, httpRequest.Response.Text, (byte[])responseResult, RequestTag));
            }
            else if (responseResult is string)
            {
                _callback(new RestResponse(httpRequest, (string)responseResult, RequestTag));
            }
            else if (responseResult == null && httpRequest.Response.Status == 204)
            {
                _callback(new RestResponse(httpRequest, RequestTag));
            }
            else if (responseResult == null)
            {
                Debug.LogWarning("REST Response Result is null");
            }
            else
            {
                Debug.LogWarning("Unsupported type in response: " + responseResult.GetType());
                _callback(null);
            }
        }

        private IHttpRequest BuildRequest(GameObject parent, RestMethod method, RestBody body, string url)
        {
            // Handle authentication for Client authentication
            if (_authType == AuthType.Client)
            {
                if (body.ContentType == BodyContents.Audio || body.ContentType == BodyContents.Xml)
                {
                    throw new NotSupportedException("Did not implement Client Auth + this type of body contents");
                }

                body.JsonBody.Add("client_id", _restParams.GetClientId());
                body.JsonBody.Add("client_secret", _restParams.GetClientSecret());
            }

            // Create the httpRequest
            var result = RestFactory.GetRequest(parent, method, url, body);

            // Add content type header
            switch (body.ContentType)
            {
                case BodyContents.Json:
                    result.AddHeader("Content-Type", "application/json");
                    break;
                case BodyContents.Xml:
                    result.AddHeader("Content-Type", "application/ssml+xml");
                    break;
                case BodyContents.Audio:
                    result.AddHeader("Content-Type", "audio/wav; samplerate=" + AudioSampleRate);
                    break;
                case BodyContents.Empty:
                case BodyContents.EmptyWithClientAuth:
                    break;
                default:
                    throw new Exception("Unknown ContentType");
            }

            // Handle authentication for all other authentication types
            switch (_authType)
            {
                case AuthType.Bare:
                    result.AddHeader("Authorization", _restParams.GetBareClientAuth());
                    break;
                case AuthType.Bearer:
                    result.AddHeader("Authorization", "Bearer " + _restParams.GetAccessToken());
                    break;
                case AuthType.None:
                    break;
            }

            // Add Extra Headers
            if (_headers != null)
            {
                foreach (var headerKey in _headers.Keys)
                {
                    result.AddHeader(headerKey.ToString(), _headers[headerKey].ToString());
                }
            }

            return result;
        }

        private string BuildQueryString()
        {
            string result = string.Empty;

            // filtering
            if (_filterParams != null && _filterParams.Count > 0)
            {
                string filterValue = Json.JsonEncode(_filterParams);
                result += "&filter=" + filterValue;
            }

            // extra params
            if (_extraQuery != null && _extraQuery.Count > 0)
            {
                foreach (string key in _extraQuery.Keys)
                {
                    result += "&" + key + "=" + _extraQuery[key];
                }
            }

            // replace the initial & with a ? at the front
            if (!string.IsNullOrEmpty(result))
            {
                result = "?" + result.Substring(1);
            }

            return result;
        }
    }
}
