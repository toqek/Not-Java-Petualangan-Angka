using UnityEngine;

public class Bola : MonoBehaviour
{
    [Header("Pengaturan Visual Berputar")]
    public float kecepatanRotasiGelinding = 500f; // Kecepatan putar utama ke depan

    [Header("Pengaturan Kemiringan (Tilt) Diagonal")]
    public float sudutMiringMaksimum = 25f;       // Seberapa miring bola saat swipe (derajat)
    public float kecepatanTransisiMiring = 8f;    // Seberapa cepat bola merespons kemiringan

    [Header("Efek Partikel & Trail (Tanpa Sampah Objek)")]

    private PlayerController playerController;
    private TrailRenderer trailRenderer;
    private float posisiXTerakhir;
    private float kemiringanZTarget = 0f;
    private float kemiringanZAktual = 0f;

    void Start()
    {
        // 1. Mencari komponen PlayerController yang ada di objek induk (Parent)
        playerController = GetComponentInParent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("Script Bola harus ditaruh di anak objek yang memiliki PlayerController!");
        }
        else
        {
            posisiXTerakhir = playerController.transform.position.x;
        }

        // 2. Cari komponen Trail Renderer yang menempel pada objek visual bola ini
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; 
        }

   
    }

    void Update()
    {
        // Jika bola berhenti (game over/salah jawab)
        if (playerController == null || !playerController.BisaJalan)
        {
            // Matikan trail dan partikel secara instan agar tidak menyembur terus saat diam
            if (trailRenderer != null) trailRenderer.enabled = false;
          
            return;
        }

        // 1. ROTASI UTAMA (GELINDING KEDEPAN SEARAH SUMBU Z)
        transform.Rotate(Vector3.down * kecepatanRotasiGelinding * Time.deltaTime, Space.Self);

        // 2. AKTIFKAN EFEK VISUAL SAAT BERJALAN
        if (trailRenderer != null && !trailRenderer.enabled)
        {
            trailRenderer.enabled = true;
        }

     

        // 3. LOGIKA KEMIRINGAN DIAGONAL (TILT) SAAT SWIPE JALUR
        float posisiXSekarang = playerController.transform.position.x;
        float deltaX = posisiXSekarang - posisiXTerakhir;

        if (Mathf.Abs(deltaX) > 0.001f)
        {
            float arahGerak = deltaX > 0 ? -1f : 1f;
            kemiringanZTarget = arahGerak * sudutMiringMaksimum;
        }
        else
        {
            kemiringanZTarget = 0f;
        }

        kemiringanZAktual = Mathf.Lerp(kemiringanZAktual, kemiringanZTarget, kecepatanTransisiMiring * Time.deltaTime);

        Vector3 rotasiLokalSekarang = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(rotasiLokalSekarang.x, rotasiLokalSekarang.y, kemiringanZAktual);

        posisiXTerakhir = posisiXSekarang;
    }
}