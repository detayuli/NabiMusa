using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audiomanager : MonoBehaviour
{
    public static audiomanager Instance { get; private set; }
    // Backwards-compatible alias for existing code that expects `audiomanager.instance`
    public static audiomanager instance => Instance;

    [Header("---------- Audio Sources ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clips ----------")]
    // Menghapus walkingClip
    public AudioClip buttonpressClip; // Menggantikan buttonpress1/2/3 yang berlebihan

    // Voice Clips (Asumsi ini adalah narasi/dialog)
    public AudioClip BangsaIsraelClip;
    public AudioClip RajaMesirclip;
    public AudioClip Bayiditaruhclip;
    public AudioClip PutriFiraun;
    public AudioClip dinamaiMusa;

    public AudioClip buttonpress1; // Pastikan variabel ini dideklarasikan
    public AudioClip buttonpress2; // Pastikan variabel ini dideklarasikan
    public AudioClip buttonpress3;

    [Header("---------- Scene Music Mapping ----------")]
    [SerializeField] private List<SceneMusicMapping> sceneMusicMappings;

    private Dictionary<string, AudioClip> sceneMusicDict = new Dictionary<string, AudioClip>();
    private bool sfxEnabled = true;


    [System.Serializable]
    public class SceneMusicMapping
    {
        public List<string> sceneNames;
        public AudioClip musicClip;
    }

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var mapping in sceneMusicMappings)
        {
            foreach (var sceneName in mapping.sceneNames)
            {
                sceneMusicDict[sceneName] = mapping.musicClip;
            }
        }

        // Subscribe to scene change events
        SceneManager.activeSceneChanged += OnSceneChanged;

        // Play music for the initial scene
        PlayMusicInCurrentScene();
    }

    public void SetSFXEnabled(bool enabled)
    {
        sfxEnabled = enabled;
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        PlayMusicInCurrentScene(); // Check and play music for the new scene
    }

    private void PlayMusicInCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (sceneMusicDict.TryGetValue(currentScene, out AudioClip targetClip))
        {
            // Cek apakah musik sekarang sudah sama dan sedang dimainkan
            if (musicSource != null && musicSource.clip == targetClip && musicSource.isPlaying)
            {
                // Musik sama dan sudah jalan, gak usah ganti
                return;
            }

            // Ganti lagu dan mainkan
            if (musicSource != null && targetClip != null)
            {
                musicSource.clip = targetClip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            // Tidak ada musik untuk scene ini, stop musik
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null && sfxEnabled)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
    
    // Menghapus fungsi HandleWalking() dan isWalking
    // Menghapus tombol buttonpress1/2/3 yang berlebihan
    
    public void HandleButtonPress()
    {
        PlaySFX(buttonpressClip); 
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
}