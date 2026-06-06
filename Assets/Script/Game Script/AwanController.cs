using UnityEngine;

public class AwanController : MonoBehaviour
{
    public float KecepatanAwan=2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(KecepatanAwan*Time.deltaTime,0,0);
    }
}
