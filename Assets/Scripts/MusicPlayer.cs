using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;
    private AudioSource audioSource;
    private bool isMuted = false;

    [Header("Müzik Klipleri")]
    public AudioClip mainMenuMusic;
    public AudioClip dedeMusic;
    public AudioClip homeMusic;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("🎵 AudioSource bileşeni eksik!");
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        string sceneName = scene.name;
        audioSource.mute = isMuted;

        if (sceneName == "MainMenu")
        {
            SwitchTrack(mainMenuMusic, 0.7f);
        }
        else if (sceneName == "Dede")
        {
            SwitchTrack(dedeMusic, 0.3f);
        }
        else if (sceneName == "Home")
        {
            SwitchTrack(homeMusic, 0.5f); // İstediğin ses seviyesi
        }
        else
        {
            audioSource.Pause();
        }
    }

    void SwitchTrack(AudioClip newClip, float volume)
    {
        if (audioSource.clip == newClip)
        {
            audioSource.volume = volume;
            if (!audioSource.isPlaying && !isMuted)
                audioSource.UnPause();
            return;
        }

        audioSource.clip = newClip;
        audioSource.volume = volume;

        // 🎯 Sahneye göre loop ayarla
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Dede")
            audioSource.loop = false;
        else
            audioSource.loop = true;

        if (!isMuted)
            audioSource.Play();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMuted = !isMuted;
            audioSource.mute = isMuted;
            Debug.Log(isMuted ? "🔇 Müzik kapatıldı" : "🔊 Müzik açıldı");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
