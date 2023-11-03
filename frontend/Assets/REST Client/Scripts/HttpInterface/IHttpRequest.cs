using System;
using System.Collections;

namespace Assets.REST_Client.Scripts.HttpInterface
{
    public enum RestMethod
    {
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4
    }

    public interface IHttpRequest
    {
        HttpResponse Response { get; }
        void AddHeader(string headerName, string body);
        IEnumerator Send();
        void InitRequest(RestMethod method, string url, RestBody body, Uri proxy);
    }

}