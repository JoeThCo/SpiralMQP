using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHit: MonoBehaviour
{
    public int MaxHP = 1000;
    public int HP;
    bool IsHit = false;
    float HitTime = 0.3f;

    
    void Start(){
        HP = MaxHP;
    }

    
    public void Hit(){
        Debug.Log("enemy hit");
        if(!IsHit)
            StartCoroutine(HitAnimation());
        HP --;
        if(HP<=0){
            GameObject.Find("Canvas").GetComponent<MenuController>().ShowMenu("GameOver");
        }
    }

    IEnumerator HitAnimation(){
        IsHit = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(HitTime);
        GetComponent<SpriteRenderer>().color = Color.white;
        IsHit = false;
    }
}
