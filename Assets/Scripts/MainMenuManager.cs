using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private Image fadePanel;
    
    [Header("Settings")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    private bool canStart = false;
    private bool isTransitioning = false;

    void Start()
    {
        // Set initial state
        menuCanvasGroup.alpha = 0;
        fadePanel.color = new Color(0, 0, 0, 1);
        
        // Fade in menu
        FadeInMenu();
    }

    void Update()
    {
        // Check for any input
        if (canStart && !isTransitioning && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    void FadeInMenu()
    {
        // Fade out black panel
        fadePanel.DOFade(0, fadeInDuration).SetEase(Ease.OutQuad);
        
        // Fade in menu
        menuCanvasGroup.DOFade(1, fadeInDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            canStart = true;
        });
    }


    void StartGame()
    {
        isTransitioning = true;
        audiomanager.instance.PlaySFX(audiomanager.instance.buttonpress1); 
        
        
        // Fade out menu
        menuCanvasGroup.DOFade(0, fadeOutDuration * 0.5f).SetEase(Ease.InQuad);
        
        // Fade in black panel
        fadePanel.DOFade(1, fadeOutDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            // Load game scene
            SceneManager.LoadScene(gameSceneName);
        });
    }

    void QuitGame()
    {
        audiomanager.instance.PlaySFX(audiomanager.instance.buttonpress1);
        Application.Quit();
    }
}