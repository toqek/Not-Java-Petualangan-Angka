using UnityEngine;

public class Batu : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // Otomatis mencari GameManager di scene
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Jika bola (Player) menabrak fisik batu
        AksiTabrakan(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Antisipasi jika Collider batu Anda diatur sebagai Is Trigger
        AksiTabrakan(other.gameObject);
    }

    void AksiTabrakan(GameObject objekPenabrak)
    {
        if (objekPenabrak.CompareTag("Player") && gameManager != null)
        {
            Debug.Log("Player Menabrak Batu! Game Over.");
            gameManager.PlayerKalah(); // Panggil fungsi kalah di GameManager
        }
    }
}