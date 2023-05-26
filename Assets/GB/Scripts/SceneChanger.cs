using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeSceneByName(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
        
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
