using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingManager : SingletonAbstract<LoadingManager>
{
    [SerializeField] RectTransform transitionRect;
    [SerializeField] float totalTransitionTime = 1f;

    int movement = 2000;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
        transitionRect?.DOAnchorPosX(-movement, 0f).SetUpdate(true);
    }

    /// <summary>
    /// Loads a scene with a transition
    /// </summary>
    /// <param name="nextScene"></param>
    public void LoadSceneWithTransistion(string nextScene)
    {
        StartCoroutine(loadASceneI(nextScene));
    }

    IEnumerator loadASceneI(string nextScene)
    {
        //sets transition to off screen left
        transitionRect?.DOAnchorPosX(-movement, 0f).SetUpdate(true);

        //left to middle
        transitionRect?.DOAnchorPosX(0, totalTransitionTime * .5f).SetUpdate(true).SetEase(Ease.InCubic);
        yield return new WaitForSecondsRealtime(totalTransitionTime * .5f);

        //Asyc call for scene loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        //waits for scene to be done loading
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //make it seem like we are loading
        if (nextScene.Equals("Game"))
        {
            yield return new WaitForSecondsRealtime(totalTransitionTime);
        }

        //middle to right
        transitionRect?.DOAnchorPosX(movement, totalTransitionTime * .5f).SetUpdate(true).SetEase(Ease.OutCubic);
        yield return new WaitForSecondsRealtime(totalTransitionTime * .5f);
    }
}