using System;
using Assets.REST_Client.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.REST_Client.Sample_Scene
{
    public class Nasa : MonoBehaviour {

        public Text OutputResult;
        public void ContactNasa ()
        {
            var rest = GetComponent<RestManager>();
	
            StartCoroutine(rest
                .SetBaseUrl("https://api.nasa.gov")
	            .ResourceAt("planetary/apod")
	            .AddQueryStringParameter("api_key","<API Key Here, obtained from https://api.nasa.gov/#getting-started>")
                .Get(response =>
                    {
                        if (response == null)
                        {
                            Debug.LogError("Problem communicating with Rest API");
                        }
                        else
                        {
                            OutputResult.text = response.Resource["title"] + "\n" + response.Resource["explanation"];
                        }
                    }
                ));
        }

        void OnRestError(object boxedError)
        {
            var restError = boxedError as RestException;

            if (restError != null)
            {
                if (restError.ErrorRaw != null)
                {
                    OutputResult.text = "Rest Error: " + restError.ErrorRaw["message"];
                }
                else if (restError.ErrorListRaw != null)
                {
                    OutputResult.text = "Rest Error: " + String.Join("|", Array.ConvertAll(restError.ErrorListRaw.ToArray(), x => x.ToString()));
                }
                else
                {
                    OutputResult.text = "Rest Error: "  + restError.Message;
                }
            }
        }
    }
}
