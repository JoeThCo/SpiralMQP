using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Compenents")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] Rigidbody2D playerRigidbody;

    [Header("Dodge")]
    public bool CanDodge = true;
    public KeyCode DodgeKey = KeyCode.Space;
    [Range(10,15)]
    public int DodgePower;
    [Range(0.2f,0.35f)]
    public float DodgeTime;
    public float DodgeCoolDownTime;
    public ParticleSystem DodgeParticles;

    [Header("Movement")]
    public bool CanMove = true;
    
    // used for melee animation to distinguish current facing direciton
    // 0: left   1: right
    private int direction = 1;

    [Range(3.5f, 50.0f)]
    public float MovementSpeed;

    [Header("Shield")]
    public bool CanShield = true;
    public KeyCode ShieldKey = KeyCode.Q;
    public GameObject Shield;
    public float ShieldCoolDownTime;
    public Vector2 inputDir;
    private SpriteRenderer Sprite;

    private void Awake()
    {
        SetDodgeParticles(false);
        SetShield(false);
        Sprite = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
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
        PlayerAnimatorController(inputDir);

        if (CanDodge && Input.GetKeyDown(DodgeKey) && inputDir != Vector2.zero)
        {
            StartCoroutine(DodgeI());
        }

        if (CanShield)
        {
            if (Input.GetKeyDown(ShieldKey))
            {
                SetShield(true);
            }

            if (Input.GetKeyUp(ShieldKey))
            {
                SetShield(false);
                StartCoroutine(ShieldI());
            }
        }
    }

    void MovePlayer()
    {
        playerRigidbody.position = Vector2.MoveTowards(playerRigidbody.position, playerRigidbody.position + inputDir, MovementSpeed * Time.deltaTime);
        UpdateSortingOrder();
    }

    void SetShield(bool state)
    {
        Shield.SetActive(state);
        CanMove = !state;
    }

    void SetDodgeParticles(bool state)
    {
        var emission = DodgeParticles.emission;
        emission.enabled = state;
    }

    IEnumerator ShieldI()
    {
        CanShield = false;

        yield return new WaitForSeconds(ShieldCoolDownTime);

        CanShield = true;
    }

    IEnumerator DodgeI()
    {
        CanDodge = false;
        CanMove = false;
        SetDodgeParticles(true);
        //can be danaged = false

        playerRigidbody.AddForce(inputDir * DodgePower, ForceMode2D.Impulse);
        gameObject.GetComponent<RangedAttack>().OnDash = true;
        yield return new WaitForSeconds(DodgeTime);
        UpdateSortingOrder();
        //can be danaged = true
        SetDodgeParticles(false);
        CanMove = true;
        playerRigidbody.velocity = Vector2.zero;
        gameObject.GetComponent<RangedAttack>().OnDash = false;
        yield return new WaitForSeconds(DodgeCoolDownTime);

        CanDodge = true;
    }

    void PlayerAnimatorController(Vector2 dir)
    {
        if (CanMove)
        {
            
             if (dir.x > 0)
            {
                direction = 1; // set character facing direction to right
            }
            if (dir.x < 0)
            {
                direction = 0; // set character facing direction to left
            }
            playerAnimator.SetInteger("xdirection", (int)Math.Round(dir.x));
            playerAnimator.SetInteger("ydirection", (int)Math.Round(dir.y));
            playerAnimator.SetInteger("direction", direction);
        }
        else
        {
            playerAnimator.SetInteger("xdirection", 0);
            playerAnimator.SetInteger("ydirection", 0);
        }
    }

    Vector2 GetDir()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dir.Normalize();
        return dir;
    }

    void UpdateSortingOrder(){
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y*100.0f);
        }
    }
}
