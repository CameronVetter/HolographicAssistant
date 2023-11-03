using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;

public class Recognize : MonoBehaviour
{
    public Text outputText;
    public Assistant assistant;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;

    public async void ButtonClick()
    {
        assistant.StartListening();
        // Creates an instance of a speech config with specified subscription key and service region.
        var config = SpeechConfig.FromSubscription("<SPEECH KEY HERE>", "eastus");

        // Make sure to dispose the recognizer after use!
        using (var recognizer = new SpeechRecognizer(config))
        {
            lock (threadLocker)
            {
                waitingForReco = true;
            }

            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
            // single utterance is determined by listening for silence at the end or until a maximum of 15
            // seconds of audio is processed.  The task returns the recognition text as result.
            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
            // shot recognition like command or query.
            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            // Checks result.
            string newMessage = string.Empty;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                newMessage = result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                newMessage = "NOMATCH: Speech could not be recognized.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
            }

            lock (threadLocker)
            {
                message = newMessage;
                waitingForReco = false;
            }
        }
    }

    void Start()
    {
        if (outputText == null)
        {
            UnityEngine.Debug.LogError("outputText property is null! Assign a UI Text element to it.");
        }
        else
        {
            // Continue with normal initialization, Text and Button objects are present.
        }
    }

    void Update()
    {
        lock (threadLocker)
        {
            if (outputText != null && !string.IsNullOrEmpty( message))
            {
                outputText.text = message;
                outputText.color = Color.white;
                assistant.HumanPromptReceived(message);
                message = string.Empty;
            }
        }

        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            ButtonClick();
        }
    }
}