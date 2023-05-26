using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStartScene : MonoBehaviour
{
    public UnityEvent OnCameraPlayer;
    public UnityEvent OnCameraBoss;
    public UnityEvent OnWall;
    public UnityEvent OnUIOff;
    public UnityEvent OnPlayed;
    public UnityEvent OnGameEND;
    private float waitTime = 2;

    private void Update()
    {
        StartCoroutine(CoroutineWait());
    }
    public void GameEnd()
    {
        StartCoroutine(CorutineEND());
    }

    IEnumerator CorutineEND()
    {
        yield return new WaitForSeconds(6);
        OnGameEND?.Invoke();
    }

    IEnumerator CoroutineWait()
    {
        OnUIOff?.Invoke();
        yield return new WaitForSeconds(waitTime);
        waitTime = 1.5f;
        OnCameraBoss?.Invoke();
        yield return new WaitForSeconds(waitTime);
        OnCameraPlayer?.Invoke();
        yield return new WaitForSeconds(waitTime);
        OnWall?.Invoke();
        yield return new WaitForSeconds(waitTime);
        OnPlayed?.Invoke();
    }
}
