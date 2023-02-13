using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Environment : MonoBehaviour
{
    // Attach this class to environment game objects whose lighting gets faded in
    [Space(10)]
    [Header("REFERENCES")]
    [Tooltip("Populate with the SpriteRenderer component on the prefab")]
    public SpriteRenderer spriteRenderer;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(spriteRenderer), spriteRenderer);
    }
#endif
    #endregion Validation

}
