using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Tooltip("The player WeaponShootPosition gameobject in the hierarchy")]
    [SerializeField] private Transform weaponShootPosition;


    [Tooltip("MovementDetailsSO containing movement details such as speed.")]
    [SerializeField] private MovementDetailsSO movementDetails;


    private Player player;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate; // Dealing with physics we use fixed update (cuz that's how unity does it)
    private bool isPlayerRolling = false;
    private float playerRollCooldownTimer = 0f;


    private void Awake()
    {
        // load components
        player = GetComponent<Player>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        // Create WaitForFixedUpdate for use in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        // Set player animation speed at start
        SetPlayerAnimationSpeed();
    }

    /// <summary>
    /// Set player animation speed to match movement speed
    /// </summary>
    private void SetPlayerAnimationSpeed()
    {
        // Set animation speed to match movement speed
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimations;
    }

    private void Update()
    {
        // If player is rolling then return (player can't perform anything else in a rolling movement)
        if (isPlayerRolling) return;

        // Process the player movement input
        MovementInput();

        // Process the player weapon input
        WeaponInput();

        // Player roll cooldown timer
        PlayerRollCooldownTimer();
    }


    /// <summary>
    /// Reset the cooldown timer if in cooldown
    /// </summary>
    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Player movement input
    /// </summary>
    private void MovementInput()
    {
        // Get movement input (tracks not only the arrows keys but also the WASD)
        float horizontalMovement = Input.GetAxisRaw("Horizontal"); // On keyboard, only return -1(left), 0(no horizontal movement) or 1(right) 
        float verticalMovement = Input.GetAxisRaw("Vertical"); // On keyboard, only return -1(down), 0(no vertical movement) or 1(up)
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1); // 0 - left mouse button, 1 - right mouse button

        // Create a direction vector based on the input
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        // Adjust distance for diagonal movement (pythagoras approximation)
        if (horizontalMovement != 0f && verticalMovement != 0f) // Going in a diagonal direction
        {
            direction *= 0.7f; // By doing this we maintain the distance of 1 in all direction
        }

        // If there is movement either move or roll
        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                // Trigger movement event
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            else if (playerRollCooldownTimer <= 0f) // Right mouse button down and the rolling not in cooldown
            {
                PlayerRoll((Vector3)direction);
            }
        }
        else // Trigger idle event
        {
            player.idleEvent.CallIdleEvent();
        }
    }


    /// <summary>
    /// Player roll
    /// </summary>
    private void PlayerRoll(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction)); // Run the coroutine and store reference for the coroutine
    }


    /// <summary>
    /// Player roll coroutine
    /// </summary>
    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        // minDistance used to decide when to exit the coroutine loop
        float minDistance = 0.2f;

        isPlayerRolling = true; // Yes, I am rolling

        // Calculate target position - the "direction" will be a unit vector
        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;

        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            // Call the movement to position event
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed, direction, isPlayerRolling);

            // Yield and wait for fixed update - to sync with the physics update
            yield return waitForFixedUpdate;
        }

        isPlayerRolling = false;

        // Set cooldown timer
        playerRollCooldownTimer = movementDetails.rollCooldownTime;

        // Send the player to exactly the target position - we might still be about minDistance away from the target
        player.transform.position = targetPosition;
    }


    /// <summary>
    /// Weapon Input
    /// </summary>
    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        // Aim weapon input
        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        // Get mouse world position
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        // Calculate direction vector of mouse cursor from weapon shoot position
        // For example: we have point A(2,2) and B(4,4)
        // B - A = (2,2) which means from point A, it need a dispacement value of (2,2) to get to point B
        // You can think of A represents the "weaponShootPosition.position" and B represents the "mouseWorldPosition"
        weaponDirection = (mouseWorldPosition - weaponShootPosition.position);

        // Similarly, calculate direction vector of mouse cursor from player transform position
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        // Get weapon to cursor angle
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        // Get player to cursor angle
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        // Set player aim direction
        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        // Trigger weapon aim event
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        // If collided with something, stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // If in collision with something, stop player roll coroutine
        // Theoratically only having "OnCollisionEnter2D" should work just fine, but just in case
        StopPlayerRollRoutine();
    }

    private void StopPlayerRollRoutine()
    {
        if (playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);

            isPlayerRolling = false;

            // Set cooldown timer - only if the cooldown time is long, otherwise the rolling feels a bit non-responsive
            // playerRollCooldownTimer = movementDetails.rollCooldownTime;
        }
    }



    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion
}
