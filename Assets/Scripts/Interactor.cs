using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Interactor : MonoBehaviour
{
    private float hiddenPosX;
    private float visiblePosX;
    public float rotatingSpeed = 30f;

    
    [SerializeField] bool armed;
    [SerializeField] Transform dollar;
    [SerializeField] Transform button;
    [SerializeField] Minigame linkedGame;
    private bool playerInRange = false;
    [SerializeField] ParticleSystem burst;
    // [SerializeField] GameManager gm; // I don't need to do this. GM is Everywhere!


    // Start is called before the first frame update
    void Start()
    {

        hiddenPosX = 512f + 100f;
        visiblePosX = hiddenPosX - 300f;

        DisArm();
        Arm();
       // Invoke("Arm", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (armed)
        {
            dollar.transform.Rotate(Vector3.up, -rotatingSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space)) ButtonMashed();

        }
    }

    public void ButtonMashed() // call this by the button when it's mashed on the screen interface.
    {
        if (armed && playerInRange)
        {
            // add money
            // take life
            DisArm();
            burst.gameObject.SetActive(true);
            burst.Clear();
            burst.Play();
            linkedGame.LaunchMinigame();
            StartCoroutine("StopBurst");

            ButtonBeGone();
           // Invoke("Arm", 5f);
        }
    }
    IEnumerator StopBurst()
    {

        yield return new WaitForSeconds(.1f);
        burst.Stop();
        
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

    private void OnTriggerEnter(Collider other)
    {
        // I should be checking player's tag here.
        playerInRange = true;

        if (armed) {
            RectTransform buttontr = (button.transform as RectTransform);
            //buttontr.localPosition = new Vector3(visiblePosX, buttontr.localPosition.y, buttontr.localPosition.z);
            (button.transform as RectTransform).DOLocalMoveX(visiblePosX, 0.4f); // button.transform.DOMove(200,1); // ok, no tweens for you.
        }
        // This will end up in tears should we change resolution, but I have no time to understand the damn thing.
        // button woint appear if the plr remain in the collider. we\'ll work on it some day.
        Debug.Log("Collided with " + other.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        // and here
        playerInRange = false;
        if (armed) ButtonBeGone();
        Debug.Log("Collision ceased with " + other.tag);
    }

    void ButtonBeGone()
    {
        DOTween.Kill(button.transform);
        (button.transform as RectTransform).DOLocalMoveX(hiddenPosX, 0.4f); // yeehaw!

    }
}
