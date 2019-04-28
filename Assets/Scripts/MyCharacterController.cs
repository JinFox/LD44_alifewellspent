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
        //StopMovementSound();
        _rb.velocity = Vector3.zero;
    }

    void PlayFootStepSound()
    {
        if (isRunning) {
            AudioManager.Instance.Stop("Walk");
            if (!AudioManager.Instance.isPlaying("Run")) {
                AudioManager.Instance.Play("Run");
            }

        } else {
            AudioManager.Instance.Stop("Run");
            if (!AudioManager.Instance.isPlaying("Walk")) {
                AudioManager.Instance.Play("Walk");
            }

        }
    }

    // Update is called once per frame
    public void UpdateCharacter()
    {
        if (GameManager.Instance.InAMinigame() || GameManager.Instance.menuStage) {
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


        if (!Input.GetButton("Run")) {
            movement *= runSpeedMult;
            isRunning = true;
        }
        else {
            isRunning = false;
        }

        _rb.velocity = movement * Time.deltaTime * speed + new Vector3(0f, _rb.velocity.y, 0f);

    }

    private void Update()
    {
        animator.SetFloat("Speed", movement.normalized.magnitude * (isRunning ? 1f : .5f));
        if (_rb.velocity.magnitude > 0.2f) {
            LookToward(movement);
            
        }
        if (movement.sqrMagnitude > 0.01f)
            PlayFootStepSound();
        else {
            StopMovementSound();
        }
    }

    private void StopMovementSound()
    {
        AudioManager.Instance.Stop("Walk");
        AudioManager.Instance.Stop("Run");
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
        StopMovementSound();
    }
    internal void StopTyping()
    {
        animator.SetBool("IsTyping", false);
    }

    public void PickupObject()
    {
        animator.SetTrigger("PickObject");
        StopMovementSound();
    }
    public void ShakeHead()
    {
        animator.SetTrigger("PickObject");
        StopMovementSound();
    }
    private void OnValidate()
    {
        if (!_cam) {
            _cam = Camera.main;
        }
    }

}
