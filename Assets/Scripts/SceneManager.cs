using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public Animator animTransition;

    private float fTransitionTime = 1.0f;

    public void ChangeScene(string _sceneName)
    {
        StartCoroutine(LoadingScene(_sceneName));
    }

    IEnumerator LoadingScene(string _sceneName)
    {
        animTransition.SetTrigger("Start");

        yield return new WaitForSeconds(fTransitionTime);

        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}
