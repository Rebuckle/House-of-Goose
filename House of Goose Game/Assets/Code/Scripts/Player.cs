using UnityEditor;
using UnityEngine;

public class Player : HoG
{

    bool inPhoneCall = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            
        }
    }

    void Interact()
    {
        if (inPhoneCall)
        {
            
        }
    }
}
