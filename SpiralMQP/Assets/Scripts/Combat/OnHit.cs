using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public void Hit(){
        Debug.Log("enemy hit");
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
