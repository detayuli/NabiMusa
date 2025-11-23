using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("--- TARGET ---")]
    public Transform target; // Seret objek Keranjang di sini
    public float smoothSpeed = 0.125f; // Kecepatan follow (semakin kecil, semakin lambat)
    
    private Vector3 offset; // Jarak Kamera ke Keranjang

    void Start()
    {
        if (target != null)
        {
            // Hitung offset awal (mempertahankan jarak kamera dari keranjang)
            offset = transform.position - target.position;
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Hitung posisi target baru (hanya ubah X, Y, Z tetap pada offset)
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, offset.z);
            
            // Gerakkan kamera secara halus ke posisi target
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}