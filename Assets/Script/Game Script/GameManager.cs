using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int GameState;

    [Header("Prefab")]
    public GameObject [] Lintasan = new GameObject [2];
    private int IndexLintasan = 0;


    [Header("Player")]
    public GameObject Player;
    public float PosisiPlayer;

    [Header("Lintasan Awal di Scene")]
    public GameObject LintasanAwal1;
    public GameObject LintasanAwal2;

    private List<GameObject> DaftarLintasan = new List<GameObject>();

    public float PosisiLintasanBaru = 48f*2;
    public float BatasLintasanBaru = 48f*2/3;
    public float PanjangLintasan = 48f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameState=1;
        DaftarLintasan.Add(LintasanAwal1);
        DaftarLintasan.Add(LintasanAwal2);
    }

    // Update is called once per frame
    void Update()
    {

        //Jika GameState lebih dari 1 maka jalankan Game
        if (GameState == 1)
        {
            PosisiPlayer = Player.transform.position.z;

            //Buat lintasan baru jika posisi player melebihi batas lintasan yang sudah ditentukan
            if (PosisiPlayer > BatasLintasanBaru)
            {
               BuatJalurBaru();
            }
            //Hapus Lintasan lama yang sudah dilalui jika daftar lintasan lebih dari 3
            else if (DaftarLintasan.Count>3)
            {
               HapusJalurLama();
            }



        } 
        //Jika Gamestate == 2 Maka Pause Game Karena Menekan tombol pause
        else if (GameState == 2)
        {
            
        }

        //Jika Gamestate==3 Maka Pause Game Karena Mati
        else if (GameState==3)
        {
            
        }
        
    }

    void BuatJalurBaru()
    {
        //Membuat lintasan baru di titik z = posisi lintasan baru
        GameObject LintasanBaru = Instantiate(Lintasan[IndexLintasan],
        new Vector3(0,0,PosisiLintasanBaru),Quaternion.identity);

        //tambahkan lintasan yang baru saja dbuat ke dalam daftar lintasan
        DaftarLintasan.Add(LintasanBaru);
        //update index Lintasan agar menggunakan prefab lintasan yang lain
        IndexLintasan = (IndexLintasan==0)?1:0;

        //update posisi lintasan baru
        PosisiLintasanBaru += PanjangLintasan;
        //Update batas lintasan baru
        BatasLintasanBaru += (PanjangLintasan*2/3);
    }

    void HapusJalurLama()
    {
        GameObject LintasanLama = DaftarLintasan[0];

        if (PosisiPlayer > LintasanLama.transform.position.z+PanjangLintasan)
        {
            DaftarLintasan.RemoveAt(0);
            Destroy(LintasanLama);
        }
    }
}
