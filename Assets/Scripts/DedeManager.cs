using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DedeManager : MonoBehaviour
{
    [Header("Dede Ayarları")]
    public Transform dede;
    public Vector2 targetPosition = new Vector2(0, 0);
    public float moveSpeed = 2f;

    [Header("UI")]
    public GameObject nameEntryPanel;
    public TMP_InputField nameInputField;
    public GameObject speechBubble;
    public TMP_Text speechText;
    public Image screenFadeOverlay;

    [Header("Zamanlama")]
    public float messageDelay = 1f;
    public float hideDelaySeconds = 4f;

    private bool reached = false;
    private float fadeAlpha = 0f;
    private bool nameSubmitted = false;
    private bool fading = false;

    void Start()
    {
        dede.position = new Vector3(19f, -5f, 0f);
        dede.GetComponent<SpriteRenderer>().flipX = true;
        nameEntryPanel.SetActive(false);
        speechBubble.SetActive(false);
    }

    void Update()
    {
        if (!reached)
        {
            dede.position = Vector2.MoveTowards(dede.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(dede.position, targetPosition) < 0.01f)
            {
                reached = true;
                fading = true;
            }
        }

        if (fading)
        {
            fadeAlpha += Time.deltaTime;
            fadeAlpha = Mathf.Clamp01(fadeAlpha);

            screenFadeOverlay.color = new Color(0, 0, 0, fadeAlpha);

            if (fadeAlpha >= 0.5f)
            {
                fading = false;
                nameEntryPanel.SetActive(true);
                nameInputField.Select();
                nameInputField.ActivateInputField();
            }
        }

        if (!nameSubmitted && nameEntryPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            string enteredName = nameInputField.text.Trim();
            if (!string.IsNullOrEmpty(enteredName))
            {
                nameSubmitted = true;
                nameEntryPanel.SetActive(false);
                StartCoroutine(ShowLocalWelcomeMessage(enteredName));
            }
        }
    }

    private IEnumerator ShowLocalWelcomeMessage(string name)
    {
        Debug.Log("🧠 ShowLocalWelcomeMessage ÇALIŞTI");

        if (speechBubble != null)
            speechBubble.SetActive(true);

        if (speechText != null)
            speechText.text = "...";

        yield return new WaitForSeconds(messageDelay);

        if (speechText != null)
            speechText.text = $"Hoş geldin {name}!";

        // ✅ Enter ile sahne geçişi tetikleme başlıyor
        float timer = 0f;
        while (timer < hideDelaySeconds)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Home");
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Otomatik geçiş (Enter basılmadıysa)
        SceneManager.LoadScene("Home");
    }



}
