using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public Sprite hitSprite;
    public Sprite idleSprite;
    bool IsHit = false;
    float HitTime = 0.3f;


    
    void Start(){
        idleSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    
    public void Hit(){
        Debug.Log("enemy hit");
        if(!IsHit)
            StartCoroutine(HitAnimation());
    }

    IEnumerator HitAnimation(){
        IsHit = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;
        yield return new WaitForSeconds(HitTime);
        GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
        IsHit = false;
    }
}
