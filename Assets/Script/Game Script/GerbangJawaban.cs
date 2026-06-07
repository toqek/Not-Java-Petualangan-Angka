using System;
using TMPro;
using UnityEngine;

public class GerbangJawaban : MonoBehaviour
{
    public TextMeshPro TMPJawaban;
    private GameManager GM;

    // ========================================================
    // TAMBAHAN: Variabel untuk Efek Partikel Gerbang
    // ========================================================
    [Header("Efek Partikel Gerbang")]
    public GameObject prefabPartikelGerbang; // Taruh prefab partikel hancur di sini via Inspector
    public float durasiPartikel = 1f;         // Waktu sebelum objek partikel dihancurkan

    void Start()
    {
    }

    void Update()
    {
    }

    public void setText(string Jawaban, GameManager GM)
    {
        this.GM = GM;
        if (TMPJawaban != null)
        {
            TMPJawaban.SetText(Jawaban);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GM != null)
        {
            string teksJawaban = TMPJawaban.text.Trim();
            Debug.Log("Player melewati gerbang dengan jawaban: " + teksJawaban);
            
            // --- MODIFIKASI: SPAWN PARTIKEL DENGAN WARNA DINAMIS ---
            if (prefabPartikelGerbang != null)
            {
                // 1. Ambil warna asli dari mesh gerbang ini saat ini
                Color warnaGerbangIni = Color.white; // Default jika tidak ketemu
                Renderer rend = GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    warnaGerbangIni = rend.material.color;
                }

                // 2. Spawn prefab partikel di posisi gerbang
                GameObject efek = Instantiate(prefabPartikelGerbang, transform.position, Quaternion.identity);

                // 3. Suntikkan warna gerbang ke komponen Particle System yang baru lahir
                ParticleSystem ps = efek.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    // Mengubah warna utama (Main Module) dari partikel secara real-time
                    var mainModule = ps.main;
                    mainModule.startColor = warnaGerbangIni;
                }

                // 4. Hancurkan objek partikel dari hierarchy setelah durasi selesai
                Destroy(efek, durasiPartikel);
            }

            // Jalankan pengecekan jawaban di GameManager
            GM.CheckJawaban(teksJawaban);
        }
    }
}