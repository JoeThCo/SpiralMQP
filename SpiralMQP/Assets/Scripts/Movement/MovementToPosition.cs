using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementToPositionEvent))]
[DisallowMultipleComponent]
public class MovementToPosition : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private MovementToPositionEvent movementToPositionEvent;

    private void Awake() 
    {
        // Load components
        rigidBody2D = GetComponent<Rigidbody2D>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
    }

    private void OnEnable() 
    {
        // Subscribe to movement to position event
        movementToPositionEvent.OnMovementToPosition += movementToPositionEvent_OnMovementToPosition;
    }

    private void OnDisable() 
    {
        // Unsubscribe from movement to position event
        movementToPositionEvent.OnMovementToPosition -= movementToPositionEvent_OnMovementToPosition;
    }

    // On movement event
    private void movementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        MoveRigidBody(movementToPositionArgs.movePosition, movementToPositionArgs.currentPosition, movementToPositionArgs.moveSpeed);
    }


    /// <summary>
    /// Move the rigidbody component
    /// </summary>
    private void MoveRigidBody(Vector3 movePosition, Vector3 currentPosition, float moveSpeed)
    {
        // Get the unit vector of the direction vector
        Vector2 unitVector = Vector3.Normalize(movePosition - currentPosition);

        // Move the rigid body to end position
        // end position = current rigidbody position + (direction * speed * time)
        // This will be called multiple times in PlayerControl until it reaches the end position
        rigidBody2D.MovePosition(rigidBody2D.position + (unitVector * moveSpeed * Time.fixedDeltaTime));
    }
}
