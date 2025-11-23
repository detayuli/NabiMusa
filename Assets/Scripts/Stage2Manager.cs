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
    public Sprite commandSprite; 
    public Sprite killBabySprite;  
    
    // --- Tombol Next Stage ---
    public Button nextStageButton2;       
    
    // --- Global Fade Panel Scene 2 dan Nama Scene 3 ---
    public Image globalFadePanel2; 
    public string nextSceneNameStage3 = "GameScene_Stage3"; 

    // --- Pengaturan Animasi ---
    public float pharaohCommandDuration = 0.2f; 
    
    // --- Variabel Kontrol ---
    private int currentInteractionStep = 0;
    private bool interactionCompleted = false; // FLAG PENTING
    private bool isAnimating = false; 

    // Menggunakan Start() untuk inisialisasi, karena kini skrip ada di GameManager
    void Start()
    {
        InitializeStage2(); 
    }

    private void InitializeStage2() 
    {
        bubbleTextContainer.SetActive(false);
        nextStageButton2.gameObject.SetActive(false);
        
        currentInteractionStep = 0;
        isAnimating = false;
        // JANGAN reset interactionCompleted di sini, biarkan nilainya bertahan antar-Scene jika Anda tidak memuat ulang. 
        // Namun, jika setiap kali Scene dimulai, interaksi harus diulang, maka tetap FALSE.
        interactionCompleted = false; 
        
        nextStageButton2.onClick.RemoveAllListeners();
        // Listener Next Stage tetap harus diatur di Inspector atau di sini jika tidak mau pakai Inspector.
        // nextStageButton2.onClick.AddListener(GoToStage3); 

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

        if (interactionCompleted)
        {
             // Interaksi selesai: Ulangi teks mulai dari step 1
             // Kita kirim flag TRUE untuk menandakan ini adalah "reset" (ulang)
             StartCoroutine(InteractionSequence(true)); 
        }
        else
        {
             // Interaksi urutan normal
             currentInteractionStep++;
             // Kita kirim flag FALSE untuk menandakan ini adalah "run pertama"
             StartCoroutine(InteractionSequence(false));
        }
    }

    private IEnumerator InteractionSequence(bool resetInteraction)
    {
        isAnimating = true; 
        
        if (resetInteraction)
        {
            // Jika ini pengulangan, set kembali ke langkah pertama dialog
            currentInteractionStep = 1;
        }

        // Efek visual klik
        if (pharaohImage != null)
        {
            pharaohImage.rectTransform.DOShakeScale(pharaohCommandDuration, 0.1f, 10);
        }
        
        yield return new WaitForSeconds(pharaohCommandDuration);

        switch (currentInteractionStep)
        {
            case 1:
                bubbleTextContainer.SetActive(true);
                bubbleTextRenderer.sprite = commandSprite;
                
                // FIX: Hanya sembunyikan tombol jika ini adalah run pertama
                if (!resetInteraction) 
                {
                    nextStageButton2.gameObject.SetActive(false); 
                }
                break;

            case 2:
                bubbleTextRenderer.sprite = killBabySprite;
                
                // FIX: Hanya sembunyikan tombol jika ini adalah run pertama
                if (!resetInteraction) 
                {
                    nextStageButton2.gameObject.SetActive(false); 
                }
                break;

            case 3:
                // Interaksi Selesai
                bubbleTextContainer.SetActive(false); 
                
                // Tampilkan Tombol Next Stage (atau pastikan tetap muncul)
                nextStageButton2.gameObject.SetActive(true);
                interactionCompleted = true; 
                
                isAnimating = false;
                yield break; 
            
            default:
                bubbleTextContainer.SetActive(false);
                currentInteractionStep = 0; 
                isAnimating = false;
                break;
        }
        
        isAnimating = false;
    }
    
    // Dipanggil dari Inspector Button Next Stage
    public void GoToStage3() 
    {
        nextStageButton2.gameObject.SetActive(false); 
        
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