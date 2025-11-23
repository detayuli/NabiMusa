using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
using UnityEngine.SceneManagement; 

public class MovementManager : MonoBehaviour
{
    // --- Nama Scene Stage 2 ---
    public string nextSceneName = "Gameplay 1"; 

    // --- Pengaturan Karakter 1 ---
    public GameObject israeliCharacter; 
    public SpriteRenderer characterSpriteRenderer; 
    
    // --- BARU: Pengaturan Karakter 2 ---
    public GameObject secondCharacter; 
    public SpriteRenderer secondCharacterSpriteRenderer; 
    
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
        // Pengecekan semua objek yang diperlukan
        if (israeliCharacter == null || characterSpriteRenderer == null || nextButton == null || nextButtonRect == null ||
            secondCharacter == null || secondCharacterSpriteRenderer == null || globalFadePanel == null) 
        {
             Debug.LogError("Pastikan semua objek sudah di-assign di Inspector! Termasuk kedua Karakter dan Fade Panel.");
             return;
        }
        audiomanager.instance.PlaySFX(audiomanager.instance.BangsaIsraelClip);

        nextButton.onClick.AddListener(OnNextButtonClicked);
        nextStageButton.onClick.AddListener(NextStageTransition);
        nextStageButton.gameObject.SetActive(false); 

        // Inisialisasi Karakter 1 (alpha 0)
        Color charColor1 = characterSpriteRenderer.color;
        charColor1.a = 0f;
        characterSpriteRenderer.color = charColor1;
        
        // BARU: Inisialisasi Karakter 2 (alpha 0)
        Color charColor2 = secondCharacterSpriteRenderer.color;
        charColor2.a = 0f;
        secondCharacterSpriteRenderer.color = charColor2;

        DoGlobalFadeIn();
        FadeInCharacter();
    }

    // --- Fungsi Fade Global ---

    private void DoGlobalFadeIn()
    {
        globalFadePanel.color = new Color(globalFadePanel.color.r, globalFadePanel.color.g, globalFadePanel.color.b, 1f);
        globalFadePanel.raycastTarget = true; 

        globalFadePanel.DOFade(0f, 1f).OnComplete(() =>
        {
            globalFadePanel.raycastTarget = false;
        });
    }

    /// <summary>
    /// Fungsi yang dipanggil saat tombol Next Stage diklik. Memuat Scene baru.
    /// </summary>
    public void NextStageTransition()
    {
        nextStageButton.gameObject.SetActive(false); 
        DoGlobalFadeOutAndLoadScene();
    }

    private void DoGlobalFadeOutAndLoadScene()
    {
        globalFadePanel.raycastTarget = true; 
        
        // Lakukan Fade Out (menjadi hitam penuh) dan muat Scene
        globalFadePanel.DOFade(1f, 1f)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
    }

    // --- Fungsi Movement dan Fade Karakter ---
    
    public void OnNextButtonClicked()
    {
        nextButton.interactable = false; 

        // Fade Out Karakter 1 dan 2 secara bersamaan
        DOTween.Sequence()
            .Join(characterSpriteRenderer.DOFade(0f, characterFadeDuration))
            .Join(secondCharacterSpriteRenderer.DOFade(0f, characterFadeDuration))
            .OnComplete(() =>
            {
                MoveCharacterAndButton();
            });
    }

    private void MoveCharacterAndButton()
    {
        Sequence moveSequence = DOTween.Sequence();
        
        // Pindahkan Karakter 1 ke KIRI
        moveSequence.Join(israeliCharacter.transform.DOLocalMoveX(israeliCharacter.transform.localPosition.x - characterMoveDistance, characterMoveDuration));
        
        // BARU: Pindahkan Karakter 2 ke KIRI
        moveSequence.Join(secondCharacter.transform.DOLocalMoveX(secondCharacter.transform.localPosition.x + characterMoveDistance, characterMoveDuration));
        
        // Pindahkan Tombol Next ke KIRI
        moveSequence.Join(nextButtonRect.DOAnchorPosX(nextButtonRect.anchoredPosition.x - buttonMoveDistance, buttonMoveDuration));
        
        // Aksi setelah movement selesai
        moveSequence.OnComplete(() =>
        {
            FadeInCharacter();

            currentClickCount++;
            audiomanager.instance.PlaySFX(audiomanager.instance.buttonpress2);
            
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
        // Fade In Karakter 1 dan 2 secara bersamaan
        DOTween.Sequence()
            .Join(characterSpriteRenderer.DOFade(1f, characterFadeDuration))
            .Join(secondCharacterSpriteRenderer.DOFade(1f, characterFadeDuration));
    }
}