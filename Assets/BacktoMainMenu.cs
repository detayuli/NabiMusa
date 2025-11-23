using UnityEngine;
using UnityEngine.SceneManagement; // Pastikan ini ada

public class BacktoMainMenu : MonoBehaviour
{
    void Start()
    {
        audiomanager.instance.PlaySFX(audiomanager.instance.dinamaiMusa);
    }
    
    public void BacktoMenu()
    {
        // --- SOLUSI: Reset Time Scale ke Normal (1f) ---
        Time.timeScale = 1f; 
        // ---------------------------------------------
        
        SceneManager.LoadScene("MainMenu");
        audiomanager.instance.PlaySFX(audiomanager.instance.buttonpress1);
    }
}