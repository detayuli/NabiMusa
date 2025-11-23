using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 
using TMPro; 

public class BasketRunnerManager : MonoBehaviour
{
    // --- Pengaturan Keranjang ---
    public float moveSpeed = 5f;        
    public float laneHeight = 1.5f;     // Diperbaiki: Nilai yang lebih kecil (1.5f) untuk jalur yang rapat
    
    // --- Pengaturan Lane (Jalur) ---
    public int maxLane = 1;             
    public int minLane = -1;            
    public float yOffset = -3.0f;       // <-- TAMBAHAN: Offset dasar untuk menggeser jalur ke bawah sungai
                                        // Pastikan nilai ini SAMA dengan ObstacleSpawner
    
    private int currentLane = 0; 
    private Vector3 targetPosition;
    
    // --- Pengaturan Game ---
    public float totalTime = 30f;       
    private float timeRemaining;
    private bool isGameOver = true;     // <-- UBAH: Set True di Start agar menunggu StartGame()
    private bool isAnimating = false; 

    // --- Referensi UI/Lainnya ---
    public GameObject gameOverPanel;
    public TextMeshProUGUI timerText; 
    public ObstacleSpawner obstacleSpawner; 


    void Start()
    {
        // Posisi Z direset ke Z=0
        targetPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        
        // PENTING: Panggil StartGame() di Start() untuk kasus Restart Scene
        StartGame(); 
        audiomanager.instance.PlaySFX(audiomanager.instance.Bayiditaruhclip);
    }
    
    // FUNGSI INI DIPANGGIL OLEH TRANSITIONER / RESTART
    public void StartGame()
    {
        // 1. Reset Status Game
        timeRemaining = totalTime;
        isGameOver = false; // <-- AKTIFKAN GAME DI SINI
        isAnimating = false;
        currentLane = 0; // Mulai di jalur tengah
        
        // 2. Set Posisi Fisik Keranjang ke Jalur Tengah Sungai
        float gameStartY = currentLane * laneHeight + yOffset;
        // Asumsi posisi X Keranjang adalah -8 (titik start gameplay)
        transform.position = new Vector3(-4f, gameStartY, 0f); 
        targetPosition = transform.position;

        // 3. Reset UI
        if (gameOverPanel != null)
        {
            // PENTING: Time.timeScale harus diset ke 1f saat restart/start
            Time.timeScale = 1f; 
            gameOverPanel.SetActive(false);
        }
        
        // 4. Mulai Spawner
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StartSpawning(); 
        }
        
        Debug.Log("Game Basket Diinisialisasi Ulang & Dimulai!");
    }


    void Update()
    {
        if (isGameOver) return;
        
        MoveBasket();
        UpdateTimer();
    }
    
    private void MoveBasket()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        if (transform.position == targetPosition)
        {
            isAnimating = false;
        }
    }
    
    // FUNGSI INPUT: MoveUp/MoveDown
    public void MoveUp()
    {
        if (isGameOver || isAnimating) return; 
        int newLane = Mathf.Clamp(currentLane + 1, minLane, maxLane);
        SetTargetLane(newLane);
    }

    public void MoveDown()
    {
        if (isGameOver || isAnimating) return;
        int newLane = Mathf.Clamp(currentLane - 1, minLane, maxLane);
        SetTargetLane(newLane);
    }
    
    private void SetTargetLane(int newLane)
    {
        if (newLane != currentLane)
        {
            currentLane = newLane;
            isAnimating = true; 
            
            // Hitung posisi Y baru berdasarkan laneHeight dan yOffset
            float newY = (currentLane * laneHeight) + yOffset;
            targetPosition = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        // -------------------------------------
        
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
        if (isGameOver) return; 
        isGameOver = true;
        
        // --- TAMBAHAN PENTING: HENTIKAN SEMUA SUARA ---
        if (audiomanager.instance != null)
        {
            // Asumsi AudioManager punya fungsi StopAllAudio() atau sejenisnya
            audiomanager.instance.StopAllSound(); 
        }
        // ---------------------------------------------
        
        Time.timeScale = 0f; // Hentikan waktu permainan (setelah audio dihentikan)
        Debug.Log("GAME OVER!");
        
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
        if (isGameOver) return; 
        isGameOver = true;
        Time.timeScale = 0f; 
        Debug.Log("GAME WIN! Waktu bertahan tercapai.");
        SceneManager.LoadScene("Gameplay 3");
        
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StopSpawning(); 
        }
    }
    
    // Tambahkan fungsi RestartGame di sini
    public void RestartGame()
    {
        Time.timeScale = 1f; // PENTING: Setel kembali waktu sebelum memuat scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}