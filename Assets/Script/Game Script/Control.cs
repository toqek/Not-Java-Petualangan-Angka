using UnityEngine;

public class Control : MonoBehaviour
{

    public float KecepatanPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0,0,KecepatanPlayer);
        
    }
}
