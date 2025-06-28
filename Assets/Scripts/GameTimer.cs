using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField][TextArea] protected string description;
    [SerializeField] protected float time;
    [SerializeField] private float fetchTimer = 1f;
    public float timer;
    protected Coroutine coroutineTimeUpdate;
    [Header("Value")]
    //[SerializeField] private FloatValue gametimer;
    [SerializeField] private GameDataValue gamedata;
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

    protected virtual IEnumerator GameTimerUpdate(float _gameTime)
    {
        timer = _gameTime;
        while ((timer > 0))
        {
            if (timerUpdateEvent)
                timerUpdateEvent.Raise(this, timer);
            yield return new WaitForSeconds(fetchTimer);
            if (timerUpdateEvent)
                timerUpdateEvent.Raise(this, timer);

            timer -= fetchTimer;
            gamedata.Value.gametimer = timer;
            gamedata.Value.usetime += fetchTimer;
        }
        if (timerUpdateEvent)
            timerUpdateEvent.Raise(this, timer);
        coroutineTimeUpdate = null;
    }
}
