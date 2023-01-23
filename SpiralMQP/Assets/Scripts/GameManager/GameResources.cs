using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This serves as a game resources repository so that if other objects want to have access to a certain resource, they can search here
/// So pretty much this is just a way to centralize any resources that we need to share and make them easily accessible
/// </summary>
public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    // Singleton design
    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                // "Resources.Load" function is only intended for loading assets at runtime
                instance = Resources.Load<GameResources>("GameResources"); // Unity will look for the "Resources" folder to load "GameResources" item
            }
            return instance;
        }
    }

    [Space(10)]
    [Header("DUNGEON")]
    [Tooltip("Create with the dungeon RoomNodeTypeListSO")]
    public RoomNodeTypeListSO roomNodeTypeList;



    [Space(10)]
    [Header("PLAYER")]
    [Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
    public CurrentPlayerSO currentPlayer;



    [Space(10)]
    [Header("MATERIALS")]
    [Tooltip("Dimmed Materials")]
    public Material dimmedMaterial;

    [Tooltip("Sprite-Lit-Default Material")]
    public Material litMaterial;

    [Tooltip("Create with the Variable Lit Shader")]
    public Shader variableLitShader;



    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(roomNodeTypeList), roomNodeTypeList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(currentPlayer), currentPlayer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(dimmedMaterial), dimmedMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(litMaterial), litMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(variableLitShader), variableLitShader);
    }
#endif
    #endregion
}
