using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] bool armed;
    [SerializeField] Transform dollar;
    [SerializeField] Transform button;


    // Start is called before the first frame update
    void Start()
    {
        DisArm();
    }

    // Update is called once per frame
    void Update()
    {
        if (armed)
        {
            dollar.transform.Rotate(Vector3.up, 2f);
            // detect collision with the plr
            // enable interaction button

        }
    }

    public void Arm()
    {
        armed = true;
        dollar.GetComponent<MeshRenderer>().enabled = true;
    }

    public void DisArm()
    {
        armed = false;
        dollar.GetComponent<MeshRenderer>().enabled = false;
    }
}
