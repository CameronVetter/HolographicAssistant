using System;
using UnityEngine;

namespace Assets.REST_Client.Scripts
{
    /// <summary>
    /// Class used to make REST calls, add this to a GameObject and reuse this for multiple REST calls, if multiple simultaneous calls are required create multiple managers,
    /// each RestManager is intended for sequential use.
    /// </summary>
    public class RestManager : MonoBehaviour
    {
        private string _baseUrl;
        private string _clientId;
        private string _clientSecret;
        private string _accessToken;
        private string _bareClientAuth;

        /// <summary>
        /// Configure a proxy for future calls to RestManager. Set this to null to disable a configured proxy.
        /// </summary>
        /// <param name="proxy">The Url of the proxy</param>
        public static void SetProxy(string proxy)
        {
            RestFactory.Proxy = new Uri(proxy);
        }

        /// <summary>
        /// Set the base URL for the REST call, this must include the protocol and should not contain a trailing slash
        /// </summary>
        /// <param name="url">The Base URL of the REST Service</param>
        /// <example>"https://api.nasa.gov"</example>
        /// <returns>This RestManager allowing for Fluent API consumption</returns>
        public RestManager SetBaseUrl(string url)
        {
            _baseUrl = url;
            return this;
        }

        /// <summary>
        /// Use Client Authentication for this call, used for 2-legged Authentication in OAuth 2
        /// </summary>
        /// <param name="clientId">client_id provided by service admin</param>
        /// <param name="clientSecret">client_secret provided by service admin</param>
        /// <returns>This RestManager allowing for Fluent API consumption</returns>
        public RestManager ConfigClientAuth(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            return this;
        }

        /// <summary>
        /// Use Authorization Header for this call with a custom Authorization Scheme
        /// </summary>
        /// <param name="clientAuthString">value to send in the Authorization Header</param>
        /// <returns>This RestManager allowing for Fluent API consumption</returns>
        public RestManager ConfigBareClientAuth(string clientAuthString)
        {
            _bareClientAuth = clientAuthString;
            return this;
        }

        /// <summary>
        /// Use Bearer Authorization in the Authorization Header for this call
        /// </summary>
        /// <param name="accessToken">bearer token to send to service</param>
        /// <returns>This RestManager allowing for Fluent API consumption</returns>
        public RestManager ConfigBearerAuth(string accessToken)
        {
            _accessToken = accessToken;
            return this;
        }

        /// <summary>
        /// Rest Resource to access
        /// </summary>
        /// <param name="resourceName">Name of resource to access</param>
        /// <example>"customers/addresses/home</example>
        /// <returns>RestRequest ready for Execution allowing for Fluent API consumption</returns>
        public RestRequest ResourceAt(string resourceName)
        {
            RestRequest restRequest = new RestRequest(this, gameObject, _baseUrl, resourceName); 
            return restRequest;
        }

        /// <summary>
        /// Retrieve configured ClientId for Client Authentication
        /// </summary>
        /// <returns>string containing ClientId</returns>
        public string GetClientId()
        {
            return _clientId;
        }

        /// <summary>
        /// Retrieve configured ClientSecret for Client Authentication
        /// </summary>
        /// <returns>string containing ClientSecret</returns>
        public string GetClientSecret()
        {
            return _clientSecret;
        }

        /// <summary>
        /// Retrieve configured value for Access Token used for Bearer Client Authentication
        /// </summary>
        /// <returns>string containing Bearer Authorization</returns>
        public string GetAccessToken()
        {
            return _accessToken;
        }

        /// <summary>
        /// Retrieve configured value for Custom Authorization
        /// </summary>
        /// <returns>string containing Custom Authorization</returns>
        public string GetBareClientAuth()
        {
            return _bareClientAuth;
        }
    }
}
