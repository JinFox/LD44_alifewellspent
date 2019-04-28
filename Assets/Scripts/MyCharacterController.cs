using System;
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

    [SerializeField] GameManager gm;


    Camera _cam;
    Rigidbody _rb;

    Vector3 movement;
    private bool isRunning;
    private bool dead;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetCharacter();
    }


    private void ResetCharacter ()
    {
        animator.SetBool("IsDead", false);
        animator.SetBool("IsTyping", false);
    }
    public void StopWalking()
    {
        animator.SetFloat("Speed", 0f);
        movement = Vector3.zero;
        _rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    public void UpdateCharacter()
    {
        if (GameManager.Instance.InAMinigame()) {
            animator.SetFloat("Speed", 0f);
            return ;
        }
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 right = _cam.transform.right;
        Vector3 forward = _cam.transform.forward;
        right.y = 0;
        forward.y = 0;
        right.Normalize();
        forward.Normalize();

        movement = (hori * right) + (vert * forward);


        if (Input.GetButton("Run")) {
            movement *= runSpeedMult;
            isRunning = true;
        }
        else {
            isRunning = false;
        }

        if (!gm.menuStage) _rb.velocity = movement * Time.deltaTime * speed + new Vector3(0f, _rb.velocity.y, 0f);

    }

    private void Update()
    {
        animator.SetFloat("Speed", movement.normalized.magnitude * (isRunning ? 1f : .5f));
        if (_rb.velocity.magnitude > 0.2f) {
            LookToward(movement);
        }
    }

    private void LookToward(Vector3 direction)
    {
        transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg, 0f);
    }

    internal void Die()
    {
        dead = true;

        animator.SetBool("IsDead", true);
    }

    internal void TypeOnKeyboard(Transform emitter)
    {
        if (dead)
            return;
        LookToward((emitter.position - transform.position).normalized);
        animator.SetBool("IsTyping", true);
    }
    internal void StopTyping()
    {
        animator.SetBool("IsTyping", false);
    }

    public void PickupObject()
    {
        animator.SetTrigger("PickObject");
    }
    public void ShakeHead()
    {
        animator.SetTrigger("PickObject");
    }
    private void OnValidate()
    {
        if (!_cam) {
            _cam = Camera.main;
        }
    }

}
