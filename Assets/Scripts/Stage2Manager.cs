using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Stage2Manager : MonoBehaviour
{
    // --- Aset Raja dan Sprite ---
    public SpriteRenderer pharaohSpriteRenderer;
    public Sprite pharaohCommandSprite; // Sprite Raja saat menunjuk/menyuruh
    public Sprite pharaohIdleSprite;    // Sprite Raja saat diam/awal

    // --- Bubble Text (Sprite Renderer) ---
    public GameObject bubbleTextContainer; // Container untuk semua bubble text
    public SpriteRenderer bubbleTextRenderer; // Renderer untuk menampilkan bubble text
    public Sprite commandSprite; // Sprite Bubble Text 1: "Tindas Bangsa Israel!"
    public Sprite killBabySprite;  // Sprite Bubble Text 2: "Bunuh para anak bayi laki-laki!"
    
    // --- Gulungan (Scroll Object) ---
    public GameObject scrollObject;     // Gulungan (harus memiliki Collider2D)
    public float scrollOpenDuration = 0.5f;

    // --- Tombol Next Stage ---
    public Button nextStageButton2;      // Tombol untuk pindah ke Stage 3

    // --- Pengaturan Animasi ---
    public float pharaohCommandDuration = 0.2f; // Durasi animasi tunjuk

    private int currentInteractionStep = 0;
    private bool isInteracting = false; // Flag untuk mencegah klik ganda
    
    // Dipanggil saat Stage 2 dimulai (setelah kamera pan)
    public void InitializeStage2(CameraStageManager camManager)
    {
        // Pastikan container text/bubble tersembunyi
        bubbleTextContainer.SetActive(false);
        nextStageButton2.gameObject.SetActive(false);
        pharaohSpriteRenderer.sprite = pharaohIdleSprite;

        // Reset step
        currentInteractionStep = 0;
        isInteracting = false;
        
        // Tambahkan listener untuk Next Stage Button (memanggil fungsi transisi kamera ke bawah)
        nextStageButton2.onClick.RemoveAllListeners();
        nextStageButton2.onClick.AddListener(() => GoToStage3(camManager));

        // Langkah awal: Raja marah (tanpa teks di awal)
        Debug.Log("Stage 2 dimulai: Raja Mesir marah.");
    }
    
    // Fungsi bawaan Unity untuk mendeteksi klik pada objek yang memiliki Collider2D
    private void OnMouseDown()
    {
        // Pastikan objek yang diklik adalah gulungan
        if (gameObject == scrollObject && !isInteracting)
        {
            isInteracting = true;
            StartCoroutine(InteractionSequence());
        }
    }

    private IEnumerator InteractionSequence()
    {
        currentInteractionStep++;
        
        // Animasi Gulungan/Raja: selalu terjadi saat Scroll diklik
        scrollObject.transform.DOShakeScale(scrollOpenDuration, 0.1f, 10); // Efek getar gulungan saat dibuka
        pharaohSpriteRenderer.sprite = pharaohCommandSprite; // Raja Menunjuk

        yield return new WaitForSeconds(pharaohCommandDuration);

        // Tampilkan teks perintah
        switch (currentInteractionStep)
        {
            case 1:
                // Teks 1: Menindas Bangsa Israel
                bubbleTextContainer.SetActive(true);
                bubbleTextRenderer.sprite = commandSprite;
                break;

            case 2:
                // Teks 2: Bunuh Bayi Laki-laki
                bubbleTextRenderer.sprite = killBabySprite;
                // Tidak perlu sembunyikan container karena kita hanya ganti sprite
                break;

            case 3:
                // Interaksi Selesai
                bubbleTextContainer.SetActive(false);
                pharaohSpriteRenderer.sprite = pharaohIdleSprite; // Kembali ke sprite Idle
                
                // Tampilkan Tombol Next Stage
                nextStageButton2.gameObject.SetActive(true);
                isInteracting = false;
                yield break; // Interaksi selesai
            
            default:
                // Jika klik berlebihan, kembali ke Idle dan nonaktifkan interaksi
                pharaohSpriteRenderer.sprite = pharaohIdleSprite;
                isInteracting = false;
                yield break;
        }
        
        // Kembali ke sprite Idle setelah perintah ditampilkan
        pharaohSpriteRenderer.sprite = pharaohIdleSprite;
        isInteracting = false; // Izinkan klik lagi
    }
    
    // Fungsi untuk pindah ke Stage 3 (Memanggil transisi ke bawah)
    public void GoToStage3(CameraStageManager camManager)
    {
        nextStageButton2.gameObject.SetActive(false); 
        
        // Tentukan jarak pan (menggunakan tinggi layar)
        float panDistanceY = camManager.CalculateScreenHeight(); // Fungsi baru yang akan kita tambahkan

        // Matikan elemen visual Stage 2
        gameObject.SetActive(false); 

        // Lakukan Pergeseran Kamera ke BAWAH
        camManager.PanCameraDown(panDistanceY, () => 
        {
            Debug.Log("Transisi Stage 2 ke Stage 3 Selesai. Kamera sekarang berada di Stage 3.");
            // Di sini Anda akan mengaktifkan Stage 3 Manager.
        });
    }
}