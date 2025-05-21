using UnityEngine;

public class PlayButtonHandler : MonoBehaviour
{
    public GameObject nameEntryPanel;   // UI'daki Panel (başta gizli olacak)

    public void OnPlayClicked()
    {
        Debug.Log("✅ Play'e tıklandı!");

        if (nameEntryPanel != null)
        {
            nameEntryPanel.SetActive(true);
            Debug.Log("📦 NameEntryPanel aktif hale getirildi.");
        }
        else
        {
            Debug.LogError("❌ nameEntryPanel bağlantısı eksik!");
        }
    }

}
