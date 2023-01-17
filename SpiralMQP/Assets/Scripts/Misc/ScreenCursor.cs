using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    private void Awake() 
    {
        // Set the default hardware cursor off
        Cursor.visible = false;
    }

    private void Update() 
    {
        // Since this component will sit on the UI element, the transform would be a rect transform
        // Which is the same as screen position in pixel coordinates
        // And that's why we can assign the mouse position to the object without any conversion
        transform.position = Input.mousePosition;    
    }
}
