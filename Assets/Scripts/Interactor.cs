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
    private float decayRate = 0.001f;
    private float startGreen = 1f;
    private float startRed = 0.2f;
    private float startBlue = 0.2f;

    RectTransform ButtonTrans;

    // Start is called before the first frame update
    void Start()
    {
        ButtonTrans = button as RectTransform;
        hiddenPosX = 300f; // changed from 1024f due to my screen not compatible
        visiblePosX = -220f;

        DisArm();
        Arm();
       // Invoke("Arm", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (armed)
        {
            // DecayDollar(); 
            // abandoning decay work for now. when we have the time and there are multiple dollars at the same time we may consider it.

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
        RejuvenateDollar();
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
            DOTween.Kill(ButtonTrans);
            //buttontr.localPosition = new Vector3(visiblePosX, buttontr.localPosition.y, buttontr.localPosition.z);
            //(button.transform as RectTransform).DOMoveX(visiblePosX, 0.4f); // button.transform.DOMove(200,1); // ok, no tweens for you.
            ButtonTrans.DOAnchorPosX(visiblePosX, 0.3f);

        }
        // This will end up in tears should we change resolution, but I have no time to understand the damn thing.
        // button woint appear if the plr remain in the collider. we\'ll work on it some day.
        // Debug.Log("Collided with " + other.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        // and here
        playerInRange = false;
        if (armed) ButtonBeGone();
        // Debug.Log("Collision ceased with " + other.tag);
    }

    void ButtonBeGone()
    {
        DOTween.Kill(ButtonTrans);
        ButtonTrans.DOAnchorPosX(hiddenPosX, 0.3f);
    }

    public void SetDecayRate(float dr)    {        decayRate = dr;    }
    void DecayDollar()
    {
        // here we'll be changing the dollar colour with time, based on an external variable of sorts. For now we'll hardcose it as decayRate
        Debug.Log(dollar.GetComponent<Renderer>().material.GetColor("_Color"));
        Color c = dollar.GetComponent<Renderer>().material.GetColor("_Color");
        dollar.GetComponent<Renderer>().material.SetColor("_Color",c - new Color(decayRate * startRed, decayRate * startGreen, decayRate * startBlue, 0f));
        if (c == new Color(0f, 0f, 0f)) DollarLost();
    }

    void RejuvenateDollar()
    {
        dollar.GetComponent<Renderer>().material.color = new Color(startRed, startGreen, startBlue,1f);
        // here we restore the dollar to its beautiful green.
    }

    void DollarLost()
    {
        // if dollar is decayed without collecting this happens.
        DisArm();
    }
}
