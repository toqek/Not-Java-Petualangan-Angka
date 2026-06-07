using UnityEngine;

public class Koin : MonoBehaviour
{
    [Header("Pengaturan Gerakan Animasi")]
    public float kecepatanRotasi = 120f;    // Kecepatan putaran koin
    public float kecepatanNaikTurun = 3f;  // Kecepatan melayang (Sumbu Y)
    public float tinggiNaikTurun = 0.15f;   // Jarak maksimum melayang (sedikit saja)

    // ========================================================
    // TAMBAHAN: Variabel untuk menampung Prefab Efek Partikel
    // ========================================================
    [Header("Efek Partikel Saat Diambil")]
    public GameObject prefabPartikelKoin;  // Taruh prefab Particle System Anda di sini via Inspector
    public float durasiPartikel = 0.6f;    // Waktu (detik) sebelum objek partikel dihancurkan otomatis

    private float posisiYAwal;
    private GameManager gameManager; // Menggunakan camelCase standar C# agar lebih rapi

    void Start()
    {
        // 1. Catat posisi Y awal saat koin di-spawn sebagai patokan melayang
        posisiYAwal = transform.position.y;

        // 2. Cari komponen GameManager yang ada di Scene secara otomatis
        this.gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        // --- 1. ANIMASI KOIN ---
        // Memutar koin secara kontinu pada sumbu Z lokal (Sangat cocok dengan rotasi awal 90X)
        transform.Rotate(Vector3.forward * kecepatanRotasi * Time.deltaTime, Space.Self);

        // Menggerakkan koin naik-turun secara halus menggunakan rumus Sinus
        float newY = posisiYAwal + Mathf.Sin(Time.time * kecepatanNaikTurun) * tinggiNaikTurun;
        
        // Terapkan posisi Y baru (Posisi X dan Z tetap diam sesuai koordinat asalnya)
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // --- 2. DETEKSI TABRAKAN ---
    private void OnTriggerEnter(Collider other)
    {
        // Pastikan objek Bola Anda sudah diberi Tag "Player" di Inspector Unity
        if (other.CompareTag("Player"))
        {
            // Panggil fungsi penambah skor koin di GameManager
            if (this.gameManager != null)
            {
                this.gameManager.AmbilKoin();
            }

            // ========================================================
            // TAMBAHAN: Logika memunculkan dan menghancurkan partikel
            // ========================================================
            if (prefabPartikelKoin != null)
            {
                // Munculkan partikel persis di koordinat koin saat ini
                GameObject efek = Instantiate(prefabPartikelKoin, transform.position, Quaternion.identity);
                
                // Langsung jadikan efek ini independen (tidak mengikuti pergerakan apapun)
                efek.transform.parent = null;

                // Hancurkan objek partikel dari hierarchy setelah durasi tertentu (misal 0.6 detik)
                Destroy(efek, durasiPartikel);
            }

            // Hancurkan koin asli dari scene setelah diambil
            Destroy(gameObject);
        }
    }
}