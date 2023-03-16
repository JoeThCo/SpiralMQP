using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(InstantiatedRoom))]
[DisallowMultipleComponent]
public class RoomLightingControl : MonoBehaviour
{
    private InstantiatedRoom instantiatedRoom;

    private void Awake()
    {
        // Load components
        instantiatedRoom = GetComponent<InstantiatedRoom>();
    }

    private void OnEnable()
    {
        // Subscribe to room changed event
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        // If this is the room entered and the room isn't already lit, then fade in the room lighting
        if (roomChangedEventArgs.room == instantiatedRoom.room && !instantiatedRoom.room.isLit)
        {
            // Fade in room
            FadeInRoomLighting();

            // Ensure room environment decoration game objects are activated
            instantiatedRoom.ActivateEnvironmentGameObjects(true);

            // Fade in the environment decoration gameobject lighting
            FadeInEnvironmentLighting();

            // Fade in the room doors lighting
            FadeInDoors();

            instantiatedRoom.room.isLit = true;
        }
    }

    /// <summary>
    /// Fade in the room lighting
    /// </summary>
    private void FadeInRoomLighting()
    {
        // Fade in the lighting for the room tilemaps
        StartCoroutine(FadeInRoomLightingRoutine(instantiatedRoom));
    }

    /// <summary>
    /// Fade in the room lighting coroutine
    /// </summary>
    private IEnumerator FadeInRoomLightingRoutine(InstantiatedRoom instantiatedRoom)
    {
        // Create new material to fade in
        Material material = new Material(GameResources.Instance.variableLitShader);

        instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = material;

        for (float i = 0f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        // Set material back to lit material
        instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;

    }


    /// <summary>
    /// Fade in the environmental decoration game objects
    /// </summary>
    private void FadeInEnvironmentLighting()
    {
        // Create new material to fade in
        Material material = new Material(GameResources.Instance.variableLitShader);

        // Get all environment components in room
        Environment[] environmentComponents = GetComponentsInChildren<Environment>();

        // Loop through
        foreach (Environment environmentComponent in environmentComponents)
        {
            if (environmentComponent.spriteRenderer != null) environmentComponent.spriteRenderer.material = material;
        }

        StartCoroutine(FadeInEnvironmentLightingRoutine(material, environmentComponents));
    }


    /// <summary>
    /// Fade in the environmental decoration game objects coroutine
    /// </summary>
    private IEnumerator FadeInEnvironmentLightingRoutine(Material material, Environment[] environmentComponents)
    {
        // Gradually fade in the lighting
        for (float i = 0f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        // Set environment components material back to lit material
        foreach (Environment environmentComponent in environmentComponents)
        {
            if (environmentComponent.spriteRenderer != null) environmentComponent.spriteRenderer.material = GameResources.Instance.litMaterial;
        }
    }


    /// <summary>
    /// Fade in the doors
    /// </summary>
    private void FadeInDoors()
    {
        Door[] doors = GetComponentsInChildren<Door>();

        foreach (Door door in doors)
        {
            DoorLightingControl doorLightingControl = door.GetComponentInChildren<DoorLightingControl>(); // Get reference for the lighting controller

            doorLightingControl.FadeInDoor(door); // Fade in the door
        }
    }

}
