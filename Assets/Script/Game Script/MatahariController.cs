using UnityEngine;

public class MatahariController : MonoBehaviour
{
    public float KecepatanRotasi=0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(KecepatanRotasi*Time.deltaTime,0,0);
    }
}
