using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    //private void Update()
    //{
    //    ChangeScene();
    //}

    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName , LoadSceneMode.Single);
    }
}
