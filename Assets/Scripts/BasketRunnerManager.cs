using UnityEngine;
using UnityEngine.SceneManagement; // Untuk Game Over/Restart
using UnityEngine.UI; // <-- TAMBAHAN: Walaupun tidak wajib, baik untuk kejelasan UI

public class BasketRunnerManager : MonoBehaviour
{
    // --- Pengaturan Keranjang ---
    public float moveSpeed = 5f;        // Kecepatan gerakan vertikal
    public float laneHeight = 2.5f;     // Jarak antar jalur (misal: 2.5 unit Unity)
    
    // --- Pengaturan Lane (Jalur) ---
    // Tambahkan Max dan Min Lane untuk mempermudah penambahan jalur
    public int maxLane = 1;             // Indeks Jalur paling atas (misal: 1)
    public int minLane = -1;            // Indeks Jalur paling bawah (misal: -1)

    // Posisi Y saat ini (0 = tengah, 1 = atas, -1 = bawah)
    private int currentLane = 0; 
    private Vector3 targetPosition;
    
    // --- Pengaturan Game ---
    public float totalTime = 30f;       // Waktu bertahan yang dibutuhkan (30 detik)
    private float timeRemaining;
    private bool isGameOver = false;
    private bool isAnimating = false; // Flag tambahan untuk kontrol gerakan

    // --- Referensi UI/Lainnya ---
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI timerText; 
    public ObstacleSpawner obstacleSpawner; 


    void Start()
    {
        // Posisi awal keranjang adalah di jalur tengah
        targetPosition = transform.position;
        timeRemaining = totalTime;
        isGameOver = false;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Asumsi: ObstacleSpawner di Start() game-nya
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StartSpawning();
        }
    }

    void Update()
    {
        if (isGameOver) return;
        
        // HandleInput(); // <-- HILANGKAN! Input sekarang ditangani oleh fungsi public MoveUp/MoveDown
        MoveBasket();
        UpdateTimer();
    }
    
    // HILANGKAN FUNGSI HandleInput() yang lama

    private void MoveBasket()
    {
        // Gerakkan keranjang ke posisi target (smoothed movement)
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        // Cek apakah sudah sampai di target, jika ya, matikan flag animasi
        if (transform.position == targetPosition)
        {
            isAnimating = false;
        }
    }
    
    // ---------------------------------------------
    // FUNGSI BARU: DIPANGGIL OLEH TOMBOL UI ATAS
    // ---------------------------------------------
    public void MoveUp()
    {
        // Cegah input jika game over atau sedang dalam proses animasi
        if (isGameOver || isAnimating) return; 

        int newLane = Mathf.Clamp(currentLane + 1, minLane, maxLane);
        SetTargetLane(newLane);
    }

    // ---------------------------------------------
    // FUNGSI BARU: DIPANGGIL OLEH TOMBOL UI BAWAH
    // ---------------------------------------------
    public void MoveDown()
    {
        // Cegah input jika game over atau sedang dalam proses animasi
        if (isGameOver || isAnimating) return;

        int newLane = Mathf.Clamp(currentLane - 1, minLane, maxLane);
        SetTargetLane(newLane);
    }
    
    private void SetTargetLane(int newLane)
    {
        if (newLane != currentLane)
        {
            currentLane = newLane;
            isAnimating = true; // Aktifkan flag saat gerakan dimulai
            
            // Hitung posisi Y baru
            float newY = currentLane * laneHeight;
            targetPosition = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    // ... (Sisa fungsi UpdateTimer, OnCollisionEnter2D, GameOver, GameWin, RestartGame tetap sama) ...
    
    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        
        // Perbarui UI
        if (timerText != null)
        {
            timerText.text = "Waktu: " + Mathf.Max(0, timeRemaining).ToString("F1");
        }

        if (timeRemaining <= 0)
        {
            GameWin();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Hentikan waktu permainan
        
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StopSpawning(); 
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    private void GameWin()
    {
        isGameOver = true;
        
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StopSpawning(); 
        }
        // ... (Lanjutkan ke Stage 4)
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}