# 🎮 Tutorial Game 3D Endless Runner Matematika — Unity
**Panduan Lengkap untuk Pemula | Step by Step**

---

> **Deskripsi Game:** Endless runner 3D edukatif. Player secara otomatis berada di jalur, rintangan 3D bergerak mendekati player dari depan. Player melompat menghindari rintangan, mengumpulkan koin 3D, dan menjawab soal matematika lewat UI untuk mendapatkan skor. Game over saat menabrak rintangan.

---

## 📋 Daftar Isi

- [Gambaran Arsitektur Game](#gambaran-arsitektur-game)
- [TAHAP 1 — Setup Project Unity](#tahap-1--setup-project-unity)
- [TAHAP 2 — Scene MainMenu](#tahap-2--scene-mainmenu)
- [TAHAP 3 — Setup Scene Game 3D](#tahap-3--setup-scene-game-3d)
- [TAHAP 4 — Semua Script C#](#tahap-4--semua-script-c)
- [TAHAP 5 — UI Scene Game](#tahap-5--ui-scene-game)
- [TAHAP 6 — Hubungkan Semua](#tahap-6--hubungkan-semua)
- [TAHAP 7 — Test Core Loop](#tahap-7--test-core-loop)
- [Troubleshooting](#-troubleshooting)
- [Pengembangan Selanjutnya](#-pengembangan-selanjutnya)

---

## Gambaran Arsitektur Game

```
ALUR SCENE:
  MainMenu Scene  ──►  Game Scene
       ↑                    │
       └────────────────────┘
        (Game Over → Main Menu)

SCENE MAINMENU:
  Canvas
  ├── MainMenuPanel   (Mulai / Tentang / Keluar)
  ├── TentangPanel    (Info game)
  └── PilihLevelPanel (Penjumlahan / Pengurangan / Perkalian / Pembagian)

SCENE GAME (3D):
  ├── Ground (3D Plane)
  ├── Player (3D Capsule + Rigidbody)
  ├── Camera (perspektif 3D, belakang player)
  ├── Spawner (ObstacleSpawner + CoinSpawner)
  ├── GameManager
  ├── MathManager
  └── Canvas UI (Skor | GameOver Panel | Soal Panel)

MECHANIC GAME:
  Rintangan (3D Cube) ────► bergerak ke arah -Z ────► tabrak player = GAME OVER
  Koin (3D Sphere)    ────► bergerak ke arah -Z ────► tabrak player = +10 poin
  Soal Matematika     ────► muncul tiap 7 detik  ────► jawaban benar = +20 poin
```

**Mengapa ini 3D?**
- Semua objek game (player, rintangan, koin, tanah) adalah 3D GameObject di Unity
- Menggunakan Rigidbody 3D dengan gravitasi nyata untuk fisika lompatan
- Kamera perspektif 3D dari belakang player (third-person view)
- Obstacle dan koin bergerak di sumbu Z ruang 3D

---

## TAHAP 1 — Setup Project Unity

### Langkah 1.1 — Buat Project Baru

1. Buka **Unity Hub**
2. Klik **New Project**
3. Pilih template **3D (Core)**
4. Beri nama: `EndlessRunnerMath`
5. Klik **Create Project**

> ⚠️ **Penting:** Pilih template **3D**, bukan 2D atau URP, agar physics dan camera sudah terkonfigurasi untuk 3D.

---

### Langkah 1.2 — Install TextMeshPro

1. Menu **Window → Package Manager**
2. Klik dropdown **Unity Registry**
3. Cari: `TextMeshPro`
4. Klik **Install**
5. Jika muncul pop-up _"Import TMP Essentials"_ → klik **Import TMP Essentials**

> 💡 TextMeshPro diperlukan agar teks soal matematika tampil tajam di semua resolusi layar.

---

### Langkah 1.3 — Buat Struktur Folder

Di panel **Project**, klik kanan folder `Assets` → `Create → Folder`. Buat folder-folder berikut:

```
Assets/
├── Scripts/
├── Prefabs/
├── Materials/
└── Scenes/
```

---

### Langkah 1.4 — Siapkan 2 Scene & Daftarkan ke Build

**Scene MainMenu:**
- Rename scene default (`SampleScene`) → klik kanan di Project → Rename → ketik `MainMenu`
- Pindahkan ke `Assets/Scenes/`

**Scene Game:**
- Menu `File → New Scene → Basic (Built-in)`
- `File → Save As` → simpan sebagai `Game` di `Assets/Scenes/`

**Daftarkan ke Build Settings:**
- Menu `File → Build Settings`
- Drag kedua scene ke kotak _"Scenes In Build"_ dengan urutan:
  - **Index 0: MainMenu**
  - **Index 1: Game**

> ⚠️ **Kritis:** Urutan index WAJIB benar! `SceneManager.LoadScene("MainMenu")` mengacu pada nama ini.

---

## TAHAP 2 — Scene MainMenu

> Double-click scene **MainMenu** di panel Project untuk membukanya.

### Langkah 2.1 — Buat Canvas

1. Klik kanan **Hierarchy** → `UI → Canvas`
2. Unity otomatis membuat Canvas + EventSystem
3. Klik **Canvas** → Inspector → **Canvas Scaler**
   - UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: `1920 × 1080`

---

### Langkah 2.2 — Buat 3 Panel

Klik kanan **Canvas** → `UI → Panel`. Buat 3 panel dan rename:

| Nama Panel | Aktif Saat Start |
|---|---|
| `MainMenuPanel` | ✅ Aktif |
| `TentangPanel` | ❌ Nonaktif |
| `PilihLevelPanel` | ❌ Nonaktif |

Untuk menonaktifkan: klik panel → Inspector → **uncheck** centang di sebelah nama objek (kotak di pojok kiri atas Inspector).

---

### Langkah 2.3 — Isi MainMenuPanel

Klik kanan **MainMenuPanel** → tambahkan elemen berikut:

```
MainMenuPanel
├── Text - TextMeshPro  → rename "TitleText"
│    teks: "MATH RUNNER"  |  Size: 72  |  Bold  |  Center
├── Button - TextMeshPro → rename "BtnMulai"
│    teks: "▶ MULAI"
├── Button - TextMeshPro → rename "BtnTentang"
│    teks: "ℹ TENTANG"
└── Button - TextMeshPro → rename "BtnKeluar"
     teks: "✖ KELUAR"
```

---

### Langkah 2.4 — Isi TentangPanel

```
TentangPanel
├── Text - TMP  → teks: "Game edukasi matematika. Lompat hindari
│                 rintangan, kumpul koin & jawab soal untuk skor!"
└── Button - TMP → rename "BtnTutupTentang"  |  teks: "TUTUP"
```

---

### Langkah 2.5 — Isi PilihLevelPanel

```
PilihLevelPanel
├── Text - TMP          → "Pilih Operasi Matematika:"
├── Button - TMP        → rename "BtnPenjumlahan"   teks: "➕ PENJUMLAHAN"
├── Button - TMP        → rename "BtnPengurangan"   teks: "➖ PENGURANGAN"
├── Button - TMP        → rename "BtnPerkalian"     teks: "✖ PERKALIAN"
├── Button - TMP        → rename "BtnPembagian"     teks: "➗ PEMBAGIAN"
└── Button - TMP        → rename "BtnBack"          teks: "◀ KEMBALI"
```

---

### Langkah 2.6 — Buat & Attach Script MainMenuManager

1. Klik kanan `Assets/Scripts` → `Create → C# Script` → nama: `MainMenuManager`
2. Klik kanan Hierarchy → `Create Empty` → rename `MenuManager`
3. Drag script `MainMenuManager` ke Inspector dari `MenuManager`

---

### Langkah 2.7 — Hubungkan Tombol ke Script

Klik **MenuManager** → di Inspector, drag panel ke slot:

| Slot di Inspector | Drag dari Hierarchy |
|---|---|
| `mainMenuPanel` | MainMenuPanel |
| `tentangPanel` | TentangPanel |
| `pilihLevelPanel` | PilihLevelPanel |

Hubungkan tombol (klik tombol → On Click() → **+** → drag MenuManager):

| Tombol | Fungsi | Parameter |
|---|---|---|
| BtnMulai | `MainMenuManager.OnMulaiClick` | — |
| BtnTentang | `MainMenuManager.OnTentangClick` | — |
| BtnTutupTentang | `MainMenuManager.OnTutupTentangClick` | — |
| BtnKeluar | `MainMenuManager.OnKeluarClick` | — |
| BtnPenjumlahan | `MainMenuManager.OnPilihLevel` | `Penjumlahan` |
| BtnPengurangan | `MainMenuManager.OnPilihLevel` | `Pengurangan` |
| BtnPerkalian | `MainMenuManager.OnPilihLevel` | `Perkalian` |
| BtnPembagian | `MainMenuManager.OnPilihLevel` | `Pembagian` |
| BtnBack | `MainMenuManager.OnBackToMenuClick` | — |

> 💡 Untuk `OnPilihLevel`: setelah pilih fungsi, akan muncul kolom teks di bawahnya. Isi dengan nama level — **PERSIS** seperti tabel di atas (case-sensitive!).

---

## TAHAP 3 — Setup Scene Game 3D

> Double-click scene **Game** di panel Project.

### Langkah 3.1 — Buat Tanah 3D (Ground)

1. Klik kanan Hierarchy → `3D Object → Plane`
2. Rename: `Ground`
3. Set Transform:
   - **Position:** `(0, 0, 10)`
   - **Scale:** `(3, 1, 10)` → lebar 3 unit, panjang 100 unit
4. **Set Tag "Ground":**
   - Klik `Tag → Add Tag → +` → ketik `Ground` → Save
   - Kembali ke Ground → ubah Tag ke **Ground**

> ⚠️ Ground JANGAN diberi Is Trigger. Harus collider biasa agar player bisa berdiri di atasnya menggunakan physics 3D.

---

### Langkah 3.2 — Buat Player 3D

1. Klik kanan Hierarchy → `3D Object → Capsule`
2. Rename: `Player`
3. Transform: **Position `(0, 1, 0)`**
4. **Tambah Rigidbody 3D:**
   - Inspector → `Add Component → Physics → Rigidbody`
   - Di Rigidbody: `Constraints → Freeze Rotation` → centang **X ✓ Y ✓ Z ✓**

> 💡 Freeze Rotation mencegah player oleng/miring saat menabrak rintangan. Gravity tetap aktif agar player jatuh dengan natural setelah lompat.

---

### Langkah 3.3 — Posisikan Kamera 3D (Third-Person View)

Klik **Main Camera** di Hierarchy. Set Transform:
- **Position:** `(0, 4, -8)`
- **Rotation:** `(20, 0, 0)`

Ini menempatkan kamera di belakang-atas player dengan sudut pandang 3D perspektif menghadap ke depan (arah +Z).

---

### Langkah 3.4 — Tambahkan Tags: Obstacle & Coin

Klik objek mana saja → Tag → `Add Tag` → klik **+** dua kali:
- `Obstacle`
- `Coin`

---

### Langkah 3.5 — Buat Prefab Rintangan 3D (Obstacle)

1. Klik kanan Hierarchy → `3D Object → Cube` → rename `ObstaclePrefab`
2. Scale: **`(1.5, 1.5, 1.5)`**
3. Buat material merah:
   - Klik kanan `Assets/Materials` → `Create → Material`
   - Ubah Albedo jadi warna merah
   - Drag material ke Cube di Scene/Hierarchy
4. Box Collider → centang **Is Trigger ✓**
5. Tag → set ke `Obstacle`
6. Drag script `MoveObject` ke Inspector → `speed = 8`
7. **Jadikan Prefab:** Drag dari Hierarchy → ke `Assets/Prefabs`
8. **Delete** dari Hierarchy (sudah tersimpan sebagai prefab)

---

### Langkah 3.6 — Buat Prefab Koin 3D (Coin)

1. Klik kanan Hierarchy → `3D Object → Sphere` → rename `CoinPrefab`
2. Scale: **`(0.5, 0.5, 0.5)`**
3. Buat material kuning → drag ke Sphere
4. Sphere Collider → **Is Trigger ✓**
5. Tag → set ke `Coin`
6. Drag script `MoveObject` → `speed = 7`
7. Drag ke `Assets/Prefabs` → Delete dari Hierarchy

---

### Langkah 3.7 — Buat GameObject Spawner

1. Klik kanan Hierarchy → `Create Empty` → rename `Spawner`
2. Position: `(0, 0, 0)`

> Satu GameObject ini akan menampung 2 script: `ObstacleSpawner` + `CoinSpawner`.

---

## TAHAP 4 — Semua Script C#

> Untuk setiap script: klik kanan `Assets/Scripts` → `Create → C# Script` → beri nama → double-click untuk buka → hapus semua isi → paste kode → **Ctrl+S** untuk save.

---

### Script 1: MainMenuManager.cs

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel-Panel UI")]
    public GameObject mainMenuPanel;
    public GameObject tentangPanel;
    public GameObject pilihLevelPanel;

    // Tombol MULAI
    public void OnMulaiClick()
    {
        mainMenuPanel.SetActive(false);
        pilihLevelPanel.SetActive(true);
    }

    // Tombol TENTANG
    public void OnTentangClick()
    {
        tentangPanel.SetActive(true);
    }

    // Tombol TUTUP di panel Tentang
    public void OnTutupTentangClick()
    {
        tentangPanel.SetActive(false);
    }

    // Tombol KELUAR
    public void OnKeluarClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Dipanggil oleh 4 tombol level — parameter diisi di Inspector
    public void OnPilihLevel(string namaLevel)
    {
        // Simpan level yang dipilih agar dibaca oleh MathQuestionManager
        PlayerPrefs.SetString("Level", namaLevel);
        // Load scene game
        SceneManager.LoadScene("Game");
    }

    // Tombol KEMBALI dari PilihLevel
    public void OnBackToMenuClick()
    {
        pilihLevelPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
```

---

### Script 2: GameManager.cs

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton — agar bisa diakses dari script lain dengan GameManager.Instance
    public static GameManager Instance;

    [Header("Skor")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Panel Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [HideInInspector]
    public bool isGameOver = false;

    void Awake()
    {
        // Pastikan hanya ada 1 GameManager di scene
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        score = 0;
        isGameOver = false;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Skor: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;   // Pause game
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null)
            finalScoreText.text = "Skor Akhir: " + score;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
```

---

### Script 3: PlayerController.cs

```csharp
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Kekuatan Lompat")]
    public float jumpForce = 8f;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        // Input lompat: Spasi (PC) atau klik/tap (mobile)
        bool jumpInput = Input.GetKeyDown(KeyCode.Space)
                      || Input.GetMouseButtonDown(0);

        if (jumpInput && isGrounded)
        {
            // Lompat menggunakan Rigidbody 3D physics
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // Mendarat di Ground (collider biasa, bukan trigger)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    // Interaksi dengan Obstacle & Coin (trigger collider)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Tabrak rintangan 3D -> Game Over
            GameManager.Instance.GameOver();
        }

        if (other.CompareTag("Coin"))
        {
            // Ambil koin 3D -> +10 poin
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
        }
    }
}
```

---

### Script 4: MoveObject.cs

```csharp
using UnityEngine;

// Pasang script ini di prefab Rintangan (speed=8) dan Koin (speed=7)
public class MoveObject : MonoBehaviour
{
    [Header("Kecepatan Gerak di Dunia 3D")]
    public float speed = 8f;

    [Header("Titik Hapus (posisi Z)")]
    public float destroyAtZ = -15f;

    void Update()
    {
        // Berhenti bergerak saat game over
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        // Gerak ke arah -Z (sumbu Z negatif = menuju player di ruang 3D)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        // Hapus object dari memori jika sudah melewati player
        if (transform.position.z < destroyAtZ)
            Destroy(gameObject);
    }
}
```

---

### Script 5: ObstacleSpawner.cs

```csharp
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab Rintangan 3D")]
    public GameObject[] obstaclePrefabs;

    [Header("Pengaturan Spawn")]
    public float spawnInterval = 3f;  // Detik antar spawn
    public float spawnZ = 25f;        // Jarak spawn di depan player (sumbu Z)

    private float timer = 0f;

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
            // Tingkat kesulitan: interval makin cepat, minimum 1.2 detik
            spawnInterval = Mathf.Max(1.2f, spawnInterval - 0.03f);
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;
        int i = Random.Range(0, obstaclePrefabs.Length);
        // Spawn di titik Z positif (depan player di ruang 3D)
        Vector3 pos = new Vector3(0f, 0.75f, spawnZ);
        Instantiate(obstaclePrefabs[i], pos, Quaternion.identity);
    }
}
```

---

### Script 6: CoinSpawner.cs

```csharp
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Prefab Koin 3D")]
    public GameObject coinPrefab;

    [Header("Pengaturan Spawn")]
    public float spawnInterval = 2f;
    public float spawnZ = 25f;

    private float timer = 0f;

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCoin();
            timer = 0f;
        }
    }

    void SpawnCoin()
    {
        if (coinPrefab == null) return;
        // Koin muncul acak di kiri (-2), tengah (0), atau kanan (+2) — posisi X di ruang 3D
        float[] xOpts = { -2f, 0f, 2f };
        float x = xOpts[Random.Range(0, xOpts.Length)];
        Instantiate(coinPrefab, new Vector3(x, 0.5f, spawnZ), Quaternion.identity);
    }
}
```

---

### Script 7: MathQuestionManager.cs

```csharp
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MathQuestionManager : MonoBehaviour
{
    public static MathQuestionManager Instance;

    [Header("Referensi UI")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;        // 4 tombol jawaban
    public TextMeshProUGUI[] answerTexts; // TextMeshPro di tiap tombol

    [Header("Interval Soal (detik)")]
    public float questionInterval = 7f;

    private int correctAnswer;
    private float timer = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() => GenerateNewQuestion();

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;
        timer += Time.deltaTime;
        if (timer >= questionInterval)
        {
            GenerateNewQuestion();
            timer = 0f;
        }
    }

    public void GenerateNewQuestion()
    {
        // Ambil level yang dipilih dari MainMenu (tersimpan di PlayerPrefs)
        string level = PlayerPrefs.GetString("Level", "Penjumlahan");
        int a = Random.Range(2, 15);
        int b = Random.Range(2, 10);

        switch (level)
        {
            case "Penjumlahan":
                correctAnswer = a + b;
                questionText.text = a + "  +  " + b + "  =  ?";
                break;

            case "Pengurangan":
                if (a < b) { int t = a; a = b; b = t; } // Hasil tidak negatif
                correctAnswer = a - b;
                questionText.text = a + "  -  " + b + "  =  ?";
                break;

            case "Perkalian":
                a = Random.Range(2, 10);
                b = Random.Range(2, 10);
                correctAnswer = a * b;
                questionText.text = a + "  x  " + b + "  =  ?";
                break;

            case "Pembagian":
                int hasil = Random.Range(2, 10);
                correctAnswer = hasil;
                questionText.text = (hasil * b) + "  /  " + b + "  =  ?";
                break;
        }

        SetupAnswerButtons();
    }

    void SetupAnswerButtons()
    {
        // Posisi tombol yang benar dipilih secara acak
        int correctPos = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            int val;
            if (i == correctPos)
            {
                val = correctAnswer;
            }
            else
            {
                // Buat jawaban salah
                int offset;
                do { offset = Random.Range(-8, 9); } while (offset == 0);
                val = Mathf.Max(1, correctAnswer + offset);
            }

            answerTexts[i].text = val.ToString();

            // Capture nilai per iterasi untuk lambda closure
            int captured = val;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(captured));
        }
    }

    void OnAnswerSelected(int selected)
    {
        if (selected == correctAnswer)
            GameManager.Instance.AddScore(20); // Jawaban benar → +20 poin

        // Reset timer dan generate soal baru
        timer = 0f;
        GenerateNewQuestion();
    }
}
```

---

## TAHAP 5 — UI Scene Game

> Pastikan kamu sedang di scene **Game**.

### Langkah 5.1 — Buat Canvas

1. Klik kanan Hierarchy → `UI → Canvas`
2. Canvas Scaler → **Scale With Screen Size** → `1920 × 1080`

---

### Langkah 5.2 — Score Text (Kiri Atas)

1. Klik kanan Canvas → `UI → Text - TextMeshPro` → rename `ScoreText`
2. Klik kotak Anchor (Rect Transform) → pilih pojok **top-left**
3. Pos X: `20`, Pos Y: `-30`
4. Teks awal: `"Skor: 0"` | Size: `40` | Bold

---

### Langkah 5.3 — Panel Game Over (Tengah Layar)

1. Klik kanan Canvas → `UI → Panel` → rename `GameOverPanel`
2. Di dalamnya buat:

```
GameOverPanel
├── Text-TMP  → rename "FinalScoreText"  teks: "Skor Akhir: 0"  Size: 60  Center
├── Button-TMP → rename "BtnUlang"      teks: "🔄 ULANG"
└── Button-TMP → rename "BtnMainMenu"   teks: "🏠 MENU UTAMA"
```

3. **KRITIS:** Klik `GameOverPanel` → Inspector → **uncheck** nama objek = nonaktifkan dari awal.

---

### Langkah 5.4 — Panel Soal Matematika (Bawah Layar)

1. Klik kanan Canvas → `UI → Panel` → rename `QuestionPanel`
2. Anchor: **Bottom Stretch** | Height: sekitar `280px`
3. Di dalam QuestionPanel buat:

```
QuestionPanel
├── Text-TMP → rename "QuestionText"   teks: "3 + 4 = ?"   Size: 52   Bold   Center
├── Button-TMP → rename "AnswerBtn0"   └─ child text rename "AnswerText0"
├── Button-TMP → rename "AnswerBtn1"   └─ child text rename "AnswerText1"
├── Button-TMP → rename "AnswerBtn2"   └─ child text rename "AnswerText2"
└── Button-TMP → rename "AnswerBtn3"   └─ child text rename "AnswerText3"
```

> 💡 Agar tombol rapi otomatis: tambah komponen **Grid Layout Group** ke QuestionPanel:
> - Cell Size: `(200, 70)`
> - Spacing: `(10, 10)`
> - Constraint: Fixed Column Count = `2`

---

### Langkah 5.5 — Buat GameObjects untuk Script

**GameManager:**
- Klik kanan Hierarchy → `Create Empty` → rename `GameManager`
- Drag script `GameManager.cs` ke Inspector

**MathManager:**
- Create Empty → rename `MathManager`
- Drag script `MathQuestionManager.cs`

---

## TAHAP 6 — Hubungkan Semua

### Langkah 6.1 — PlayerController → Player

- Klik **Player** di Hierarchy
- Drag script `PlayerController` ke Inspector
- `jumpForce` = **8** (naikkan jika ingin lompatan lebih tinggi)

---

### Langkah 6.2 — GameManager — Assign UI Slots

Klik **GameManager** di Hierarchy → komponen `GameManager.cs`:

| Slot | Drag dari |
|---|---|
| `scoreText` | Canvas/ScoreText |
| `gameOverPanel` | Canvas/GameOverPanel |
| `finalScoreText` | Canvas/GameOverPanel/FinalScoreText |

---

### Langkah 6.3 — Tombol Game Over

| Tombol | Fungsi |
|---|---|
| BtnUlang | `GameManager.RestartGame` |
| BtnMainMenu | `GameManager.GoToMainMenu` |

(Klik tombol → On Click() → **+** → drag **GameManager** → pilih fungsi)

---

### Langkah 6.4 — ObstacleSpawner & CoinSpawner → Spawner

Klik **Spawner** → drag script `ObstacleSpawner`:

| Field | Nilai |
|---|---|
| `obstaclePrefabs` | Size: 1 → drag ObstaclePrefab dari Assets/Prefabs |
| `spawnInterval` | 3 |
| `spawnZ` | 25 |

Di GameObject yang sama, drag juga `CoinSpawner`:

| Field | Nilai |
|---|---|
| `coinPrefab` | drag CoinPrefab dari Assets/Prefabs |
| `spawnInterval` | 2 |
| `spawnZ` | 25 |

---

### Langkah 6.5 — MathQuestionManager → MathManager

Klik **MathManager**:

| Field | Nilai |
|---|---|
| `questionText` | drag QuestionText |
| `answerButtons` | Size: 4 → slot 0–3: AnswerBtn0–AnswerBtn3 |
| `answerTexts` | Size: 4 → slot 0–3: AnswerText0–AnswerText3 |
| `questionInterval` | 7 |

---

### Langkah 6.6 — Verifikasi Collider Semua Prefab

**ObstaclePrefab** (Assets/Prefabs):
- Box Collider → Is Trigger: ✅ **TRUE**
- Tag: `Obstacle`
- MoveObject → speed: `8`

**CoinPrefab** (Assets/Prefabs):
- Sphere Collider → Is Trigger: ✅ **TRUE**
- Tag: `Coin`
- MoveObject → speed: `7`

**Ground** (Hierarchy):
- Mesh Collider → Is Trigger: ❌ **FALSE**
- Tag: `Ground`

---

### Langkah 6.7 — Rangkuman Hierarchy Scene Game (Final)

```
📁 Game (Scene)
├── Main Camera        pos(0, 4, -8)   rot(20, 0, 0)   [kamera 3D third-person]
├── Directional Light
├── Ground             Plane · Tag:Ground · Collider bukan trigger
├── Player             Capsule · Rigidbody (Freeze Rot XYZ) · PlayerController
├── Spawner            ObstacleSpawner + CoinSpawner
├── GameManager        GameManager.cs
├── MathManager        MathQuestionManager.cs
└── Canvas
    ├── ScoreText              TMP · anchor top-left
    ├── GameOverPanel          (nonaktif saat start)
    │   ├── FinalScoreText
    │   ├── BtnUlang           → GameManager.RestartGame
    │   └── BtnMainMenu        → GameManager.GoToMainMenu
    └── QuestionPanel          (selalu aktif)
        ├── QuestionText       TMP
        ├── AnswerBtn0         └─ AnswerText0
        ├── AnswerBtn1         └─ AnswerText1
        ├── AnswerBtn2         └─ AnswerText2
        └── AnswerBtn3         └─ AnswerText3
```

---

## TAHAP 7 — Test Core Loop

### Test Scene Game Langsung

1. Tekan **▶ Play** di Unity
2. Yang harus terjadi:
   - Rintangan merah 3D muncul dari depan setiap ~3 detik, bergerak ke arah player
   - Koin kuning 3D muncul di posisi acak kiri/tengah/kanan
   - Tekan **Spasi** atau **Klik** → player lompat (dengan physics 3D)
   - Tabrak rintangan → Game Over panel muncul + skor akhir tampil
   - Kena koin → skor +10
   - Setiap 7 detik soal baru muncul → jawab benar → +20 poin
   - Klik **ULANG** → game restart
   - Klik **MENU UTAMA** → kembali ke scene MainMenu

---

### Test Alur Lengkap MainMenu → Game

1. Stop Play. Buka scene **MainMenu**. Tekan Play
2. Klik **MULAI** → PilihLevelPanel muncul
3. Pilih level (misal: **Penjumlahan**) → scene Game ter-load
4. Soal yang muncul harus berupa penjumlahan
5. Game Over → klik MENU UTAMA → kembali ke MainMenu

---

## 🐛 Troubleshooting

| Error | Penyebab | Solusi |
|---|---|---|
| `NullReferenceException: GameManager.Instance` | Tidak ada GameManager di scene | Pastikan ada GameObject bernama `GameManager` dengan script `GameManager.cs` di scene Game |
| Player tidak bisa lompat | Ground/Rigidbody belum benar | (1) Ground punya tag "Ground". (2) Rigidbody sudah di-add. (3) Freeze Rotation XYZ sudah dicentang |
| Player tembus rintangan | Collider/Tag salah | Is Trigger ObstaclePrefab = TRUE. Tag "Obstacle" sudah di-set. PlayerController sudah di-attach ke Player |
| Soal tidak muncul | Slot MathManager kosong | Pastikan questionText, answerButtons (Size=4), answerTexts (Size=4) semua sudah di-assign |
| Game Over panel tidak muncul | Panel tidak di-assign / belum dinonaktifkan | (1) gameOverPanel sudah di-drag ke GameManager. (2) GameOverPanel harus di-uncheck/nonaktif saat start |
| Skor tidak terupdate | Komponen Text salah | Harus pakai `TextMeshProUGUI`, bukan komponen `Text` lama |
| Level tidak terbaca di Game | PlayerPrefs kosong | Pastikan `OnPilihLevel` memanggil `PlayerPrefs.SetString("Level", namaLevel)` sebelum load scene |

---

## 🚀 Pengembangan Selanjutnya

### Fitur Esensial (Disarankan)
- **🎵 Musik & SFX** — `AudioSource` + `AudioClip` untuk BGM dan efek koin/lompat/game over
- **❤️ Sistem Nyawa** — mulai dengan 3 nyawa, game over saat 0, tampilkan ikon hati di UI
- **🏆 High Score** — `PlayerPrefs.SetInt("HighScore", score)` + tampilkan di Game Over panel

### Fitur Visual 3D
- **🌈 Ground Scrolling** — beri material ground dengan texture ubin lalu animasikan `material.mainTextureOffset` di script untuk ilusi berlari
- **🏃 Animasi Player** — Animator Controller + animation clips (run, jump, idle)
- **✨ Efek Partikel** — `Particle System` untuk efek koin dikumpulkan
- **🌆 Lingkungan 3D** — tambahkan background building/pohon/objek dekoratif di sisi track

### Fitur Gameplay
- **⚡ Kecepatan Dinamis** — naikkan `speed` di `MoveObject` seiring waktu untuk kesulitan progresif
- **📱 Input Mobile** — tambahkan `Input.touchCount > 0` untuk mendukung tap di layar sentuh
- **🎯 Mode Kiri-Kanan** — tambahkan input A/D untuk player bergerak ke 3 jalur (seperti Subway Surfers)

### Fitur Teknis
- **🌊 Track Generator** — sistem tile recycling: 3 tile masing-masing 20 unit, recycle tile pertama ke depan saat sudah dilewati
- **💾 Save System** — simpan high score, level favorit, dan progress lewat PlayerPrefs
- **🎮 Cinemachine** — gunakan Cinemachine Third Person Camera untuk kamera 3D yang lebih smooth

---

## 📌 Catatan Penting

### Kenapa `Space.World` di MoveObject?
```csharp
transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
```
`Space.World` memastikan objek selalu bergerak ke arah -Z dunia, **tidak peduli rotasi objek itu sendiri**. Jika pakai `Space.Self`, objek yang sedikit miring akan bergerak ke arah yang salah.

### Kenapa `ForceMode.Impulse` untuk lompat?
```csharp
rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
```
`ForceMode.Impulse` memberikan dorongan langsung (instantaneous), bukan gaya berkelanjutan. Hasilnya: lompatan tajam dan responsif, sesuai game 3D endless runner.

### Kenapa level disimpan di PlayerPrefs?
`PlayerPrefs.SetString("Level", namaLevel)` menyimpan pilihan level lintas scene. Saat berpindah dari MainMenu ke Game, nilai ini tetap tersedia untuk dibaca `MathQuestionManager`.

---

*Tutorial ini dibuat untuk Unity 2022.3 LTS ke atas.*
*Semua script C# kompatibel dengan Unity 2021+ sampai Unity 6.*
