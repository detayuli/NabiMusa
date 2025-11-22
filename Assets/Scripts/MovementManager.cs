using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
// Pastikan script ini masih dilampirkan ke Game Manager

public class MovementManager : MonoBehaviour
{
    // Variabel baru untuk Camera Manager
    public CameraStageManager cameraManager;
    // Jarak pergeseran kamera. Jika 0, script akan mencoba menghitung lebar layar.
    public float cameraPanDistance = 0f; 

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
        
        if (israeliCharacter == null || characterSpriteRenderer == null || nextButton == null || nextButtonRect == null || cameraManager == null)
        {
            Debug.LogError("Pastikan semua objek sudah di-assign di Inspector, termasuk CameraStageManager!");
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
    /// Fungsi yang dipanggil saat tombol Next Stage diklik. Sekarang melakukan pergeseran kamera.
    /// </summary>
    public void NextStageTransition()
    {
        nextStageButton.gameObject.SetActive(false); 

        // Tentukan jarak pan
        float panDistance = (cameraPanDistance > 0) ? cameraPanDistance : cameraManager.CalculateScreenWidth();

        // Lakukan Pergeseran Kamera
        cameraManager.PanCamera(panDistance, () => 
        {
            Debug.Log("Transisi Stage 1 ke Stage 2 Selesai. Kamera sekarang berada di Stage 2.");
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
                
                if (currentClickCount >= 3) 
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