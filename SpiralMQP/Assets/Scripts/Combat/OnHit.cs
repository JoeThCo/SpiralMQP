using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public Sprite hitSprite;
    public Sprite idleSprite;
    public bool IsHit = false;
    public int HitCD = 60;
    
    public void Start(){
        idleSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void Update(){
        if(IsHit == true){
            HitCD--;
        }
        if(HitCD <= 0){
            GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
            IsHit = false;
        }
    }
    public void Hit(){
        Debug.Log("enemy hit");
        GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;
        IsHit = true;
        HitCD = 60;
    }
}
