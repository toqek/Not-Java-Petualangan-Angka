using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
 : MonoBehaviour
{
    // ── Operasi Matematika ───────────────────────────────────────────────
    public enum OperasiMatematika { Penjumlahan, Pengurangan, Perkalian, Pembagian }
    private OperasiMatematika operasiAktif = OperasiMatematika.Penjumlahan;

    [SerializeField] private int GameState;
    [Header("Prefab Lintasan")]
    public GameObject[] Lintasan = new GameObject[3];
    private int IndexLintasan = 0;

    [Header("Player")]
    public GameObject Player;
    public float PosisiPlayer;
    public TextMeshProUGUI TMPSpeed;

    [Header("Lintasan Awal di Scene")]
    public GameObject LintasanAwal1, LintasanAwal2, LintasanAwal3;
    private List<GameObject> DaftarLintasan = new List<GameObject>();

    [Header("Konfigurasi Lintasan")]
    public float PosisiLintasanBaru = 48f * 3;
    public float BatasLintasanBaru = 48f * 2 / 3;
    public float PanjangLintasan = 48f;

    [Header("Sistem Soal Matematika")]
    public TextMeshProUGUI TMPSoal;
    public GameObject PrefabJawaban;
    public int JumlahJawaban = 3;
    public float JarakJawaban = 3.7f;
    public float PosisiSoalBaru = 50f;

    [Header("Warna Gerbang Jawaban")]
    public Color[] WarnaGerbang = new Color[3] {
        new Color(0.2f, 0.6f, 1f),   // Biru
        new Color(1f, 0.3f, 0.3f),   // Merah
        new Color(0.3f, 1f, 0.4f)    // Hijau
    };
    
    [HideInInspector] public int JawabanBenar;
    [HideInInspector] public bool SpawnSoal;

    private List<GameObject> ListJawaban = new List<GameObject>();
    
    // PERBAIKAN: List untuk mencatat posisi Z gerbang yang sedang aktif di scene
    private List<float> PosisiZGerbangAktif = new List<float>();

    [Header("Informasi Skor")]
    public TextMeshProUGUI TMPScore;
    private int SkorTertinggi = 0;
    public int SkorJawaban = 10;
    public int SkorKoin = 1;

    [Header("Sistem Otomatis Koin")]
    public GameObject PrefabKoin;          
    private float PosisiKoinZBaru = 15f;    
    private float LajurTerakhirX = 0f;      

    [Header("Sistem Otomatis Rintangan Batu")]
    public GameObject PrefabBatu;          
    private float PosisiBatuZBaru = 30f;    
    
    private List<GameObject> ListBatuAktif = new List<GameObject>();
    private int MaksimalBatuDiScene = 2;

    private float[] opsiX = new float[] { -3.7f, 0f, 3.7f }; 

    void Start()
    {
        this.TMPSoal.enabled=true;
        
        GameState = 1;

        string namaScene = SceneManager.GetActiveScene().name.ToLower();
        if (namaScene.Contains("pengurangan"))       operasiAktif = OperasiMatematika.Pengurangan;
        else if (namaScene.Contains("perkalian"))    operasiAktif = OperasiMatematika.Perkalian;
        else if (namaScene.Contains("pembagian"))    operasiAktif = OperasiMatematika.Pembagian;
        else                                         operasiAktif = OperasiMatematika.Penjumlahan;
        
        DaftarLintasan.Add(LintasanAwal1);
        DaftarLintasan.Add(LintasanAwal2);
        DaftarLintasan.Add(LintasanAwal3);

        UpdateTextSkor();

        BuatSoal();
        SpawnSoal = true;

        GenerateFormasiKoin(3);
        GenerateRintanganBatu(3); 

        UpdateTextSpeed();
    }

        

    void Update()
    {
        if (GameState == 1)
        {
            PosisiPlayer = Player.transform.position.z;

            if (PosisiPlayer > BatasLintasanBaru && DaftarLintasan.Count == 3)
            {
                BuatJalurBaru();
            }
            else if (DaftarLintasan.Count > 3)
            {
                HapusJalurLama();
            }

            if (!SpawnSoal)
            {
                BuatSoal();
                SpawnSoal = true;
            }

            HapusBatuSudahDilewati();
        }
    }

    void BuatJalurBaru()
    {
        GameObject LintasanBaru = Instantiate(Lintasan[IndexLintasan], new Vector3(0, 0, PosisiLintasanBaru), Quaternion.identity);
        DaftarLintasan.Add(LintasanBaru);

        IndexLintasan = (IndexLintasan + 1) % Lintasan.Length;
        PosisiLintasanBaru += PanjangLintasan;
        BatasLintasanBaru += (PanjangLintasan * 2f / 3f);

        GenerateFormasiKoin(3);
        GenerateRintanganBatu(2); 
    }

    void HapusJalurLama()
    {
        GameObject LintasanLama = DaftarLintasan[0];
        if (PosisiPlayer > LintasanLama.transform.position.z + PanjangLintasan)
        {
            DaftarLintasan.RemoveAt(0);
            Destroy(LintasanLama);
        }
    }

    void BuatSoal()
    {
        int a, b;
        string simbolOperasi;

        switch (operasiAktif)
        {
            case OperasiMatematika.Pengurangan:
                a = UnityEngine.Random.Range(2, 20);
                b = UnityEngine.Random.Range(1, a);
                JawabanBenar = a - b;
                simbolOperasi = "-";
                break;

            case OperasiMatematika.Perkalian:
                a = UnityEngine.Random.Range(1, 10);
                b = UnityEngine.Random.Range(1, 10);
                JawabanBenar = a * b;
                simbolOperasi = "X";
                break;

            case OperasiMatematika.Pembagian:
            {
                // Blok {} wajib agar deklarasi lokal 'int hasil' tidak bentrok antar case (CS0136)
                b = UnityEngine.Random.Range(1, 10);
                int hasil = UnityEngine.Random.Range(1, 10);
                a = b * hasil;
                JawabanBenar = hasil;
                simbolOperasi = "/";
                break;
            }

            default: // Penjumlahan
                a = UnityEngine.Random.Range(1, 10);
                b = UnityEngine.Random.Range(1, 10);
                JawabanBenar = a + b;
                simbolOperasi = "+";
                break;
        }

        TMPSoal.SetText($"{a} {simbolOperasi} {b} = ?");
        int indeksJawabanBenar = UnityEngine.Random.Range(0, JumlahJawaban);

        // PERBAIKAN: Catat posisi koordinat Z gerbang ini sebelum di-instantiate
        PosisiZGerbangAktif.Add(PosisiSoalBaru);

        // Acak urutan warna tanpa duplikat (Fisher-Yates shuffle)
        int[] urutanWarna = new int[] { 0, 1, 2 };
        for (int s = urutanWarna.Length - 1; s > 0; s--)
        {
            int acak = UnityEngine.Random.Range(0, s + 1);
            int tmp = urutanWarna[s];
            urutanWarna[s] = urutanWarna[acak];
            urutanWarna[acak] = tmp;
        }

        for (int i = 0; i < JumlahJawaban; i++)
        {
            Vector3 posisiSpawn = new Vector3(0, 2, PosisiSoalBaru);
            if (i == 1) posisiSpawn += Vector3.right * JarakJawaban;
            else if (i == 2) posisiSpawn += Vector3.left * JarakJawaban;

            GameObject ObjJawaban = Instantiate(PrefabJawaban, posisiSpawn, Quaternion.identity);
            GerbangJawaban gerbangScript = ObjJawaban.GetComponent<GerbangJawaban>();

            Renderer rend = ObjJawaban.GetComponentInChildren<Renderer>();
            if (rend != null && i < urutanWarna.Length)
            {
                Color warna = WarnaGerbang[urutanWarna[i]];
                rend.material.color = warna;
                rend.material.SetColor("_EmissionColor", warna * 2f); 
            }

            if (i == indeksJawabanBenar)
            {
                gerbangScript.setText(JawabanBenar.ToString(), this);
            }
            else
            {
                int rangeJawaban = (operasiAktif == OperasiMatematika.Perkalian) ? 81 : 20;
                int jawabanSalah = UnityEngine.Random.Range(1, rangeJawaban);
                while (jawabanSalah == JawabanBenar)
                {
                    jawabanSalah = UnityEngine.Random.Range(1, rangeJawaban);
                }
                gerbangScript.setText(jawabanSalah.ToString(), this);
            }

            ListJawaban.Add(ObjJawaban);
        }

        if (PosisiKoinZBaru < PosisiSoalBaru + 5f) PosisiKoinZBaru = PosisiSoalBaru + 10f;

        PosisiSoalBaru += 50f;
    }

    // Kumpulkan semua posisi gerbang: yang aktif + yang sudah direncanakan berikutnya
    List<float> DapatkanSemuaPosisiGerbang()
    {
        List<float> semua = new List<float>(PosisiZGerbangAktif);
        float gerbangBerikutnya = PosisiSoalBaru - 50f;
        if (!semua.Contains(gerbangBerikutnya))
            semua.Add(gerbangBerikutnya);
        return semua;
    }

    void GenerateRintanganBatu(int jumlahBarisBatu)
    {
        if (PrefabBatu == null) return;

        for (int b = 0; b < jumlahBarisBatu; b++)
        {
            if (ListBatuAktif.Count >= MaksimalBatuDiScene)
            {
                return; 
            }

            // Geser posisi batu sampai aman dari SEMUA gerbang (zona steril 10f depan & belakang).
            // While-loop restart dari awal setiap kali ada pergeseran. Maks 30 iterasi.
            int iterasi = 0;
            bool perluGeser = true;
            while (perluGeser && iterasi < 30)
            {
                perluGeser = false;
                foreach (float zGerbang in DapatkanSemuaPosisiGerbang())
                {
                    float delta = PosisiBatuZBaru - zGerbang;
                    if (delta > -10f && delta < 10f)
                    {
                        PosisiBatuZBaru = zGerbang + 10f;
                        perluGeser = true;
                        break;
                    }
                }
                iterasi++;
            }

            int jumlahBatuDiBarisIni = UnityEngine.Random.Range(1, 3); 

            if (jumlahBatuDiBarisIni == 1)
            {
                float lajurX = opsiX[UnityEngine.Random.Range(0, opsiX.Length)];
                
                // Sinkronisasi ketinggian Y batu agar pas di atas jalan (misal: 0.1f atau sesuai prefab)
                Vector3 posisiBatu = new Vector3(lajurX, 0.1f, PosisiBatuZBaru);
                
                GameObject batuBaru = Instantiate(PrefabBatu, posisiBatu, Quaternion.identity);
                ListBatuAktif.Add(batuBaru);
            }
            else if (jumlahBatuDiBarisIni == 2)
            {
                int lajurLolos = UnityEngine.Random.Range(0, 3); 

                for (int i = 0; i < 3; i++)
                {
                    if (i != lajurLolos && ListBatuAktif.Count < MaksimalBatuDiScene)
                    {
                        Vector3 posisiBatu = new Vector3(opsiX[i], 0.1f, PosisiBatuZBaru);
                        GameObject batuBaru = Instantiate(PrefabBatu, posisiBatu, Quaternion.identity);
                        ListBatuAktif.Add(batuBaru);
                    }
                }
            }

            PosisiBatuZBaru += UnityEngine.Random.Range(15f, 25f);
        }
    }

    void HapusBatuSudahDilewati()
    {
        int jumlahBatuDihapus = 0;

        for (int i = ListBatuAktif.Count - 1; i >= 0; i--)
        {
            GameObject batu = ListBatuAktif[i];

            if (batu == null)
            {
                ListBatuAktif.RemoveAt(i);
            }
            else if (PosisiPlayer > batu.transform.position.z + 3f)
            {
                ListBatuAktif.RemoveAt(i);
                Destroy(batu);
                jumlahBatuDihapus++;
            }
        }

        // Generate di luar loop agar tidak dipanggil berkali-kali dalam 1 frame
        if (jumlahBatuDihapus > 0)
        {
            GenerateRintanganBatu(jumlahBatuDihapus);
        }
    }

    void GenerateFormasiKoin(int jumlahGrup)
    {
        if (PrefabKoin == null) return;

        for (int g = 0; g < jumlahGrup; g++)
        {
            float lajurX = opsiX[UnityEngine.Random.Range(0, opsiX.Length)];
            while (lajurX == LajurTerakhirX)
            {
                lajurX = opsiX[UnityEngine.Random.Range(0, opsiX.Length)];
            }
            LajurTerakhirX = lajurX;

            int panjangDeret = UnityEngine.Random.Range(3, 6);
            float jarakKoinZ = UnityEngine.Random.Range(2.5f, 3.5f);

            for (int i = 0; i < panjangDeret; i++)
            {
                if (Mathf.Abs(PosisiKoinZBaru - PosisiBatuZBaru) < 3f)
                {
                    PosisiKoinZBaru += 4f; 
                }

                Vector3 posisiSpawnKoin = new Vector3(lajurX, 1f, PosisiKoinZBaru);
                Quaternion rotasiKoin90X = Quaternion.Euler(90f, 0f, UnityEngine.Random.Range(0, 360));

                Instantiate(PrefabKoin, posisiSpawnKoin, rotasiKoin90X);
                PosisiKoinZBaru += jarakKoinZ;
            }

            PosisiKoinZBaru += UnityEngine.Random.Range(8f, 15f);
        }
    }
    

    public void AmbilKoin()
    {
        SkorTertinggi += SkorKoin;
        UpdateTextSkor();
    }

    void UpdateTextSkor()
    {
        if (TMPScore != null) TMPScore.SetText("Poin:" + SkorTertinggi);
    }

    void UpdateTextSpeed()
    {
        TMPSpeed.SetText("Speed:" +Player.GetComponent<PlayerController>().KecepatanMaju);
    
    }

    public void PlayerKalah()
    {
        Debug.Log("GAME OVER!");
        GameState = 3; 
        
        PlayerController playerCtrl = Player.GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            playerCtrl.BisaJalan = false;
        }

       this.GetComponent<UIManager>().TampilkanMenuGameOver(SkorTertinggi);
       this.TMPSoal.enabled=false;
    }

    public void CheckJawaban(string nilai)
    {
        int n = int.Parse(nilai);

        if (n == JawabanBenar)
        {
            

            Debug.Log("Jawaban Benar!");
            SkorTertinggi += SkorJawaban;
            UpdateTextSkor();
            Player.GetComponent<PlayerController>().KecepatanMaju+=0.8f;
            UpdateTextSpeed();
            // Sesaat setelah jawaban benar diverifikasi, hapus catatan posisi Z gerbang ini dari list
            if (PosisiZGerbangAktif.Count > 0)
            {
                PosisiZGerbangAktif.RemoveAt(0);
            }

            foreach (GameObject gerbang in ListJawaban)
            {
                if (gerbang != null) Destroy(gerbang);
            }
            ListJawaban.Clear();
            SpawnSoal = false; 
        }
        else
        {
            Debug.Log("Jawaban Salah!");
            PlayerKalah(); 
        }
    }
}