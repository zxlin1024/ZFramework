using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class IniHanler
{
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public static string GetProfile(string section, string key) {
        return GetProfile("myconfig.ini", section, key);
    }

    public static string GetProfile(string fileName, string section, string key) {
        StringBuilder strCom = new StringBuilder(255);
        GetPrivateProfileString(section, key, "", strCom, 255, Application.streamingAssetsPath + "/" + fileName);
        return strCom.ToString();
    }

    private void Test() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            StringBuilder strCom = new StringBuilder(255);
            GetPrivateProfileString("Time", "time", "", strCom, 255, Application.streamingAssetsPath + "/config.ini");
            Debug.Log(strCom.ToString());
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            //WritePrivateProfileString("Time", "time", "5", Application.streamingAssetsPath + "/config.ini");
        }
    }
}
