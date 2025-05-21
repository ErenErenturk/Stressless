using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LLMClient : MonoBehaviour
{
    [Header("LLM Ayarları")]
    public string host = "127.0.0.1";
    public int port = 65432;

    [Header("UI Referansları")]
    public TMP_InputField nameInput;
    public GameObject nameEntryPanel;         // 🔥 Burada eklendi
    public GameObject speechBubble;
    public TMP_Text speechText;

    [Header("Opsiyonel")]
    public bool autoHideBubble = true;
    public float hideDelaySeconds = 5f;

    private string playerId;

    void Start()
    {
        speechBubble.SetActive(false);
    }

    public void OnClickSubmitName()
    {
        if (nameInput == null)
        {
            Debug.LogError("❌ nameInput is NULL!");
            return;
        }

        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("⚠️ İsim boş.");
            return;
        }

        playerId = GenerateIdFromName(playerName);
        nameInput.text = "";
        nameInput.interactable = false;

        // 🔥 name entry paneli kapat
        if (nameEntryPanel != null)
            nameEntryPanel.SetActive(false);

        StartCoroutine(SendIntroduceWithDelay(playerId, playerName));
    }

    public void OnEndEditName(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnClickSubmitName();
        }
    }

    private string GenerateIdFromName(string name)
    {
        return name.ToLower().Replace(" ", "_").Trim();
    }

    private IEnumerator SendIntroduceWithDelay(string id, string name)
    {
        if (speechBubble != null && speechText != null)
        {
            speechBubble.SetActive(true);
            speechText.text = "...";
        }

        yield return new WaitForSeconds(1f);

        string message = $"introduce:{id}|{name}";

        try
        {
            using (TcpClient client = new TcpClient(host, port))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                byte[] responseData = new byte[1024];
                int bytes = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.UTF8.GetString(responseData, 0, bytes);

                Debug.Log("🧠 LLM Response: " + response);

                if (speechText != null)
                    speechText.text = response;

                if (autoHideBubble)
                    StartCoroutine(HideBubbleAfterDelay());
            }
        }
        catch (Exception e)
        {
            Debug.LogError("💥 LLM connection error: " + e.Message);
            if (speechText != null)
                speechText.text = "❌ Bağlantı hatası.";
        }
    }

    private IEnumerator HideBubbleAfterDelay()
    {
        yield return new WaitForSeconds(hideDelaySeconds);
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }

        // 🔥 Sahne geç
        SceneManager.LoadScene("Home");
    }

}
