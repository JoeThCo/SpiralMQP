using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DealContactDamage : MonoBehaviour
{
    [Space(10)]
    [Header("DEAL DAMAGE")]
    [Tooltip("The contact damage to deal (can be overridden by the receiver)")]
    [SerializeField] private int contactDamageAmount;

    [Tooltip("Specify what layers objects should be on to receive contact damage")]
    [SerializeField] private LayerMask layerMask;

    private bool isColliding = false;

    // Trigger contact damage when enter a collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If already colliding with something return
        if (isColliding) return;

        ContactDamage(collision);
    }

    // Trigger contact damage when staying withing a collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If already colliding with something return
        if (isColliding) return;

        ContactDamage(collision);
    }

    private void ContactDamage(Collider2D collision)
    {
        // If the collision object isn't in the specified layer then return (use bitwise comparison)
        int collisionObjectLayerMask = (1 << collision.gameObject.layer); // collision.gameObject.layer returns the layer number. collisionObjectLayerMask is a decimal value after the bitshift

        if ((layerMask.value & collisionObjectLayerMask) == 0) return; // There is no match. The collision object is on the layer that we don't want contact damage to happen

        // Check to see if the colliding object should take contact damage
        ReceiveContactDamage receiveContactDamage = collision.gameObject.GetComponent<ReceiveContactDamage>();

        if (receiveContactDamage != null)
        {
            isColliding = true;

            // Reset the contact collision after set time
            Invoke(nameof(ResetContactCollision), Settings.contactDamageCollisionResetDelay);

            receiveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    /// <summary>
    /// Reset the isColliding bool
    /// </summary>
    private void ResetContactCollision()
    {
        isColliding = false;
    }



    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(contactDamageAmount), contactDamageAmount, true);
    }
#endif
    #endregion
}
