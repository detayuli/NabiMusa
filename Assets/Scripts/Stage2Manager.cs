using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement; 

public class Stage2Manager : MonoBehaviour
{
    // --- Aset Raja (Sesuai Inspector) ---
    public Image pharaohImage;         
    public Button pharaohButton;        
    
    // --- Bubble Text (Sprite Renderer) ---
    public GameObject bubbleTextContainer; 
    public SpriteRenderer bubbleTextRenderer; 
    
    // TIGA SPRITE DIALOG YANG DIGUNAKAN
    public Sprite commandSprite;      // -> Step 1: Tindas Israel
    public Sprite killBabySprite;     // -> Step 2: Bunuh Bayi Laki-laki
    public Sprite crossBabySprite;    // -> Step 3: Tanda Silang/Larangan
    
    // --- Tombol Next Stage ---
    public Button nextStageButton2;       
    
    // --- Global Fade Panel Scene 2 dan Nama Scene 3 ---
    public Image globalFadePanel2; 
    public string nextSceneNameStage3 = "GameScene_Stage3"; 

    // --- Pengaturan Animasi ---
    public float pharaohCommandDuration = 0.2f; 
    
    // --- Variabel Kontrol ---
    private int currentInteractionStep = 0;
    private bool interactionCompleted = false; // Flag ini kini hanya menandai kapan dialog PERTAMA kali selesai
    private bool isAnimating = false; 

    void Start()
    {
        InitializeStage2(); 
    }

    private void InitializeStage2() 
    {
        // Pastikan kontainer dialog disembunyikan di awal
        if(bubbleTextContainer != null) bubbleTextContainer.SetActive(false);
        
        // --- PERUBAHAN: Tombol Next Stage diaktifkan permanen setelah fade in ---
        if(nextStageButton2 != null) nextStageButton2.gameObject.SetActive(false); 
        
        currentInteractionStep = 0;
        isAnimating = false;
        interactionCompleted = false; 
        
        if (nextStageButton2 != null)
        {
            nextStageButton2.onClick.RemoveAllListeners();
            nextStageButton2.onClick.AddListener(GoToStage3); 
        }

        DoScene2FadeIn(); 

        Debug.Log("Stage 2 dimulai: Raja Mesir siap memberi perintah.");
    }
    
    private void DoScene2FadeIn()
    {
        if (globalFadePanel2 != null)
        {
            globalFadePanel2.color = new Color(0, 0, 0, 1f); 
            globalFadePanel2.raycastTarget = true; 

            globalFadePanel2.DOFade(0f, 1f).OnComplete(() =>
            {
                globalFadePanel2.raycastTarget = false;
                
                // PERUBAHAN: Aktifkan tombol Next Stage segera setelah fade in
                if(nextStageButton2 != null) nextStageButton2.gameObject.SetActive(true); 
            });
        }
    }
    
    // Dipanggil dari Inspector Button RajaMesir
    public void OnPharaohClicked()
    {
        if (isAnimating) 
        {
            return;
        }

        // --- LOGIKA LOOPING BARU ---
        
        // 1. Naikkan step
        currentInteractionStep++;

        // 2. Jika step melebihi 3, reset ke 1
        if (currentInteractionStep > 3)
        {
            currentInteractionStep = 1;
        }

        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        isAnimating = true; 
        
        // Efek visual klik
        if (pharaohImage != null)
        {
            pharaohImage.rectTransform.DOShakeScale(pharaohCommandDuration, 0.1f, 10);
        }
        
        yield return new WaitForSeconds(pharaohCommandDuration);

        // Tombol Next Stage TIDAK diubah statusnya di sini
        
        if(bubbleTextContainer != null) bubbleTextContainer.SetActive(true);

        switch (currentInteractionStep)
        {
            case 1:
                // Langkah 1: Tindas Israel
                if(bubbleTextRenderer != null) bubbleTextRenderer.sprite = commandSprite;
                Debug.Log("Dialog: Step 1 (Tindas Israel).");
                break;

            case 2:
                // Langkah 2: Bunuh Bayi Laki-laki
                if(bubbleTextRenderer != null) bubbleTextRenderer.sprite = killBabySprite;
                Debug.Log("Dialog: Step 2 (Bunuh Bayi).");
                break;
                
            case 3:
                // Langkah 3: Tanda Silang/Larangan
                if(bubbleTextRenderer != null) bubbleTextRenderer.sprite = crossBabySprite; 
                Debug.Log("Dialog: Step 3 (Silang/Larangan).");
                // Flag interaksi selesai hanya untuk menandai setidaknya satu loop penuh
                interactionCompleted = true; 
                break;
            
            default:
                // Ini seharusnya tidak pernah tercapai karena logika reset di OnPharaohClicked
                if(bubbleTextContainer != null) bubbleTextContainer.SetActive(false);
                break;
        }
        
        isAnimating = false;
    }

    // Fungsi ini tidak diperlukan lagi karena Raja Firaun boleh diklik terus
    /*
    private void DisablePharaohButton()
    {
        if (pharaohButton != null)
        {
            pharaohButton.interactable = false;
        }
    }
    */
    
    // Dipanggil dari Inspector Button Next Stage
    public void GoToStage3() 
    {
        // Menyembunyikan tombol saat diklik, lalu melakukan fade dan pindah scene
        if(nextStageButton2 != null) nextStageButton2.gameObject.SetActive(false); 
        
        if (globalFadePanel2 != null)
        {
            globalFadePanel2.raycastTarget = true; 
            globalFadePanel2.DOFade(1f, 1f) 
                .OnComplete(() =>
                {
                    SceneManager.LoadScene(nextSceneNameStage3);
                });
        }
        else
        {
            SceneManager.LoadScene(nextSceneNameStage3);
        }
    }
}