using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField][TextArea] protected string description;
    [SerializeField] protected float time;
    // public float TIME
    // {
    //     get
    //     {
    //         if (gameTime != null)
    //             return gameTime.Value;
    //         else
    //             return time;
    //     }
    // }
    public float timer;
    protected Coroutine coroutineTimeUpdate;
    [Header("Value")]
    [SerializeField] private FloatValue gametimer;
    [Header("Event")]
    [SerializeField] protected GameEvent timerUpdateEvent;


    public void SetTime(float _time)
    {
        time = _time;
    }
    public void StartTimer()
    {
        if (coroutineTimeUpdate != null)
            StopCoroutine(coroutineTimeUpdate);

        coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(time));
    }
    public void StopTimer()
    {
        if (coroutineTimeUpdate != null)
            StopCoroutine(coroutineTimeUpdate);
        coroutineTimeUpdate = null;
    }


    // protected virtual void StartTimeCount()
    // {

    //     if (coroutineTimeUpdate != null)
    //         StopCoroutine(coroutineTimeUpdate);
    //     // timer = TIME;
    //     coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(gameTime.Value));


    // }
    protected virtual IEnumerator GameTimerUpdate(float _gameTime)
    {
        timer = _gameTime;
        while ((timer > 0))
        {
            if (timerUpdateEvent)
                timerUpdateEvent.Raise(this, timer);
            yield return new WaitForSeconds(1);
            if (timerUpdateEvent)
                timerUpdateEvent.Raise(this, timer);

            timer--;
            gametimer.Value = timer;
        }
        if (timerUpdateEvent)
            timerUpdateEvent.Raise(this, timer);
        coroutineTimeUpdate = null;
    }
}
