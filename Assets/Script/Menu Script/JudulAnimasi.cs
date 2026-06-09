using System.Collections;
using UnityEngine;

public class JudulAnimasi : MonoBehaviour
{
    [Header("Animasi Masuk")]
    public float TundaAwal = 0.3f;
    public float DurasiMasuk = 0.8f;

    [Header("Animasi Mengambang")]
    public float TinggiAmbang = 15f;
    public float KecepatanAmbang = 1.2f;

    [Header("Animasi Skala Napas")]
    public bool AktifkanNapas = true;
    public float SkalaMin = 0.97f;
    public float SkalaMax = 1.03f;
    public float KecepatanNapas = 1.0f;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector2 posisiAwal;         // pakai Vector2 karena RectTransform
    private Vector3 skalaAwal;
    private float waktu = 0f;
    private bool sudahMasuk = false;
    private bool sedangMasuk = false;
    private float waktuMasuk = 0f;

    // Batas ambang atas agar tidak melewati parent
    private float batasAtas;
    private float batasBawah;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        skalaAwal = transform.localScale;
    }

    void OnEnable()
    {
        sudahMasuk = false;
        sedangMasuk = false;
        waktu = 0f;
        waktuMasuk = 0f;

        StartCoroutine(InisialisasiLaluMasuk());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        sedangMasuk = false;
    }

    IEnumerator InisialisasiLaluMasuk()
    {
        // Tunggu 1 frame agar Canvas/Layout selesai hitung posisi
        yield return null;

        // Simpan posisi SETELAH layout selesai
        posisiAwal = rectTransform.anchoredPosition;

        // Hitung batas dari parent agar judul tidak melayang keluar
        parentRect = transform.parent.GetComponent<RectTransform>();
        if (parentRect != null)
        {
            float tinggiParent = parentRect.rect.height;
            float tinggiSelf   = rectTransform.rect.height * skalaAwal.y;

            // Pivot default 0.5 — posisi anchor di tengah objek
            float pivot = rectTransform.pivot.y;
            float tepiAtas   =  (tinggiParent * (1f - rectTransform.anchorMin.y)) - (tinggiSelf * (1f - pivot));
            float tepiBawah  = -(tinggiParent * rectTransform.anchorMin.y)        + (tinggiSelf * pivot);

            // Batas gerak mengambang: posisiAwal ± TinggiAmbang, tidak boleh lewat tepi parent
            batasAtas  = Mathf.Min(posisiAwal.y + TinggiAmbang,  tepiAtas);
            batasBawah = Mathf.Max(posisiAwal.y - TinggiAmbang, tepiBawah);

            // Recalculate TinggiAmbang efektif berdasarkan batas nyata
            TinggiAmbang = Mathf.Min(TinggiAmbang,
                Mathf.Min(batasAtas - posisiAwal.y, posisiAwal.y - batasBawah));
        }

        // Reset tersembunyi
        SetAlpha(0f);
        rectTransform.anchoredPosition = posisiAwal + Vector2.down * 40f;
        transform.localScale = skalaAwal * 0.85f;

        yield return new WaitForSecondsRealtime(TundaAwal);
        sedangMasuk = true;
        waktuMasuk = 0f;
    }

    void Update()
    {
        if (sedangMasuk)
        {
            waktuMasuk += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(waktuMasuk / DurasiMasuk);
            float eased = EaseOutBack(t);

            rectTransform.anchoredPosition = Vector2.Lerp(
                posisiAwal + Vector2.down * 40f,
                posisiAwal,
                eased
            );
            transform.localScale = Vector3.Lerp(skalaAwal * 0.85f, skalaAwal, eased);
            SetAlpha(Mathf.Clamp01(t * 2f));

            if (t >= 1f)
            {
                sedangMasuk = false;
                sudahMasuk = true;
                rectTransform.anchoredPosition = posisiAwal;
                transform.localScale = skalaAwal;
                SetAlpha(1f);
            }
            return;
        }

        if (!sudahMasuk) return;

        waktu += Time.unscaledDeltaTime;

        // Mengambang naik turun — dikunci dalam batas parent
        float offsetY = Mathf.Sin(waktu * KecepatanAmbang * Mathf.PI) * TinggiAmbang;
        float posY = Mathf.Clamp(posisiAwal.y + offsetY, batasBawah, batasAtas);
        rectTransform.anchoredPosition = new Vector2(posisiAwal.x, posY);

        // Napas skala
        if (AktifkanNapas)
        {
            float skalaFaktor = Mathf.Lerp(
                SkalaMin, SkalaMax,
                (Mathf.Sin(waktu * KecepatanNapas * Mathf.PI) + 1f) * 0.5f
            );
            transform.localScale = skalaAwal * skalaFaktor;
        }
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    void SetAlpha(float alpha)
    {
        foreach (var cr in GetComponentsInChildren<CanvasRenderer>())
            cr.SetAlpha(alpha);
    }
}
