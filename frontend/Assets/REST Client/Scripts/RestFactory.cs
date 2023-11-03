using System;
using Assets.REST_Client.Scripts.HttpInterface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.REST_Client.Scripts
{
    /// <summary>
    /// Factory class used to generate REST Request class and inject desired configuration
    /// <remarks>Currently only WebHttpRequest is supported so this is always injected without configuration, this factory is used to extend support to otherr HTTP libraries</remarks>
    /// </summary>
    public static class RestFactory
    {
        /// <summary>
        /// Specify Proxy used for network communication
        /// </summary>
        public static Uri Proxy { get; set; }

        /// <summary>
        /// Generate a IHttpRequest Object of the configured HTTP library.  You should never have to call this, it is handled by REST Manager
        /// </summary>
        /// <param name="parent">GameObject to notify on events</param>
        /// <param name="method">REST Method used for execution</param>
        /// <param name="url">url of the service to call</param>
        /// <param name="body">body of the service to call</param>
        /// <returns></returns>
        public static IHttpRequest GetRequest(GameObject parent, RestMethod method, string url, RestBody body)
        {
            IHttpRequest result = parent.AddComponent<RestUnityWebHttpRequest>();

            result.InitRequest(method, url, body, Proxy);

            return result;
        }

        /// <summary>
        /// Used to cleanup the component after the request is completed
        /// </summary>
        /// <param name="request">The request to cleanup</param>
        public static void DestroyRequest(IHttpRequest request)
        {
            Object.Destroy((MonoBehaviour)request);
        }
    }
}
