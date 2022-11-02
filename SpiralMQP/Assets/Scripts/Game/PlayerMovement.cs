using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Compenets")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera cam;

    [Header("Values")]
    [SerializeField] float speed;
    [SerializeField] Animator playerAnimator;


    private void FixedUpdate()
    {
        rb.position = Vector2.MoveTowards(rb.position, rb.position + GetDir(), speed * Time.fixedDeltaTime);
    }

    Vector2 GetDir()
    {
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        float verticalDirection = Input.GetAxisRaw("Vertical");
        playerAnimator.SetInteger("xdirection", (int)horizontalDirection);
        playerAnimator.SetInteger("ydirection", (int)verticalDirection);
        return new Vector2(horizontalDirection, verticalDirection);
    }
}
