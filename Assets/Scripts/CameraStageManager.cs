using UnityEngine;
using DG.Tweening;
using UnityEngine.UI; // Penting: Diperlukan untuk RectTransform

public class CameraStageManager : MonoBehaviour
{
    public Camera mainCamera; 
    public float panDuration = 1.0f;
    public Ease panEase = Ease.InOutQuad; 
    
    // --- TAMBAHAN: Referensi Canvas untuk Sinkronisasi ---
    // Pastikan Canvas Anda diatur ke Render Mode: Screen Space - Camera
    public RectTransform mainCanvasRect; 

    // Tidak perlu fungsi Start jika Anda mengasumsikan mainCamera sudah di-assign

    public void PanCamera(float distanceToPan, System.Action onComplete = null)
    {
        if (mainCamera == null) return;

        // Pindah ke posisi saat ini dikurangi jarak (Geser ke KIRI)
        mainCamera.transform.DOLocalMoveX(mainCamera.transform.localPosition.x - distanceToPan, panDuration)
            .SetEase(panEase)
            .OnComplete(() => 
            {
                // Sinkronkan Canvas setelah kamera selesai bergerak
                SynchronizeCanvasPosition();
                
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// Fungsi untuk menggeser kamera ke BAWAH (-Y direction).
    /// </summary>
    public void PanCameraDown(float distanceToPan, System.Action onComplete = null)
    {
        if (mainCamera == null) return;

        // Pindah ke posisi saat ini dikurangi jarak Y (Geser ke BAWAH)
        mainCamera.transform.DOLocalMoveY(mainCamera.transform.localPosition.y - distanceToPan, panDuration)
            .SetEase(panEase)
            .OnComplete(() => 
            {
                // Sinkronkan Canvas setelah kamera selesai bergerak
                SynchronizeCanvasPosition();
                
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// Menyinkronkan posisi horizontal dan vertikal Canvas agar tetap berada di tengah kamera.
    /// </summary>
    private void SynchronizeCanvasPosition()
    {
        if (mainCanvasRect == null || mainCamera == null)
        {
            Debug.LogWarning("Canvas RectTransform atau Main Camera belum di-assign.");
            return;
        }
        
        // Ambil posisi lokal kamera
        Vector3 cameraLocalPos = mainCamera.transform.localPosition;

        // Set posisi lokal Canvas sama dengan posisi lokal kamera (hanya X dan Y)
        // Z-position harus tetap, karena Z menentukan Plane Distance di Screen Space - Camera
        mainCanvasRect.transform.localPosition = new Vector3(
            cameraLocalPos.x,
            cameraLocalPos.y,
            mainCanvasRect.transform.localPosition.z 
        );

        Debug.Log($"Canvas disinkronkan ke posisi: ({cameraLocalPos.x}, {cameraLocalPos.y})");
    }

    public float CalculateScreenWidth()
    {
        return 2f * mainCamera.orthographicSize * mainCamera.aspect;
    }
    
    public float CalculateScreenHeight()
    {
        // Tinggi = Camera.orthographicSize * 2
        return 2f * mainCamera.orthographicSize;
    }
}