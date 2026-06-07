using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // Wajib ditambahkan untuk mengontrol TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Referensi UI Canvas")]
    public GameObject canvasMenu;        // Tarik objek CanvasMenu ke sini
    public TextMeshProUGUI teksJudulMenu; // Tarik objek komponen Text (Judul) ke sini
    public GameObject tombolKembali;     // Tarik objek Tombol Kembali ke sini

    [Header("Referensi Player")]
    private PlayerController playerController;

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();

        // Sembunyikan canvas di awal game
        if (canvasMenu != null)
        {
            canvasMenu.SetActive(false);
        }
        
        Time.timeScale = 1f; 
    }

    // ==========================================
    // 1. FUNGSI KHUSUS TOMBOL PAUSE (DI ATAS LAYAR)
    // ==========================================
    public void TampilkanMenuPause()
    {
        if (canvasMenu != null) canvasMenu.SetActive(true);
        
        // Atur teks judul menjadi PAUSE
        if (teksJudulMenu != null) teksJudulMenu.text = "PAUSE";
        
        // Tombol kembali HARUS MUNCUL saat pause
        if (tombolKembali != null) tombolKembali.SetActive(true);

        HentikanGame();
    }

    // ==========================================
    // 2. FUNGSI KHUSUS SAAT KALAH / MATI / GAME OVER
    // ==========================================
    // Fungsi ini menerima parameter 'poinAkhir' dari script yang membuat player mati
    public void TampilkanMenuGameOver(int poinAkhir)
    {
        if (canvasMenu != null) canvasMenu.SetActive(true);
        
        // Ubah teks menjadi "Poin Kamu : [Angka Poin]"
        if (teksJudulMenu != null) teksJudulMenu.SetText("GAME OVER\n" + poinAkhir);
        
        // Tombol kembali HARUS SEMBUNYI saat game over
        if (tombolKembali != null) tombolKembali.SetActive(false);

        HentikanGame();
    }

    private void HentikanGame()
    {
        if (playerController != null) playerController.BisaJalan = false;
        Time.timeScale = 0f; 
    }

   public void SembunyikanMenu()
    {
        if (canvasMenu != null)
        {
            canvasMenu.SetActive(false); // Sembunyikan Canvas Menu
        }

        // Kembalikan waktu game menjadi normal berjalan
        Time.timeScale = 1f; 

        // PERBAIKAN: Gunakan Coroutine untuk mengaktifkan pergerakan bola dengan jeda aman
        StartCoroutine(AktifkanGerakanPlayerDenganJeda());
    }

    // Coroutine khusus untuk memberi jeda agar input tombol tidak terbaca sebagai swipe
    private IEnumerator AktifkanGerakanPlayerDenganJeda()
    {
        // Tunggu sampai akhir frame ini selesai diproses (menghapus sisa input sentuhan)
        yield return new WaitForEndOfFrame();
        
        // Baru setelah itu aman untuk mengizinkan player bergerak lagi
        if (playerController != null)
        {
            playerController.BisaJalan = true;
        }
    }

    // ==========================================
    // ACTION BUTTONS
    // ==========================================
    public void TombolKembali()
    {
        SembunyikanMenu();
    }

    public void TombolUlang()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TombolKeluar()
    {
        Debug.Log("Game Ditutup!");
       // Application.Quit();
       SceneManager.LoadScene("Main Menu");
    }
}