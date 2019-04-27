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
        animator.SetBool("isDead", false);
        animator.SetBool("isTyping", false);
    }

    // Update is called once per frame
    public void UpdateCharacter()
    {
        if (GameManager.Instance.InAMinigame()) {
            animator.SetFloat("Speed", 0f);
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

        _rb.velocity = movement * Time.deltaTime * speed + new Vector3(0f, _rb.velocity.y, 0f);

        if (Mathf.Abs(hori) + Mathf.Abs(vert) > 0.08f)
            LookToward(movement);
        animator.SetFloat("Speed", movement.normalized.magnitude * (isRunning ? 1f : .5f));

    }

    private void LookToward(Vector3 direction)
    {
        transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg, 0f);
    }

    internal void Die()
    {
        dead = true;

        animator.SetBool("isDead", true);
    }

    internal void TypeOnKeyboard(Transform emitter)
    {
        if (dead)
            return;
        LookToward((emitter.position - transform.position).normalized);
        animator.SetBool("isTyping", true);
    }
    internal void StopTyping()
    {
        animator.SetBool("isTyping", false);
    }

    private void OnValidate()
    {
        if (!_cam) {
            _cam = Camera.main;
        }
    }

}
