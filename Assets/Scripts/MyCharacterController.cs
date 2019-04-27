using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [SerializeField]
    float speed;

    Camera _cam;
    Rigidbody _rb;
    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {

        Vector3 right = _cam.transform.right;
         right.y = 0;
        right.Normalize();


        Vector3 forward = _cam.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Debug.DrawLine(transform.position, transform.position + right, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + forward, Color.blue);

        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 movement = (hori * right) + (vert * forward);
        transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(movement.z, movement.x) * Mathf.Rad2Deg, 0f);
        _rb.velocity = movement * Time.deltaTime * speed + new Vector3(0f, _rb.velocity.y, 0f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (!_cam) {
            _cam = Camera.main;
        }
    }
    private void OnDrawGizmos()
    {

        Vector3 right = _cam.transform.right;
        right.y = 0;
        right.Normalize();


        Vector3 forward = _cam.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Gizmos.DrawLine(transform.position, transform.position + right);
        Gizmos.DrawLine(transform.position, transform.position + forward);

    }
}
