using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private SpriteRenderer Sprite;
    public Animator ChestAnimator;
    public bool Opened = false;
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y*100.0f);
        }
    }

    public void Open()
    {
        ChestAnimator.SetBool("Open", true);
        Opened = true;
    }

    public void Close()
    {
        ChestAnimator.SetBool("Open", false);
        Opened = false;
    }
}
