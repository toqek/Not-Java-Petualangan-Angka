using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TombolAnimasi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animasi Masuk")]
    public float TundaAwal = 0.1f;
    public float DurasiMasuk = 0.6f;

    [Header("Animasi Hover")]
    public float SkalaHover = 1.08f;
    public float DurasiHover = 0.15f;

    [Header("Animasi Tekan")]
    public float SkalaTekan = 0.93f;
    public float DurasiTekan = 0.08f;

    [Header("Animasi Idle (napas)")]
    public bool AktifkanNapas = true;
    public float SkalaMin = 0.98f;
    public float SkalaMax = 1.02f;
    public float KecepatanNapas = 0.9f;

    private Vector3 skalaAwal;
    private bool sudahMasuk = false;
    private bool sedangHover = false;
    private Coroutine coroutineAktif;
    private float waktuNapas = 0f;

    void Awake()
    {
        // Simpan skala asli SEBELUM apapun diubah
        skalaAwal = transform.localScale;
    }

    void OnEnable()
    {
        // OnEnable dipanggil setiap kali object aktif, termasuk saat scene di-reload
        // Lebih reliable daripada Start() untuk animasi yang perlu reset
        // skalaAwal sudah disimpan di Awake()
        sudahMasuk = false;
        sedangHover = false;
        coroutineAktif = null;

        transform.localScale = Vector3.zero;
        SetAlpha(0f);

        StartCoroutine(TungguLaluMasuk());
    }

    void OnDisable()
    {
        // Hentikan semua coroutine saat object dinonaktifkan
        StopAllCoroutines();
        coroutineAktif = null;
    }

    IEnumerator TungguLaluMasuk()
    {
        yield return new WaitForSecondsRealtime(TundaAwal);
        StartCoroutine(AnimasiMasuk());
    }

    IEnumerator AnimasiMasuk()
    {
        float t = 0f;
        while (t < DurasiMasuk)
        {
            t += Time.unscaledDeltaTime; // unscaled agar tidak terpengaruh timeScale
            float progress = Mathf.Clamp01(t / DurasiMasuk);
            float eased = EaseOutBack(progress);

            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, skalaAwal, eased);
            SetAlpha(Mathf.Clamp01(progress * 2f));
            yield return null;
        }

        transform.localScale = skalaAwal;
        SetAlpha(1f);
        sudahMasuk = true;
        waktuNapas = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        if (!sudahMasuk || sedangHover || coroutineAktif != null) return;
        if (!AktifkanNapas) return;

        waktuNapas += Time.unscaledDeltaTime * KecepatanNapas;
        float skalaFaktor = Mathf.Lerp(SkalaMin, SkalaMax, (Mathf.Sin(waktuNapas * Mathf.PI) + 1f) * 0.5f);
        transform.localScale = skalaAwal * skalaFaktor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!sudahMasuk) return;
        sedangHover = true;
        JalankanAnimasiSkala(skalaAwal * SkalaHover, DurasiHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!sudahMasuk) return;
        sedangHover = false;
        JalankanAnimasiSkala(skalaAwal, DurasiHover);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!sudahMasuk) return;
        JalankanAnimasiSkala(skalaAwal * SkalaTekan, DurasiTekan);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!sudahMasuk) return;
        Vector3 targetSkala = sedangHover ? skalaAwal * SkalaHover : skalaAwal;
        JalankanAnimasiSkala(targetSkala, DurasiTekan);
    }

    void JalankanAnimasiSkala(Vector3 target, float durasi)
    {
        if (coroutineAktif != null) StopCoroutine(coroutineAktif);
        coroutineAktif = StartCoroutine(AnimasiSkala(target, durasi));
    }

    IEnumerator AnimasiSkala(Vector3 target, float durasi)
    {
        Vector3 dari = transform.localScale;
        float t = 0f;
        while (t < durasi)
        {
            t += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(t / durasi);
            transform.localScale = Vector3.Lerp(dari, target, EaseOutQuad(progress));
            yield return null;
        }
        transform.localScale = target;
        coroutineAktif = null;
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    void SetAlpha(float alpha)
    {
        foreach (var cr in GetComponentsInChildren<CanvasRenderer>())
            cr.SetAlpha(alpha);
    }
}
