using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

namespace Assets.REST_Client.Scripts.HttpInterface
{
    public class RestUnityWebHttpRequest : MonoBehaviour, IHttpRequest
    {
        private RestMethod _method;
        private RestBody _body;
        private string _url;
        private Dictionary<string, string> _headers;

        public void InitRequest(RestMethod method, string url, RestBody body, Uri proxy)
        {
            _method = method;
            _body = body;
            _url = url;
            _headers = new Dictionary<string, string>();

            if (proxy != null) throw new NotImplementedException("UnityWebHttpRequest does not support a Proxy");
        }

        public void AddHeader(string headerName, string body)
        {
            _headers.Add(headerName, body);
        }

        public HttpResponse Response { get; private set; }

        public IEnumerator Send()
        {
            yield return DoSend();
        }

        private IEnumerator DoSend()
        {
            Response = new HttpResponse();

            UnityWebRequest request;
            switch (_method)
            {
                case RestMethod.Put:
                    request = UnityWebRequest.Put(_url, _body.GetBytes());
                    break;
                case RestMethod.Delete:
                    request = UnityWebRequest.Delete(_url);
                    break;
                case RestMethod.Post:
                    request = new UnityWebRequest(_url);
                    request.method = "POST";
                    request.uploadHandler = new UploadHandlerRaw(_body.GetBytes());
                    request.downloadHandler = new DownloadHandlerBuffer();
                    break;
                default:
                    request = UnityWebRequest.Get(_url);
                    break;
            }
            foreach (var header in _headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            yield return request.SendWebRequest();

            Response.Status = (int)request.responseCode;
            Response.SetHeaders(request.GetResponseHeaders());
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Response.Error = request.error;
            }
            else
            {
                // Or retrieve results as binary data
                Response.Bytes = request.downloadHandler.data;
            }
        }

    }
}
