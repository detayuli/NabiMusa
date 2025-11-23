using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
using UnityEngine.SceneManagement; // <-- TAMBAHAN: Diperlukan untuk pindah Scene

public class MovementManager : MonoBehaviour
{
    // Variabel baru untuk Camera Manager
    // public CameraStageManager cameraManager; // <-- DIHAPUS karena tidak digunakan lagi untuk transisi ini
    // Jarak pergeseran kamera. Jika 0, script akan mencoba menghitung lebar layar.
    // public float cameraPanDistance = 0f; // <-- DIHAPUS

    // --- BARU: Nama Scene Stage 2 ---
    public string nextSceneName = "Gameplay 1"; // <-- Ganti nama Scene Anda!

    // --- Pengaturan Karakter ---
    public GameObject israeliCharacter; 
    public SpriteRenderer characterSpriteRenderer; 
    public float characterMoveDistance = 2.0f; 
    public float characterMoveDuration = 0.5f; 
    public float characterFadeDuration = 0.3f; 

    // --- Pengaturan UI & Fade ---
    public Button nextButton; 
    public RectTransform nextButtonRect; 
    public float buttonMoveDistance = 2.0f; 
    public float buttonMoveDuration = 0.5f;

    public Button nextStageButton; 
    public Image globalFadePanel; 

    private int currentClickCount = 0;
    
    void Start()
    {
        // ... (Logika pemeriksaan dan setup awal tetap sama) ...
        
        // --- MODIFIKASI: Hapus pengecekan cameraManager ---
        if (israeliCharacter == null || characterSpriteRenderer == null || nextButton == null || nextButtonRect == null)
        {
             Debug.LogError("Pastikan semua objek sudah di-assign di Inspector!");
             return;
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
        nextStageButton.onClick.AddListener(NextStageTransition);
        nextStageButton.gameObject.SetActive(false); 

        Color charColor = characterSpriteRenderer.color;
        charColor.a = 0f;
        characterSpriteRenderer.color = charColor;

        DoGlobalFadeIn();
        FadeInCharacter();
    }
    
    // ... (Fungsi DoGlobalFadeIn, OnNextButtonClicked, MoveCharacterAndButton, FadeInCharacter tetap sama) ...

    /// <summary>
    /// Fungsi yang dipanggil saat tombol Next Stage diklik. Sekarang memuat Scene baru.
    /// </summary>
    public void NextStageTransition()
    {
        nextStageButton.gameObject.SetActive(false); 

        // Lakukan Fade Out Global sebelum pindah Scene
        DoGlobalFadeOutAndLoadScene();
    }

    /// <summary>
    /// Fungsi baru: Lakukan Fade Out (menjadi hitam penuh) dan muat Scene baru.
    /// </summary>
    private void DoGlobalFadeOutAndLoadScene()
    {
        // Matikan input saat fade
        globalFadePanel.raycastTarget = true; 
        
        // Lakukan Fade Out (menjadi hitam penuh)
        globalFadePanel.DOFade(1f, 1f) // Durasi fade 1 detik
            .OnComplete(() =>
            {
                // Setelah layar hitam penuh, pindah ke Scene berikutnya
                SceneManager.LoadScene(nextSceneName);
            });
    }

    
    private void DoGlobalFadeIn()
    {
        globalFadePanel.color = new Color(globalFadePanel.color.r, globalFadePanel.color.g, globalFadePanel.color.b, 1f);
        globalFadePanel.raycastTarget = true; 

        globalFadePanel.DOFade(0f, 1f).OnComplete(() =>
        {
            globalFadePanel.raycastTarget = false;
        });
    }
    
    public void OnNextButtonClicked()
    {
        nextButton.interactable = false; 

        characterSpriteRenderer.DOFade(0f, characterFadeDuration)
            .OnComplete(() =>
            {
                MoveCharacterAndButton();
            });
    }

    private void MoveCharacterAndButton()
    {
        // Pindahkan Karakter ke KIRI
        israeliCharacter.transform.DOLocalMoveX(israeliCharacter.transform.localPosition.x - characterMoveDistance, characterMoveDuration);
        
        // Pindahkan Tombol Next ke KIRI (Menggunakan DOAnchorPosX pada RectTransform)
        nextButtonRect.DOAnchorPosX(nextButtonRect.anchoredPosition.x - buttonMoveDistance, buttonMoveDuration)
            .OnComplete(() =>
            {
                FadeInCharacter();

                currentClickCount++;
                
                if (currentClickCount >= 2) 
                {
                    nextButton.gameObject.SetActive(false); 
                    nextStageButton.gameObject.SetActive(true); 
                }
                else
                {
                    nextButton.interactable = true; 
                }
            });
    }

    private void FadeInCharacter()
    {
        characterSpriteRenderer.DOFade(1f, characterFadeDuration);
    }
}