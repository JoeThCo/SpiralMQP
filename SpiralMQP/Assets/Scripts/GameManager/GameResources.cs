using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

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
    [Header("MUSIC")]
    [Tooltip("Populate with the music master mixer group")]
    public AudioMixerGroup musicMasterMixerGroup;
    
    [Tooltip("Main menu music scriptable object")]
    public MusicTrackSO mainMenuMusic;

    [Tooltip("music on full snapshot")]
    public AudioMixerSnapshot musicOnFullSnapshot;
   
    [Tooltip("music low snapshot")]
    public AudioMixerSnapshot musicLowSnapshot;
    
    [Tooltip("music off snapshot")]
    public AudioMixerSnapshot musicOffSnapshot;



    [Space(10)]
    [Header("SOUNDS")]
    [Tooltip("Fill with the sounds master mixer group")]
    public AudioMixerGroup soundsMasterMixerGroup;

    [Tooltip("Door open and close sound effect")] // In the future we can sound effect for each event, but for now we are using the same one for both events
    public SoundEffectSO doorOpenCloseSoundEffect;

    [Tooltip("Populate with the table flip sound effect")]
    public SoundEffectSO tableFlip;

    [Tooltip("Populate with the chest open sound effect")]
    public SoundEffectSO chestOpen;

    [Tooltip("Populate with the health pickup sound effect")]
    public SoundEffectSO healthPickup;

    [Tooltip("Populate with the weapon pickup sound effect")]
    public SoundEffectSO weaponPickup;

    [Tooltip("Populate with the ammo pickup sound effect")]
    public SoundEffectSO ammoPickup;



    [Space(10)]
    [Header("MATERIALS")]
    [Tooltip("Dimmed Materials")]
    public Material dimmedMaterial;

    [Tooltip("Sprite-Lit-Default Material")]
    public Material litMaterial;

    [Tooltip("Create with the Variable Lit Shader")]
    public Shader variableLitShader;

    [Tooltip("Create with the Materialize Shader")]
    public Shader materializeShader;



    [Space(10)]
    [Header("SPECIAL TILEMAP TILES")]
    [Tooltip("Collision tiles that the enemies can navigate to")]
    public TileBase[] enemyUnwalkableCollisionTilesArray;

    [Tooltip("Preferred path tile for enemy navigation")]
    public TileBase preferredEnemyPathTile;



    [Space(10)]
    [Header("UI")]
    [Tooltip("Populate with ammo icon prefab")]
    public GameObject ammoIconPrefab;

    [Tooltip("Populate with the green health bar sprite")]
    public Sprite healthBarGreen;

    [Tooltip("Populate with the yellow health bar sprite")]
    public Sprite healthBarYellow;
    
    [Tooltip("Populate with the red health bar sprite")]
    public Sprite healthBarRed;


    [Space(10)]
    [Header("CHESTS")]
    [Tooltip("Chest item prefab")]
    public GameObject chestItemPrefab;

    [Tooltip("Populate with heart icon sprite")]
    public Sprite heartIcon;

    [Tooltip("Populate with bullet icon sprite")]
    public Sprite bulletIcon;



    [Space(10)]
    [Header("MINIMAP")]
    [Tooltip("Minimap Boss Icon Gameobject")]
    public GameObject minimapBossIconPrefab;



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
