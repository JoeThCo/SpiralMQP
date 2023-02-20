using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;


    [Tooltip("Create with the CursorTarget gameobject")]
    [SerializeField] private Transform cursorTarget;

    private void Awake() 
    {
        // Load components
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();    
    }

    private void Start() 
    {
        SetCinemachineTargetGroup();
    }

    /// <summary>
    /// Set the cinemachine camera target group
    /// </summary>
    private void SetCinemachineTargetGroup()
    {
        // Create target group for cinemachine for the cinemachine camera to follow
        // Track the player as well as the cursor
        CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target {weight = 1f, radius = 2.5f, target = GameManager.Instance.GetPlayer().transform};

        CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target {weight = 1f, radius = 1f, target = cursorTarget};

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] {cinemachineGroupTarget_player, cinemachineGroupTarget_cursor};

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray; // Update the list with the list that has the current player object and the cursor (When start, follow the current player and cursor)
    }

    private void Update() 
    {
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition(); // Keep up with the mouse world position
    }
}
