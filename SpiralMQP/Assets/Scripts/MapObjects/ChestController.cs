using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ChestController : MonoBehaviour
{
    private SpriteRenderer Sprite;
    [SerializeField] Animator ChestAnimator;
    [SerializeField] bool Opened = false;
    [SerializeField] ItemSO Item;
    [SerializeField] GameObject CollectableItemPrefab;
    [SerializeField] AudioClip chestOpenAudio;

    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y * 100.0f);
        }
    }

    public void Open()
    {
        if(!Opened){
            ChestAnimator.SetBool("Open", true);
            Opened = true;
    
            SoundManager.PlayOneShot(chestOpenAudio, gameObject);

            Vector3 pos = transform.position + new Vector3(-1.0f, -1.0f, 0.0f);
            Quaternion rotation = transform.rotation;
            GameObject CollectableItem = Instantiate(CollectableItemPrefab, pos, rotation);
            CollectableItem.GetComponent<CollectableItemController>().Item = Item;
        }
    }

    public void Close()
    {
        ChestAnimator.SetBool("Open", false);
        Opened = false;
    }
}
