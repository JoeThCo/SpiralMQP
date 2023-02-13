using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Space(10)]
    [Header("GAMEOBJECT REFERENCES")]
    [Tooltip("Populate with the child Bar gameobject ")]
    [SerializeField] private GameObject healthBar;

    /// <summary>
    /// Enable/Disable the health bar
    /// </summary>
    public void EnableHealthBar(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }

    /// <summary>
    /// Set health bar value with health percent between 0 and 1
    /// </summary>
    public void SetHealthBarValue(float healthPercent)
    {
        healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}