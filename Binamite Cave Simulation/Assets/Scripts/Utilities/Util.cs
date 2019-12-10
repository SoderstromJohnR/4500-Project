using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for static methods used thruought the project. At the moment,
/// it contains only a method that takes a List<int> and returns a string with
/// the elements seperated by commas.
/// </summary>
public class Util
{
    /* Returns a string composed of the results of ToString called on each element in a list
     * seperated by commas. */
   public static string intListToString (List<int> intList)
    {
        string intString = ""; // Return value

        // Adds the first element to the string
        if (intList.Count > 0)
        {
            intString = intList[0].ToString();
        }

        // Adds each subsequent element preceeded by a comma
        for (int i = 1; i < intList.Count; i++)
        {
            intString += ", " + intList[i].ToString();
        }

        return intString;
    }
}
