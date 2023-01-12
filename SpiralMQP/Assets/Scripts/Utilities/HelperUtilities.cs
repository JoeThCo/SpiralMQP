using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{
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
    /// Positive value debug check
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
}
