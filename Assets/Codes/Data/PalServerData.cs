using System;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PalServerData
{
    public string PalServerPath = "";
    public Process currentProcess = null; // 현재 실행 중인 프로세스를 추적하기 위한 필드
    public string PalServerExcutePath
    {
        get
        {
            return PalServerPath + "/Pal/Binaries/Win64/PalServer-Win64-Test-Cmd.exe";
        }
    }
    public string PalServerConfigPath
    {
        get
        {
            return PalServerPath + "/Pal/Saved/Config/WindowsServer/PalWorldSettings.ini";
        }
    }

    public List<PalServerValue> palServerValues;
    public PalServerData()
    {
        palServerValues = new List<PalServerValue>();
    }
    public void KillProcess() {
        if (currentProcess != null && !currentProcess.HasExited)
        {
            currentProcess.Kill();
            currentProcess = null;
            UnityEngine.Debug.Log("프로세스가 종료되었습니다.");
        }
    }

    public void ExecuteOrKillConfigFile()
    {
        if (currentProcess != null && !currentProcess.HasExited)
        {
            currentProcess.Kill();
            currentProcess = null;
            UnityEngine.Debug.Log("프로세스가 종료되었습니다.");
        }
        else
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = PalServerExcutePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                currentProcess = new Process() { StartInfo = startInfo };
                currentProcess.Start();

                UnityEngine.Debug.Log("프로세스가 시작되었습니다.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("프로세스 실행 중 오류 발생: " + ex.Message);
            }
        }
    }
}