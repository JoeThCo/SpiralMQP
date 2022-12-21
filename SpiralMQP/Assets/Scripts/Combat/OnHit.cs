using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public Sprite hitSprite;
    public Sprite idleSprite;
    public int MaxHP = 1000;
    public int HP;
    bool IsHit = false;
    float HitTime = 0.3f;
    [SerializeField] AudioClip onHitAudio;
    
    void Start(){
        idleSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        HP = MaxHP;
    }


    public void Hit()
    {
        Debug.Log("enemy hit");
        if(!IsHit && hitSprite)
            StartCoroutine(HitAnimation());
        HP --;
        if(HP<=0){
            Destroy(gameObject, 0.1f);
        }
    }

    IEnumerator HitAnimation()
    {
        IsHit = true;

        GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;

        SoundManager.PlayOneShot(onHitAudio, gameObject);

        yield return new WaitForSeconds(HitTime);

        GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
        
        IsHit = false;
    }
}
