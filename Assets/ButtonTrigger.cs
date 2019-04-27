using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public bool buttonMashed = false;
    // for the [space] button management
    public void ButtonMashed() { buttonMashed = true;  }
    public void UnmashButton() { buttonMashed = false; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (buttonMashed && other.CompareTag("interactor"))
        {
            Debug.Log("Player pressed the space button while colliding with stuff");
            other.GetComponent<Interactor>().ButtonMashed();
            UnmashButton();
        } 
    }
}
