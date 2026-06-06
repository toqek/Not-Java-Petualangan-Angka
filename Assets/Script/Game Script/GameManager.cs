using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int GameState;

    [Header("Prefab")]
    public GameObject [] Lintasan = new GameObject [3];
    private int IndexLintasan = 0;


    [Header("Player")]
    public GameObject Player;
    public float PosisiPlayer;

    [Header("Lintasan Awal di Scene")]
    public GameObject LintasanAwal1, LintasanAwal2, LintasanAwal3;

    private List<GameObject> DaftarLintasan = new List<GameObject>();

    public float PosisiLintasanBaru = 48f*3;
    public float BatasLintasanBaru = 48f*2/3;
    public float PanjangLintasan = 48f;

   // public TextMeshPro Text;
    public TextMeshProUGUI TextSoal;
    public int JumlahJawaban = 3;
    public GameObject PrefabJawaban;
    public float JarakJawaban;
    public bool SpawnSoal;
    public int JawabanBenar;
    public float PosisiSoalBaru = 50f;
    private List<GameObject> ListJawaban = new List<GameObject>();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameState=1;
        DaftarLintasan.Add(LintasanAwal1);
        DaftarLintasan.Add(LintasanAwal2);
        DaftarLintasan.Add(LintasanAwal3);
    }

    // Update is called once per frame
    void Update()
    {

        //Jika GameState lebih dari 1 maka jalankan Game
        if (GameState == 1)
        {
            PosisiPlayer = Player.transform.position.z;

            //Buat lintasan baru jika posisi player melebihi batas lintasan yang sudah ditentukan
            if (PosisiPlayer > BatasLintasanBaru && DaftarLintasan.Count==3)
            {
               BuatJalurBaru();
               if(!SpawnSoal){
                   
               }
            }
            //Hapus Lintasan lama yang sudah dilalui jika daftar lintasan lebih dari 3
            else if (DaftarLintasan.Count>3)
            {
               HapusJalurLama();
            }

            //Buat Soal Baru
            
            if (!SpawnSoal)
            {
                BuatSoal();
                SpawnSoal=true;
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
        if(IndexLintasan==0){IndexLintasan=1;}
        else if(IndexLintasan==1){IndexLintasan=2;}
        else if(IndexLintasan==2){IndexLintasan=0;}

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

    void BuatSoal()
    {
        int a = UnityEngine.Random.Range(1,10);
        int b = UnityEngine.Random.Range(1,10);
        JawabanBenar = a+b;

        TextSoal.SetText(a+" + "+b+" = ?");
        bool jawabanUdahAda = false;
        for(int i=0; i<JumlahJawaban; i++)
        {
            int randomBenar = UnityEngine.Random.Range(0,1);

            GameObject ObjJawaban = Instantiate(PrefabJawaban,
            new Vector3(0,2,PosisiSoalBaru),Quaternion.identity);

            

            Jawaban Jawaban = ObjJawaban.GetComponent<Jawaban>();

            int c = UnityEngine.Random.Range(1,100);
            Jawaban.setText(c.ToString(),this);
            

            if (i == 1)
            {
                ObjJawaban.transform.position += Vector3.right*JarakJawaban;
                
            }
            else if (i == 2)
            {
                ObjJawaban.transform.position += Vector3.left*JarakJawaban;
            }

            if (randomBenar == 1 && !jawabanUdahAda)
            {
                Jawaban.setText(JawabanBenar.ToString(),this);
                jawabanUdahAda=true;
            }
            if (i == JumlahJawaban - 1 && jawabanUdahAda == false)
            {
                Jawaban.setText(JawabanBenar.ToString(),this);
                jawabanUdahAda=true;

            }

            ListJawaban.Add(ObjJawaban);
            

        }

        PosisiSoalBaru+=50f;

    }

    public void CheckJawaban(String nilai)
    {
        SpawnSoal = false;
        int n = int.Parse(nilai);

        if (n == JawabanBenar)
        {
            Debug.Log("Benar");
            for(int i =0; i<JumlahJawaban; i++)
            {

                GameObject Hapus = ListJawaban[i];
                ListJawaban.RemoveAt(0);
                Destroy(Hapus);
                
            }
            
        }
        else
        {
            Debug.Log("Salah");
            Player.GetComponent<PlayerController>().BisaJalan = false;
        }
    }
}
