using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPlaying = true;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static bool IsInLayerMask(GameObject obj, LayerMask checkLayers)
    {
        return ((checkLayers.value & (1 << obj.layer)) > 0);
    }
}
