using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Compenents")]
    [SerializeField] Rigidbody2D playerRigidbody;

    [Header("Dodge")]
    public bool CanDodge;
    public KeyCode DodgeKey = KeyCode.Space;
    public int DodgePower;
    public float DodgeTime;
    public ParticleSystem DodgeParticles;

    [Header("Movement")]
    public float MovementSpeed;
    public bool CanMove = true;

    Vector2 inputDir;

    private void Awake()
    {
        SetDodgeParticles(false);
    }

    private void FixedUpdate()
    {
        inputDir = GetDir();

        if (CanMove)
        {
            MovePlayer();
        }
    }

    private void Update()
    {
        if (CanDodge && Input.GetKeyDown(DodgeKey) && inputDir != Vector2.zero)
        {
            StartCoroutine(DodgeI());
        }
    }

    void MovePlayer()
    {
        playerRigidbody.position = Vector2.MoveTowards(playerRigidbody.position, playerRigidbody.position + inputDir, MovementSpeed * Time.deltaTime);
    }

    void SetDodgeParticles(bool state)
    {
        var emission = DodgeParticles.emission;
        emission.enabled = state;
    }

    IEnumerator DodgeI()
    {
        CanDodge = false;
        CanMove = false;
        //can be danaged = false

        SetDodgeParticles(true);

        playerRigidbody.AddForce(inputDir * DodgePower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(DodgeTime);

        //can be danaged = true

        SetDodgeParticles(false);
        CanMove = true;
        CanDodge = true;

        playerRigidbody.velocity = Vector2.zero;
    }

    Vector2 GetDir()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
