using System;
using System.Collections;
using UnityEngine;

public class Animate : MonoBehaviour
{
    public AudioClip AcknowledgementSound;
    public AudioSource audioSource;
    private Animator CharacterAnimator;
    public DateTime? BoredStartTime;
    private const int TIME_TO_BORED_MIN = 10;
    private const int TIME_TO_BORED_MAX = 60;
    private const int BORED_TO_SLEEP_COUNT = 3;
    private int boredCount;
    private bool floating = false;
    private IEnumerator activeCoroutine;
    private readonly System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = gameObject.GetComponent<Animator>();
        MakeIdle();
    }

    // Update is called once per frame
    void Update()
    {
        if (BoredStartTime != null && BoredStartTime < DateTime.Now)
        {
            MakeBored();
        }
    }

    public void StartListening ()
    {
        audioSource.clip = AcknowledgementSound;
        audioSource.Play();
    }

    public void StartTalking()
    {
        MakeTalk();
    }

    public void StopTalking()
    {
        MakeIdle();
    }

    public void StartThinking()
    {
        MakeThink();
    }

    public void StopThinking()
    {
        MakeIdle();
    }

    private void MakeThink()
    {
        Debug.Log("Make Think");
        BecameActive();
        startFloatingThink();
        CharacterAnimator.CrossFade("Elephant Roll In Place", .25f);
    }

    private void MakeTalk()
    {
        Debug.Log("Make Talk");
        startStanding();
        BecameActive();
        CharacterAnimator.CrossFade("Elephant Talk", .25f);
    }

    private void MakeIdle()
    {
        Debug.Log("Make Idle");
        startStanding();
        CharacterAnimator.CrossFade("Elephant Idle", .25f);
        SetNextBoredStartTime();
    }

    private void MakeBored()
    {
        Debug.Log($"Make Bored {boredCount}");
        if (boredCount < BORED_TO_SLEEP_COUNT)
        {
            SetNextBoredStartTime();
            boredCount++;
            CharacterAnimator.CrossFade("Elephant Idle 2", .25f);
        }
        else
        {
            BoredStartTime = null;
            CharacterAnimator.CrossFade("Elephant Sleep", .25f);
            startFloatingSleep();
        }
    }
    private void SetNextBoredStartTime()
    {
        BoredStartTime = DateTime.Now.AddSeconds(random.Next(TIME_TO_BORED_MIN, TIME_TO_BORED_MAX));
    }

    private void BecameActive()
    {
        Debug.Log("Became Active");
        boredCount = 0;
        BoredStartTime = null;
    }

    private void startFloatingSleep ()
    {
        if (!floating)
        {
            if (activeCoroutine != default) StopCoroutine(activeCoroutine);
            activeCoroutine = SetFloatSleepPosition();
            StartCoroutine(activeCoroutine);
        }
    }

    private void startFloatingThink()
    {
        if (!floating)
        {
            if (activeCoroutine != default) StopCoroutine(activeCoroutine);
            activeCoroutine = SetFloatThinkPosition();
            StartCoroutine(activeCoroutine);
        }
    }

    private void startStanding()
    {
        if (floating)
        {
            if (activeCoroutine != default) StopCoroutine(activeCoroutine);
            StopCoroutine(activeCoroutine);
            activeCoroutine = SetStandPosition();
            StartCoroutine(activeCoroutine);
        }
    }

    IEnumerator SetFloatSleepPosition()
    {
        Debug.Log("Floating Sleep");
        var timeElapsed = 0;
        var newPos = new Vector3(-.65f , -2.25f, .3f);
        var newRot = Quaternion.Euler(0f, -65f, -70f);
        const int duration = 30;

        floating = true;
        while (timeElapsed < duration)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPos, timeElapsed / duration);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRot, timeElapsed / duration);
            timeElapsed++;
            yield return null;
        }

        gameObject.transform.position = newPos;
        gameObject.transform.rotation = newRot;
    }

    IEnumerator SetFloatThinkPosition()
    {
        Debug.Log("Floating Think");
        var timeElapsed = 0;
        var newPos = new Vector3(-.15f, -3.25f, .3f);
        var newRot = Quaternion.Euler(0f, 0f, 0f);
        const int duration = 30;

        floating = true;
        while (timeElapsed < duration)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPos, timeElapsed / duration);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRot, timeElapsed / duration);
            timeElapsed++;
            yield return null;
        }

        gameObject.transform.position = newPos;
        gameObject.transform.rotation = newRot;
    }

    IEnumerator SetStandPosition()
    {
        Debug.Log("Standing");
        var timeElapsed = 0;
        var newPos = new Vector3(-.15f, -4.3f, 0f);
        var newRot = Quaternion.Euler(0f, 0f, 0f);
 
        floating = false;
        while (timeElapsed < 30)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPos, Time.deltaTime * timeElapsed);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRot, Time.deltaTime * timeElapsed);
            timeElapsed++;
            yield return null;
        }

        gameObject.transform.position = newPos;
        gameObject.transform.rotation = newRot;
    }
}
