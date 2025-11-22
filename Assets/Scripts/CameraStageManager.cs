using UnityEngine;
using DG.Tweening;

public class CameraStageManager : MonoBehaviour
{
    public Camera mainCamera; 
    public float panDuration = 1.0f;
    public Ease panEase = Ease.InOutQuad; 

    // ... (Fungsi Start dan PanCamera tetap sama) ...

    public void PanCamera(float distanceToPan, System.Action onComplete = null)
    {
        // ... (Fungsi PanCamera (ke Kiri) tetap sama) ...
        if (mainCamera == null) return;

        // Pindah ke posisi saat ini dikurangi jarak (Geser ke KIRI)
        mainCamera.transform.DOLocalMoveX(mainCamera.transform.localPosition.x - distanceToPan, panDuration)
            .SetEase(panEase)
            .OnComplete(() => 
            {
                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// Fungsi BARU: Menggeser kamera ke BAWAH (-Y direction).
    /// </summary>
    public void PanCameraDown(float distanceToPan, System.Action onComplete = null)
    {
        if (mainCamera == null) return;

        // Pindah ke posisi saat ini dikurangi jarak Y (Geser ke BAWAH)
        mainCamera.transform.DOLocalMoveY(mainCamera.transform.localPosition.y - distanceToPan, panDuration)
            .SetEase(panEase)
            .OnComplete(() => 
            {
                onComplete?.Invoke();
            });
    }

    public float CalculateScreenWidth()
    {
        return 2f * mainCamera.orthographicSize * mainCamera.aspect;
    }
    
    /// <summary>
    /// Fungsi BARU: Menghitung tinggi layar (ukuran kamera * 2).
    /// </summary>
    public float CalculateScreenHeight()
    {
        // Tinggi = Camera.orthographicSize * 2
        return 2f * mainCamera.orthographicSize;
    }
}