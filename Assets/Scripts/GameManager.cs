using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private Process serverProcess;

    void Start()
    {
        StartCoroutine(StartLLMServer());
    }

    IEnumerator StartLLMServer()
    {
        string exePath = @"C:\Users\EBRAR\OneDrive\Masaüstü\Stressless\LLMServer\server.exe";

        if (!File.Exists(exePath))
        {
            UnityEngine.Debug.LogError("❌ server.exe bulunamadı: " + exePath);
            yield break;
        }

        serverProcess = new Process();
        serverProcess.StartInfo.FileName = exePath;
        serverProcess.StartInfo.CreateNoWindow = true;
        serverProcess.StartInfo.UseShellExecute = false;
        serverProcess.StartInfo.RedirectStandardOutput = true;
        serverProcess.StartInfo.RedirectStandardError = true;

        serverProcess.OutputDataReceived += (s, e) => UnityEngine.Debug.Log("[LLM Server] " + e.Data);
        serverProcess.ErrorDataReceived += (s, e) => UnityEngine.Debug.LogError("[LLM Server Error] " + e.Data);

        try
        {
            serverProcess.Start();
            serverProcess.BeginOutputReadLine();
            serverProcess.BeginErrorReadLine();
            UnityEngine.Debug.Log("✅ server.exe başlatıldı");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("💥 server.exe başlatılamadı: " + e.Message);
        }

        yield return null;
    }

    void OnApplicationQuit()
    {
        if (serverProcess != null && !serverProcess.HasExited)
        {
            serverProcess.Kill();
            UnityEngine.Debug.Log("🛑 server.exe kapatıldı");
        }
    }

    // 🎮 Play butonuna bastığında sadece sahne geç
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Dede");
    }
}
