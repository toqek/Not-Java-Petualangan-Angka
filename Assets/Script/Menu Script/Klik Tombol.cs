using Unity.VisualScripting;
using UnityEngine;

public class KlikTombol : MonoBehaviour
{


    [SerializeField]
    Canvas CanvasMenu, CanvasTentang, CanvasLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Untuk keluar dari aplikasi
    public void Keluar()
    {
        Debug.Log("Keluar");

        //keluar aplikasi
        Application.Quit();
    }

    //Untuk masuk ke menu tentang
    public void Tentang()
    {
        Debug.Log("Tentang");
        
        //matikan canvas main menu
        this.CanvasMenu.enabled=false;
        //nyalakan canvas tentang
        this.CanvasTentang.enabled=true;
    }

    //Untuk masuk ke pilih level
    public void Mulai()
    {
        Debug.Log("Pilih Level");
        
        //matikan canvas main menu
        this.CanvasMenu.enabled=false;
        //nyalakan canvas level
        this.CanvasLevel.enabled=true;

    }

    //Untuk kembali ke menu sebelumnya dengan parameter nama menu yang aktif
    public void KembaliKeMenu(string canvas)
    {
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
    
        
    
    
}
