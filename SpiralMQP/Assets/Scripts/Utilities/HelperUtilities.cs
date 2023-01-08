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
