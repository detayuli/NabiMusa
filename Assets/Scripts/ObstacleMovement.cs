using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 5f; // Kecepatan (diset oleh Spawner)

    void Update()
    {
        // Gerakkan obstacle secara konstan ke kiri
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}