using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    [SerializeField]
    float speed = 100f;
    float runSpeedMult = 2f;

    [SerializeField]
    Animator animator;

    Camera _cam;
    Rigidbody _rb;

    Vector3 movement;
    private bool isRunning;

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
        if (!GameManager.Instance.InAMinigame()) {
            _rb.velocity = movement * Time.deltaTime * speed + new Vector3(0f, _rb.velocity.y, 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InAMinigame()) {
            return ;
        }
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
        movement = (hori * right) + (vert * forward);


        if (Input.GetButton("Run")) {
            movement *= runSpeedMult;
            isRunning = true;
        }
        else {
            isRunning = false;
        }


        if (Mathf.Abs(hori) + Mathf.Abs(vert) > 0.02f)
            transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(movement.z, movement.x) * Mathf.Rad2Deg, 0f);

        animator.SetFloat("Speed", movement.normalized.magnitude * (isRunning? 1f : .5f));
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
