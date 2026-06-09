using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerController : MonoBehaviour
{
    [Header("Pergerakan Karakter")]
    public float KecepatanMaju;
    public float KecepatanPindahJalur = 10f;

    private Vector2 startPos;
    private bool isDragging = false;

    private int currentLane = 1;
    private float[] laneX = { -3.7f, 0f, 3.7f };

    public bool BisaJalan = true;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (!BisaJalan)
            return;

        transform.position += Vector3.forward * KecepatanMaju * Time.deltaTime;

        HandleSwipe();
        HandleMouseSwipe();

        Vector3 targetPosition = new Vector3(
            laneX[currentLane],
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            KecepatanPindahJalur * Time.deltaTime
        );
    }

    // ── Touch (mobile) ───────────────────────────────────────────────────
    void HandleSwipe()
    {
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
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

    // ── Mouse (PC) — New Input System ───────────────────────────────────
    void HandleMouseSwipe()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            startPos = mouse.position.ReadValue();
            isDragging = true;
        }
        else if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            float deltaX = mouse.position.ReadValue().x - startPos.x;

            if (deltaX > 50)
                SwipeRight();
            else if (deltaX < -50)
                SwipeLeft();
        }
    }

    void SwipeLeft()
    {
        if (currentLane > 0)
            currentLane--;

        PlayAudiouSwipe();
    }

    void SwipeRight()
    {
        if (currentLane < 2)
            currentLane++;

        PlayAudiouSwipe();
    }

    void PlayAudiouSwipe()
    {
        AudioSource audioInternal = GetComponent<AudioSource>();
        if (audioInternal != null && audioInternal.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioInternal.clip, transform.position);
        }
    }
}
