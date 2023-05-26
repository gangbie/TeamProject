using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSceneFlow : MonoBehaviour
{
    [SerializeField] private State curState;

    public UnityEvent OnReadyed;
    public UnityEvent OnPlayed;
    public UnityEvent OnGameOvered;

    private void Start()
    {
        ChangeState(State.Ready);
    }

    public void ChangeState(State state)
    {
        curState = state;
        switch (state)
        {
            case State.Ready:
                OnReadyed?.Invoke();
                GameManager.Data.CurScore = 0;
                break;
            case State.Play:
                OnPlayed?.Invoke();
                break;
            case State.GameOver:
                OnGameOvered?.Invoke();
                break;
        }
    }

    public void Play()
    {
        //SceneManager.LoadScene("Map1");
        ChangeState(State.Play);
    }

    public void GameOver()
    {
        ChangeState(State.GameOver);
    }

    [Serializable]
    public enum State
    {
        Ready,
        Play,
        GameOver,
    }
}