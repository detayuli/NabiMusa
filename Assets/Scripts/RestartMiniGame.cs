using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMiniGame : MonoBehaviour
{
    public void RestartGame()
    {
        // Mendapatkan indeks (nomor urut) dari scene yang sedang aktif
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Memuat ulang scene tersebut
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f; // Pastikan waktu permainan berjalan normal setelah restart`
    }
}