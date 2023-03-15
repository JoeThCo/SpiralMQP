using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class JuiceHelper : MonoBehaviour
{
    [SerializeField] float lerpTime = .25f;
    [SerializeField] float lerpSize = 1.15f;

    Vector3 lerpScale;

    private void Awake()
    {
        lerpScale = transform.localScale;
    }

    public void OnEnter()
    {
        gameObject?.transform.DOScale(Vector3.one * lerpSize, lerpTime).SetUpdate(true);
    }

    public void OnExit()
    {
        gameObject?.transform.DOScale(lerpScale, lerpTime).SetUpdate(true);
    }
}