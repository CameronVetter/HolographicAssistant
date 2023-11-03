using System;
using System.Collections;
using Assets.REST_Client.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Assistant : MonoBehaviour
{
    public Text OutputText;
    public Speech Speaker;
    public Animate CharacterToAnimate;

    private object threadLocker = new object();
    private bool Working;
    private string OpenAiResponse;
    private string HumanPrompt;

    void Start()
    {
        Working = false;
        OpenAiResponse = string.Empty;
        HumanPrompt = String.Empty;
    }

    void Update()
    {
        lock (threadLocker)
        {
            if (!string.IsNullOrEmpty(HumanPrompt))
            {
                SendChat();
                HumanPrompt = string.Empty;
            }

            if (OpenAiResponse != string.Empty)
            {
                Speaker.Speak(OpenAiResponse);
                OpenAiResponse = string.Empty;
            }
        }
    }

    public void StartListening()
    {
        CharacterToAnimate.StartListening();
    }

    public void HumanPromptReceived(string toSpeak)
    {
        CharacterToAnimate.StartThinking();
        HumanPrompt = toSpeak;
    }

    public void SendChat()
    {
        if (Working) 
        {
            Console.WriteLine("Already processing chat.");
            return;
        }
        lock (threadLocker)
        {
            Working = true;
        }

        var rest = GetComponent<RestManager>();
        var json = new Hashtable
        {
            { "Prompt", HumanPrompt }
        };

        StartCoroutine(rest
            .SetBaseUrl("<INSERT BACKEND URL HERE>")
            .ResourceAt("chat")
            .Post(new RestBody(json), response =>
                {
                    if (response == null)
                    {
                        Debug.LogError("Problem communicating with Rest API");
                    }
                    else
                    {
                        var message = response.Resource["Response"].ToString();
                        OutputText.text = message;
                        OutputText.color = Color.red;

                        lock (threadLocker)
                        {
                            if (message != null)
                            {
                                OpenAiResponse = message;
                            }

                            Working = false;
                        }
                    }
                }
            )
        );
    }

    void OnRestError(object boxedError)
    {
        var restError = boxedError as RestException;
        string error;

        if (restError != null)
        {
            if (restError.ErrorRaw != null)
            {
                error = "Rest Error: " + restError.ErrorRaw["message"];
            }
            else if (restError.ErrorListRaw != null)
            {
                error = "Rest Error: " + String.Join("|", Array.ConvertAll(restError.ErrorListRaw.ToArray(), x => x.ToString()));
            }
            else
            {
                error = "Rest Error: " + restError.Message;
            }
            OutputText.text = error;
            Console.WriteLine(error);
        }
    }
}
