using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject interactionUI;      // "E'ye bas" yazısı
    public GameObject dialogPanel;        // Konuşma kutusu paneli
    public Text dialogText;               // Panel içindeki metin

    [Header("Oyuncu")]
    public Transform playerTransform;     // Oyuncunun Transform'u

    private bool playerInRange = false;
    private bool dialogOpen = false;

    void Start()
    {
        interactionUI.SetActive(false);
        dialogPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogOpen)
            {
                dialogPanel.SetActive(true);
                dialogText.text = "Merhaba, yardım eder misin?";
                dialogOpen = true;
            }
            else
            {
                dialogPanel.SetActive(false);
                dialogOpen = false;
            }
        }

        if (dialogOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            dialogPanel.SetActive(false);
            dialogOpen = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float playerScaleX = playerTransform.localScale.x;
            float npcPosX = transform.position.x;
            float playerPosX = playerTransform.position.x;

            if ((playerScaleX > 0 && playerPosX < npcPosX) || (playerScaleX < 0 && playerPosX > npcPosX))
            {
                interactionUI.SetActive(true);
                playerInRange = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
            dialogPanel.SetActive(false);
            playerInRange = false;
            dialogOpen = false;
        }
    }
}
