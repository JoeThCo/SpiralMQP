using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : SingletonAbstract<LoadingManager>
{
    [SerializeField] RectTransform transitionRect;
    [SerializeField] float totalTransitionTime = 1f;

    int movement = 1000;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadAScene(string nextScene)
    {
        StartCoroutine(loadASceneI(nextScene));
    }

    IEnumerator loadASceneI(string nextScene)
    {
        yield return new WaitForSeconds(totalTransitionTime * .5f);
    }
}
