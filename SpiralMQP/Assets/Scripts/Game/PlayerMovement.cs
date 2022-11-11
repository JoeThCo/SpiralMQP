using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Range(3.5f, 5.0f)]
    public float MovementSpeed;

    [Header("Shield")]
    public bool CanShield = true;
    public KeyCode ShieldKey = KeyCode.Q;
    public GameObject Shield;
    public float ShieldCoolDownTime;

    Vector2 inputDir;

    private void Awake()
    {
        SetDodgeParticles(false);
        SetShield(false);
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

        if (Input.GetKeyDown(DodgeKey) && CanDodge && inputDir != Vector2.zero)
        {
            StartCoroutine(DodgeI());
        }

        if (CanShield)
        {
            if (Input.GetKeyDown(ShieldKey) && CanDodge && CanMove)
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
        //can be damaged = false

        playerRigidbody.AddForce(inputDir * DodgePower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(DodgeTime);

        //can be damaged = true
        SetDodgeParticles(false);
        CanMove = true;
        playerRigidbody.velocity = Vector2.zero;

        SetShield(Input.GetKey(ShieldKey));

        yield return new WaitForSeconds(DodgeCoolDownTime);
        CanDodge = true;
    }

    void PlayerAnimatorController(Vector2 dir)
    {
        if (CanMove)
        {
            playerAnimator.SetInteger("xdirection", (int)dir.x);
            playerAnimator.SetInteger("ydirection", (int)dir.y);
        }
        else
        {
            playerAnimator.SetInteger("xdirection", 0);
            playerAnimator.SetInteger("ydirection", 0);
        }
    }

    Vector2 GetDir()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
