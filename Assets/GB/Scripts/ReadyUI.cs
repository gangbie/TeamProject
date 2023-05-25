using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private GameSceneFlow flow;

    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void OnPlay()
    {
        flow.Play();
    }
}