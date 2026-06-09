using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Referensi UI Canvas")]
    public GameObject canvasMenu;
    public TextMeshProUGUI teksJudulMenu;
    public GameObject tombolKembali;

    [Header("Tombol Pause (pojok kanan atas)")]
    public GameObject tombolPause;       // Tarik objek tombol pause ke sini

    [Header("Referensi Player")]
    private PlayerController playerController;

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();

        if (canvasMenu != null) canvasMenu.SetActive(false);

        // Pastikan tombol pause muncul saat game dimulai
        if (tombolPause != null) tombolPause.SetActive(true);

        Time.timeScale = 1f;
    }

    // ==========================================
    // 1. PAUSE
    // ==========================================
    public void TampilkanMenuPause()
    {
        PlayClickSound();

        if (canvasMenu != null) canvasMenu.SetActive(true);
        if (teksJudulMenu != null) teksJudulMenu.text = "PAUSE";
        if (tombolKembali != null) tombolKembali.SetActive(true);

        // Tombol pause tetap tersembunyi selama menu pause terbuka
        if (tombolPause != null) tombolPause.SetActive(false);

        HentikanGame();
    }

    // ==========================================
    // 2. GAME OVER
    // ==========================================
    public void TampilkanMenuGameOver(int poinAkhir)
    {
        if (canvasMenu != null) canvasMenu.SetActive(true);
        if (teksJudulMenu != null) teksJudulMenu.SetText("GAME OVER\n" + poinAkhir);
        if (tombolKembali != null) tombolKembali.SetActive(false);

        // Sembunyikan tombol pause saat game over agar tidak bisa ditekan
        if (tombolPause != null) tombolPause.SetActive(false);

        HentikanGame();
    }

    private void HentikanGame()
    {
        if (playerController != null) playerController.BisaJalan = false;
        Time.timeScale = 0f;
    }

    // ==========================================
    // SEMBUNYIKAN MENU (Resume dari Pause)
    // ==========================================
    public void SembunyikanMenu()
    {
        if (canvasMenu != null) canvasMenu.SetActive(false);

        // Tampilkan kembali tombol pause saat resume
        if (tombolPause != null) tombolPause.SetActive(true);

        Time.timeScale = 1f;
        StartCoroutine(AktifkanGerakanPlayerDenganJeda());
    }

    private IEnumerator AktifkanGerakanPlayerDenganJeda()
    {
        yield return new WaitForEndOfFrame();
        if (playerController != null) playerController.BisaJalan = true;
    }

    // ==========================================
    // ACTION BUTTONS
    // ==========================================
    public void TombolKembali()
    {
        PlayClickSound();
        SembunyikanMenu();
    }

    public void TombolUlang()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TombolKeluar()
    {
        PlayClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private void PlayClickSound()
    {
        AudioSource audioInternal = GetComponent<AudioSource>();
        if (audioInternal != null && audioInternal.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioInternal.clip, transform.position);
        }
    }
}
