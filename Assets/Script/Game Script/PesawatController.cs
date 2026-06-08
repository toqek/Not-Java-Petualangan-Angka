using UnityEngine;

public class PesawatController : MonoBehaviour
{
    public float KecepatanPesawat=0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0,0,KecepatanPesawat*Time.deltaTime);
    }
}
