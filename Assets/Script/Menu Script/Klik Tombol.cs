using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KlikTombol : MonoBehaviour
{

    [SerializeField]
    Canvas CanvasMenu, CanvasTentang, CanvasLevel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanvasMenu.enabled= true;
        CanvasTentang.enabled = false;
        CanvasLevel.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Untuk keluar dari aplikasi
    public void Keluar()
    {
        PlayClickSound();
        //Debug.Log("Keluar");

        //keluar aplikasi
        Application.Quit();
    }

    //Untuk masuk ke menu tentang
    public void Tentang()
    {
        PlayClickSound();
        //Debug.Log("Tentang");
        
        //matikan canvas main menu
        this.CanvasMenu.enabled=false;
        //nyalakan canvas tentang
        this.CanvasTentang.enabled=true;
    }

    //Untuk masuk ke pilih level
    public void Mulai()
    {
        PlayClickSound();
       // Debug.Log("Pilih Level");
        
        //matikan canvas main menu
        this.CanvasMenu.enabled=false;
        //nyalakan canvas level
        this.CanvasLevel.enabled=true;

    }

    //Untuk kembali ke menu sebelumnya dengan parameter nama menu yang aktif
    public void KembaliKeMenu(string canvas)
    {
        PlayClickSound();

        //jika sekarang berada di menu tentang
        if (canvas == "Tentang")
        {
            //maka matikan canvas tentang
            this.CanvasTentang.enabled = false;
            //nyalakan canvas menu
            this.CanvasMenu.enabled = true;

        } 
        //jika sekarang berada di canvas level
        else if (canvas == "Level")
        {
            //matikan canvas level
            this.CanvasLevel.enabled = false;
            //nyalakan canvas menu
            this.CanvasMenu.enabled = true;
        }

    }

    public void PindahScene(string scene)
    {
        PlayClickSound();
        SceneManager.LoadScene(scene);
        
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
