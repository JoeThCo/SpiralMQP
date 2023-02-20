using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
[DisallowMultipleComponent]
public class Idle : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private IdleEvent idleEvent;

    private void Awake() 
    {
        // Load components
        rigidBody2D = GetComponent<Rigidbody2D>();
        idleEvent = GetComponent<IdleEvent>();
    }

    private void OnEnable() 
    {
        // Subscribe to idle event
        idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable() 
    {
        // Unsubscribe to idle event
        idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        MoveRigidBody();
    }

    /// <summary>
    /// Move the rigidbody component
    /// </summary>
    private void MoveRigidBody()
    {
        // Ensure the rb collision detection is set to continuous
        rigidBody2D.velocity = Vector2.zero; // Set velocity to zero since we are idling
    }
}
