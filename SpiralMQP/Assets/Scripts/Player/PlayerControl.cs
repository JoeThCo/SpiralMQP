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


    private void Awake()
    {
        // load components
        player = GetComponent<Player>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Update()
    {
        // Process the player movement input
        MovementInput();

        // Process the player weapon input
        WeaponInput();
    }

    /// <summary>
    /// Player movement input
    /// </summary>
    private void MovementInput()
    {
        // Get movement input (tracks not only the arrows keys but also the WASD)
        float horizontalMovement = Input.GetAxisRaw("Horizontal"); // On keyboard, only return -1(left), 0(no horizontal movement) or 1(right) 
        float verticalMovement = Input.GetAxisRaw("Vertical"); // On keyboard, only return -1(down), 0(no vertical movement) or 1(up)

        // Create a direction vector based on the input
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        // Adjust distance for diagonal movement (pythagoras approximation)
        if (horizontalMovement != 0f && verticalMovement !=0f) // Going in a diagonal direction
        {
            direction *= 0.7f; // By doing this we maintain the distance of 1 in all direction
        }

        // If there is movement
        if (direction != Vector2.zero)
        {
            // Trigger movement event
            player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
        }
        else // Trigger idle event
        {
            player.idleEvent.CallIdleEvent();
        }
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


    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
       HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion
}
