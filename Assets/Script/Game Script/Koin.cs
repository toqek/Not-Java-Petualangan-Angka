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
        if (other.CompareTag("Player"))
        {
            // 1. Panggil fungsi penambah skor koin di GameManager
            if (this.gameManager != null)
            {
                this.gameManager.AmbilKoin();
            }

            // ========================================================
            // PERBAIKAN AUDIO: Ambil clip dari AudioSource internal,
            // lalu putar secara independen agar tidak terpotong saat Destroy!
            // ========================================================
            AudioSource audioInternal = GetComponent<AudioSource>();
            if (audioInternal != null && audioInternal.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioInternal.clip, transform.position);
            }

            // 2. Logika memunculkan dan menghancurkan partikel
            if (prefabPartikelKoin != null)
            {
                GameObject efek = Instantiate(prefabPartikelKoin, transform.position, Quaternion.identity);
                efek.transform.parent = null;
                Destroy(efek, durasiPartikel);
            }

            // 3. Hancurkan koin asli dari scene setelah diambil (Sekarang Aman!)
            Destroy(gameObject);
        }
    }
}