using Cinemachine;
using UnityEngine;

[DisallowMultipleComponent]
public class Minimap : MonoBehaviour
{
    [Tooltip("Populate with the child MinimapPlayer gameobject")]
    [SerializeField] private GameObject minimapPlayer;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.GetPlayer().transform; // Get player transform

        // Populate player as cinemachine camera target
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        // Set minimap player icon
        SpriteRenderer spriteRenderer = minimapPlayer.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GameManager.Instance.GetPlayerMinimapIcon();
        }
    }

    private void Update()
    {
        // Move the minimap player to follow the player
        if (playerTransform != null && minimapPlayer != null)
        {
            minimapPlayer.transform.position = playerTransform.position;
        }
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(minimapPlayer), minimapPlayer);
    }

#endif

    #endregion Validation

}