using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileOperate {

    private IEnumerator Start() {
        //string json = JsonUtility.ToJson(new MyConfig() { DianXunJianUrl = "http://106.15.42.249:8989/mms-rdp/pointInspecPlanController.do?list#" }, true);
        //FileWrite(Application.streamingAssetsPath, "myconfig.txt", json);
        yield return new WaitForSeconds(0.1f);
        string json01 = FileRead(Application.streamingAssetsPath, "myconfig.txt");
        //Debug.Log(json01);
        //MyConfig._Instance = JsonUtility.FromJson<MyConfig>(json01);
    }

    public static string FileRead(string path, string fileName) {
        StreamReader sr;
        path = string.IsNullOrEmpty(path) ? "" : "/" + path;
        FileInfo file = new FileInfo(Application.streamingAssetsPath + path + "/" + fileName);
        if (file.Exists) {
            sr = file.OpenText();
            //sr = File.OpenText(Application.streamingAssetsPath + path + "/" + fileName);
            string content = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return content;
        } else {
            //throw new Exception("文件不存在");
            return "";
        }
    }

    public static void FileWrite(string path, string fileName, string fileContent) {
        StreamWriter sw;
        path = Application.streamingAssetsPath+ (string.IsNullOrEmpty(path) ? "" : "/" + path);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        FileInfo file = new FileInfo(path + "/" + fileName);
        sw = file.CreateText();
        sw.WriteLine(fileContent);
        sw.Dispose();
        sw.Close();
    }

    public static void FileWriteAdd(string path, string fileName, string fileContent) {
        StreamWriter sw;
        path = Application.streamingAssetsPath + (string.IsNullOrEmpty(path) ? "" : "/" + path);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        FileInfo file = new FileInfo(path + "/" + fileName);
        if (file.Exists)
            sw = file.AppendText();
        else
            sw = file.CreateText();
        sw.WriteLine(fileContent);
        sw.Dispose();
        sw.Close();
    }

    public static void WriteLog(string fileContent) {
        StreamWriter sw;
        string path = Application.streamingAssetsPath + "/Log";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string filePath = path + "/" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
            sw = file.AppendText();
        else
            sw = file.CreateText();
        sw.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + fileContent);
        sw.Dispose();
        sw.Close();
    }

    public static void FileDelete(string path)
    {
        File.Delete(path);
    }

    private const string RegName = "BIM3D";
    string[] arguments;
    string GetPath() {
        //把程序路径转换成注册表中路径，用“\”替换“/”，否则访问不到
        string[] path1 = Application.dataPath.Split('/');
        List<string> pathL2 = new List<string>(path1);
        pathL2.RemoveAt(path1.Length - 1);
        string path = string.Join("\\", pathL2.ToArray()) + "\\" + RegName;

        //程序使用参数启动，获取参数值
        arguments = Environment.GetCommandLineArgs();
        return path;
    }
    //写注册表，使程序可以从网页中启动
    public void UpdateProtocal() {
        try {
            if (Registry.ClassesRoot.OpenSubKey(RegName) != null) {
                RegistryKey rg = Registry.ClassesRoot.OpenSubKey(RegName + "\\shell\\open\\command", true);
                string
                    oldUrl = rg.GetValue("").ToString(),
                    newUrl = string.Format("\"{0}\" \"%1\"", GetPath());

                if (!oldUrl.Equals(newUrl, System.StringComparison.CurrentCultureIgnoreCase)) {
                    rg.SetValue("", newUrl);
                }
            } else {
                RegistryKey first = Registry.ClassesRoot.CreateSubKey(RegName);
                first.SetValue("", RegName + " Protocol");
                first.SetValue("URL Protocol", "");
                RegistryKey
                    shell = first.CreateSubKey("shell"),
                    open = shell.CreateSubKey("open"),
                    cmd = open.CreateSubKey("command");
                cmd.SetValue("", string.Format("\"{0}\" \"%1\"", GetPath()));
            }
        } catch (Exception ex) {
            throw ex;
        }
    }
}