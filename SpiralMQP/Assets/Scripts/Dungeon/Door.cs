using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Door : MonoBehaviour
{
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    [Tooltip("Create this with the BoxCollision2D component on the DoorCollider gameobjects")]
    [SerializeField] private BoxCollider2D doorCollider;
    [HideInInspector] public bool isBossRoomDoor = false; // We only want the player to access the boss room after they clear other rooms (at least for now)
    private BoxCollider2D doorTrigger;
    private bool isOpen = false;
    private bool previouslyOpened = false; // When player enter a new room, they have to defeat all the enemies before they can exit. This variable tracks that if the room has been searched yet
    private Animator animator;


    private void Awake() 
    {
        // Disable door collider by default
        doorCollider.enabled = false;

        // Load components
        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == Settings.playerTag || other.tag == Settings.playerWeapon)
        {
            OpenDoor();
        }
    }

    private void OnEnable() 
    {
        // When the parent gameobject is disabled (when the player moves far enough away from the room), the animator state gets reset
        // So we need to restore the animator state
        animator.SetBool(Settings.open, isOpen);    
    }


    /// <summary>
    /// Open the door
    /// </summary>
    public void OpenDoor()
    {
        // We only want to open the door when it's closed
        if (!isOpen)
        {
            isOpen = true;
            previouslyOpened = true;
            doorCollider.enabled = false; // When the door is opening, we don't want the collider in the way
            doorTrigger.enabled = false; 

            // Set open parameter in animator
            animator.SetBool(Settings.open, true);
        }
    }

    /// <summary>
    /// Lock the door
    /// </summary>
    public void LockDoor()
    {
        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        // Activate close door animation
        animator.SetBool(Settings.open, false);
    }

    /// <summary>
    /// Unlock the door
    /// </summary>
    public void UnlockDoor()
    {
        doorCollider.enabled = false;
        doorTrigger.enabled = true;

        if (previouslyOpened == true)
        {
            isOpen = false; // Set the door to not isOpen to execute OpenDoor()
            OpenDoor();
        }
    }



    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);
    }
#endif
    #endregion

}
