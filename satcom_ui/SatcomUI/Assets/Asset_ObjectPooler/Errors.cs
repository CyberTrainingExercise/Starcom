using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Errors
 *  desc:
 *  manages all the errors for the objectpooler system
 **/
public static class Errors {

    //hashtable of ints (error code), key is string (error name)
    private static Hashtable codeMap = new Hashtable();
    //list of errors
    private static List<string> errors = new List<string>();

    private static void InitErrors()
    {
        errors.Add("Pool out of objects! Consider enabling recycleObjects or increasing maxObjects");
        codeMap.Add("OutOfObjects", 1);

        errors.Add("Pool does not contain definition for id: ");
        codeMap.Add("PoolNullDefinition", 2);

        errors.Add("Pool not on object with manager!");
        codeMap.Add("PoolNotOnManager", 3);

        errors.Add("Cannot check visibility with no renderer attached to: ");
        codeMap.Add("NoRenderer", 4);

        errors.Add("IO Error: ");
        codeMap.Add("IOError", 5);

        errors.Add("Object pooler does not contain definitiion for pool id:");
        codeMap.Add("NullDefinition", 6);

    }
    public static void LogError (string errorCode, string extraInfo)
    {
        LogError((int)codeMap[errorCode], extraInfo);
    }
    public static void LogError (string errorCode)
    {
        LogError((int)codeMap[errorCode], null);
    }
    public static void LogError(int errorCode)
    {
        LogError(errorCode, null);
    }
    public static void LogError (int errorCode, string extraInfo)
    {
        if (errors.Count == 0) InitErrors();
        if (extraInfo != null)
        {
            Debug.LogError("Error (Code " + errorCode + "): " + errors[errorCode] + "!");
        } else
        {
            Debug.LogError("Error (Code " + errorCode + "): " + errors[errorCode] + " : " + extraInfo + "!");
        }
    }
}
