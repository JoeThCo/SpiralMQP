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

    private void FixedUpdate()
    {
        rb.position = Vector2.MoveTowards(rb.position, rb.position + GetDir(), speed * Time.deltaTime);
    }

    Vector2 GetDir()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
