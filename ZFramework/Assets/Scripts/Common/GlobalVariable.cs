using UnityEngine;

public class GlobalVariable {
    public static string AssetBundlePath {
        get {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return Application.absoluteURL;
            else
                return System.Environment.CurrentDirectory;
        }
    }
    
}
