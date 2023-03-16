using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DoorLightingControl : MonoBehaviour
{
    private bool isLit = false; // Room liting condition
    private Door door;

    private void Awake() 
    {
        // Get components
        door = GetComponentInParent<Door>();
    }

    // Fade door in if triggered
    private void OnTriggerEnter2D(Collider2D other) 
    {
        FadeInDoor(door);
    }

    /// <summary>
    /// Fade in door
    /// </summary>
    public void FadeInDoor(Door door)
    {
        // Create new material to fade in
        Material material = new Material(GameResources.Instance.variableLitShader);

        if (!isLit)
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInParent<SpriteRenderer>(); // We have two sprite renderer in EW door

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                StartCoroutine(FadeInDoorRoutine(spriteRenderer, material));
            }

            isLit = true;
        }
    }


    /// <summary>
    /// Fade in door coroutine
    /// </summary>
    private IEnumerator FadeInDoorRoutine(SpriteRenderer spriteRenderer, Material material)
    {
        spriteRenderer.material = material;

        for (float i = 0f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        spriteRenderer.material = GameResources.Instance.litMaterial;
    }
}
