using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExceptionHandler : MonoSingleton<ExceptionHandler> 
{
    private void Start() {
        Application.logMessageReceived += Handler;
    }

    protected override void OnDestroy() {
        Application.logMessageReceived -= Handler;
        base.OnDestroy();
    }

    void Handler(string logString, string stackTrace, LogType type) {
        if(type == LogType.Error || type == LogType.Exception || type == LogType.Assert) {
            FileOperate.FileWriteAdd("Log", "log.log", logString);
        }
    }

    public void Init() { }
}
