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
    private float waitTime = 2;

    private void Start()
    {
    }
    private void Update()
    {
        StartCoroutine(CoroutineWait());
    }

    IEnumerator CoroutineWait()
    {
        OnUIOff?.Invoke();
        yield return new WaitForSeconds(waitTime);
        waitTime = 1;
        OnCameraBoss?.Invoke();
        yield return new WaitForSeconds(waitTime);
        OnCameraPlayer?.Invoke();
        yield return new WaitForSeconds(waitTime);
        OnWall?.Invoke();
        yield return new WaitForSeconds(waitTime);
        OnPlayed?.Invoke();
    }
}
