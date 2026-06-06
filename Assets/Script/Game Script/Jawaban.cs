using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Jawaban : MonoBehaviour
{
    public TextMeshPro TMPJawaban;
    private GameManager GM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string Jawaban, GameManager GM)
    {
        this.GM = GM;
        TMPJawaban.SetText(Jawaban);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(TMPJawaban.text);
            //GM.SpawnSoal=false;
            GM.CheckJawaban(TMPJawaban.text);
        }

        
    }
}
