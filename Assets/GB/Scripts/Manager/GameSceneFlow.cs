using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSceneFlowMap1 : MonoBehaviour
{
    [SerializeField] private State curState;

    public UnityEvent OnPlayed;
    public UnityEvent OnGameOvered;

    private void Start()
    {
        GameOver();
        GameManager.Data.CurScore = 0;
    }

    public void Play()
    {
        curState = State.Play;
        OnPlayed?.Invoke();
    }

    public void GameOver()
    {
        curState = State.GameOver;
        OnGameOvered?.Invoke();
    }

    public enum State
    {
        Play,
        GameOver,
    }
}