using Unity.VisualScripting;
using UnityEngine;
// 1. Tambahkan namespace New Input System
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch; // Menghindari bentrok dengan Touch lama

public class PlayerController : MonoBehaviour
{
    [Header("Pergerakan Karakter")]
    public float KecepatanMaju;
    public float KecepatanPindahJalur = 10f;

    private Vector2 startPos;

    // 0 = kiri, 1 = tengah, 2 = kanan
    private int currentLane = 1;

    // Posisi X setiap jalur
    private float[] laneX = { -3.7f, 0f, 3.7f };

    public bool BisaJalan = true;

    // 2. Wajib aktifkan EnhancedTouch saat objek aktif
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    // 3. Matikan EnhancedTouch saat objek tidak aktif
    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        // Bola terus maju
        if(!BisaJalan)
            return;
            
        transform.position += Vector3.forward * KecepatanMaju * Time.deltaTime;

        HandleSwipe();

        // Target posisi sesuai jalur
        Vector3 targetPosition = new Vector3(
            laneX[currentLane],
            transform.position.y,
            transform.position.z
        );

        // Pindah jalur dengan halus
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            KecepatanPindahJalur * Time.deltaTime
        );
    }

    void HandleSwipe()
    {
        // 4. Gunakan Touch.activeTouches milik New Input System
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                // Ambil posisi awal dari screenPosition
                startPos = touch.screenPosition;
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                float deltaX = touch.screenPosition.x - startPos.x;      

                if (deltaX > 50)
                    SwipeRight();

                else if (deltaX < -50)
                    SwipeLeft();

            }
        }
    }

    void SwipeLeft()
    {
        if (currentLane > 0)
            currentLane--;

        this.PlayAudiouSwipe();
    }

    void SwipeRight()
    {
        if (currentLane < 2)
            currentLane++;

        this.PlayAudiouSwipe();
    }

    void PlayAudiouSwipe()
    {
        //play audio swipe
        AudioSource audioInternal = GetComponent<AudioSource>();
        if (audioInternal != null && audioInternal.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioInternal.clip, transform.position);
        }
    }
}