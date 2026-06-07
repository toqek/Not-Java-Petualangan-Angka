using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Bobbing (Efek Berlari) Settings")]
    public float bobSpeed = 8f;       // Kecepatan naik turun (makin besar makin cepat)
    public float bobAmount = 0.01f;     // Tinggi naik turun (makin besar makin tinggi jaraknya)
    
    [Header("Shake (Tabrakan) Settings")]
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.15f;
    private float dampingSpeed = 1.0f;

    private Vector3 startLocalPosition;
    private float timer = 0f;

    void Start()
    {
        // Menyimpan posisi lokal awal kamera relatif terhadap parent-nya
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // 1. LOGIKA BOBBING (NAIK TURUN SEPERTI BERLARI)
        // Menggunakan Mathf.Sin untuk membuat gerakan naik turun bergelombang yang halus
        timer += Time.deltaTime * bobSpeed;
        float newY = startLocalPosition.y + Mathf.Sin(timer) * bobAmount;
        
        // Terapkan posisi Y yang baru ke posisi lokal kamera
        Vector3 targetLocalPos = new Vector3(startLocalPosition.x, newY, startLocalPosition.z);

        // 2. LOGIKA SHAKE (JIKA PERLU GETARAN SAAT MENABRAK)
        if (shakeDuration > 0)
        {
            // Tambahkan getaran acak pada posisi lokal kamera
            targetLocalPos += Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }

        // Eksekusi perubahan posisi ke kamera
        transform.localPosition = targetLocalPos;
    }

    // Panggil fungsi ini dari script Player/Bola saat menabrak gerbang matematika
    public void TriggerShake(float duration = 0.2f, float magnitude = 0.2f)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}