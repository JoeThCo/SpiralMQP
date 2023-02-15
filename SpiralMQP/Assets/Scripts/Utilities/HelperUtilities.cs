using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{
    // Hold reference for the main camera in the scene
    public static Camera mainCamera;

    /// <summary>
    /// Get the mouse world position
    /// </summary>
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mouseScreenPosition = Input.mousePosition; // Get the screen position of the mouse

        // Clamp mouse position to screen size
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);

        // Using the Camera built-in function to convert the screen position to world position and set the Z-axis to 0
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        worldPosition.z = 0f;

        return worldPosition;
    }


    /// <summary>
    /// Get the camera viewport lower and upper bounds
    /// </summary>
    public static void CameraWorldPositionBounds(out Vector2Int cameraWorldPositionLowerBounds, out Vector2Int cameraWorldPositionUpperBounds, Camera camera)
    {
        Vector3 worldPositionViewportBottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)); // Get the camera bottom left world position
        Vector3 worldPositionViewportTopRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)); // Get the camera top right world position

        cameraWorldPositionLowerBounds = new Vector2Int((int)worldPositionViewportBottomLeft.x, (int)worldPositionViewportBottomLeft.y);
        cameraWorldPositionUpperBounds = new Vector2Int((int)worldPositionViewportTopRight.x, (int)worldPositionViewportTopRight.y);
    }


    /// <summary>
    /// Get the angle in degrees from a direction vector
    /// Say we have a vector (x,y) 
    /// The angle between the vector and its x will be - arc tan(y/x) 
    /// </summary>
    public static float GetAngleFromVector(Vector3 vector)
    {
        // This will return the angle in radians
        float radians = Mathf.Atan2(vector.y, vector.x);

        // Convert to degrees
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }


    /// <summary>
    /// Get the direction vector from an angle in degrees
    /// </summary>
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f); // Mathf.Cos and Mathf.Sin take input in radians
        return directionVector;
    }


    /// <summary>
    /// Get AimDirection enum value from the passed in angleDegrees
    /// </summary>
    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDirection;

        // --- Set player direction ---
        // Up Right (covers 46 degrees of area)
        if (angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDirection = AimDirection.UpRight;
        }

        // Up (covers 45 degrees of area)
        else if (angleDegrees > 67f && angleDegrees <= 112f)
        {
            aimDirection = AimDirection.Up;
        }

        // Up Left (covers 46 degrees of area)
        else if (angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDirection = AimDirection.UpLeft;
        }

        // Left (covers 67 degrees of area)
        else if ((angleDegrees > 158f && angleDegrees <= 180f) || (angleDegrees > -180f && angleDegrees <= -135f))
        {
            aimDirection = AimDirection.Left;
        }

        // Down (covers 90 degrees of area)
        else if (angleDegrees > -135f && angleDegrees <= -45f)
        {
            aimDirection = AimDirection.Down;
        }

        // Right (covers 66 degrees of area)
        else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0f && angleDegrees < 22f))
        {
            aimDirection = AimDirection.Right;
        }

        // if we somehow messed up, just aim right
        else
        {
            aimDirection = AimDirection.Right;
        }

        return aimDirection;
    }

    /// <summary>
    /// Convert the linear volume scale to decibels
    /// </summary>
    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f; // This is the referenced amplitude level

        // Formula to convert from the linear scale to the logarithmic decibel scale
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }


    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object obj, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object " + obj.name.ToString());
            return true;
        }
        return false;
    }


    /// <summary>
    /// Null value debug log check
    /// </summary>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
            return true;
        }

        return false;
    }


    /// <summary>
    /// Positive value debug check - for the integer value
    /// </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }

        return error;
    }

    /// <summary>
    /// Positive value debug check - for the float value
    /// </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object " + thisObject.name.ToString());
                error = true;
            }
        }

        return error;
    }

    /// <summary>
    /// Positive range debug check - set isZeroAllowed to true if the min and max range values can both be zero.
    /// </summary>
    /// <returns>true if there is an error</returns>
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMin, float valueToCheckMin, string fieldNameMax, float valueToCheckMax, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMin > valueToCheckMax)
        {
            Debug.Log(fieldNameMin + " must be less than or equal to " + fieldNameMax + " in object " + thisObject.name.ToString());
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMin, valueToCheckMin, isZeroAllowed)) error = true; // Check if the min value is positive
        if (ValidateCheckPositiveValue(thisObject, fieldNameMax, valueToCheckMax, isZeroAllowed)) error = true; // Check if the max value is positive

        return error;
    }


    /// <summary>
    /// positive range debug check - set isZeroAllowed to true if the min and max range values can both be zero. Returns true if there is an error
    /// </summary>
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, int valueToCheckMinimum, string fieldNameMaximum,int valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be less than or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;

        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;
    }


    /// <summary>
    /// List empty or contains null value check - returns true if there is a error
    /// </summary>
    public static bool ValidateCheckEnumerableValues(Object obj, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        // Check for null case
        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " + obj.name.ToString());
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {
            // In SO lists, if you delete a value, it retains a null value in the inspector, so this check is necessary
            if (item == null)
            {
                Debug.Log(fieldName + " has null values in object " + obj.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + obj.name.ToString());
            error = true;
        }

        return error;
    }


    /// <summary>
    /// Get the nearest spawn position to player
    /// </summary>
    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f); // Set the initial value to a large vector and in the comparison process, the value will be updated with smaller vectors

        // Loop through room spawn positions
        foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            // Convert the spawn grid positions to world positions
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                nearestSpawnPosition = spawnPositionWorld;
            }
        }

        return nearestSpawnPosition;
    }
}
