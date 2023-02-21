using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulScoreUI : MonoBehaviour
{
    private TextMeshProUGUI soulScoreTextTMP;

    private void Awake()
    {
        soulScoreTextTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Subscribe to the soul amount changed event
        StaticEventHandler.OnSoulAmountChanged += StaticEventHandler_OnSoulAmountChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from the soul amount changed event
        StaticEventHandler.OnSoulAmountChanged -= StaticEventHandler_OnSoulAmountChanged;
    }

    // Handle soul amount changed event
    private void StaticEventHandler_OnSoulAmountChanged(SoulAmountChangedArgs soulAmountChangedArgs)
    {
        // Update UI
        soulScoreTextTMP.text = "SOUL COLLECTED: " + soulAmountChangedArgs.soulAmount.ToString("#,###,##0") + "\nMULTIPLIER: x" + soulAmountChangedArgs.multiplier; // Use the US number seperating format "ToString("#,###,##0")"
    }

}
