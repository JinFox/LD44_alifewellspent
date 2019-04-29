using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdWatchingMinigame : Minigame
{
    public Text timerUI;

    public float startTimer = 30f;
    float timer;
    [SerializeField] Cinemachine.CinemachineVirtualCamera mainCam;
    // [SerializeField] Camera bwCam;
    [SerializeField] Cinemachine.CinemachineVirtualCamera bwCam;
    [SerializeField] Transform sky0;
    [SerializeField] Transform sky1;
    [SerializeField] Transform birds;
    private Animation anim;
    private int numberToPick;

    public GameReward foundABirdReward;
    void UpdateTimerUI()
    {
        timerUI.text = String.Format("{0:0.#}s remaining to enjoy view", timer);
    }

    public override void LaunchMinigame()
    {
        anim = birds.GetComponent<Animation>();
        anim.Play();
        base.LaunchMinigame(); // this stays
        // INITIALISATION
        GameReward rew = TheReward;

        numberToPick = 0;
        timer = startTimer;
        MyCharacterController p = GameManager.Instance.thePlayer;
        p.StopWalking();
        p.PickupObject();
        mainCam.enabled = false;
        bwCam.enabled = true;
        sky0.gameObject.SetActive(false);
        sky1.gameObject.SetActive(true);
        birds.gameObject.SetActive(true);
        foreach (Transform child in birds.transform) {
            child.gameObject.SetActive(true);
            numberToPick++;
        }
    }

    // called every frame
    public override void updateMinigame()
    {
        timer -= Time.deltaTime;
        // Debug.DrawRay(Camera.main.transform.position, Input.mousePosition, Color.green, 1);
        UpdateTimerUI();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 20.0f))
        {
               // Debug.Log("hit " + hit.transform.name);
                // hit.transform.position += Vector3.right * speed * time.deltaTime; // << declare public speed and set it in inspector
         if (hit.transform.CompareTag("bird"))
            {
                AudioManager.Instance.Play("Success");
                TheReward.enjoyment += foundABirdReward.enjoyment;
                TheReward.profit += foundABirdReward.profit;
                TheReward.age += foundABirdReward.age;
                hit.transform.gameObject.SetActive(false);
                numberToPick--;
            }
                
        }
        
        // this minigame dont allow walking so we don't update 
        // GameManager.Instance.thePlayer.UpdateCharacter();

        if (timer <= 0 || numberToPick <= 0) { // winning condition has been reached
            Debug.Log("BirdWatchingMinigame finished");
            AudioManager.Instance.Play("Victory");
            GameManager.Instance.OnMinigameFinished(this);
        }
    }



    public override void DisableMinigameObject()
    {
        mainCam.enabled = true;
        bwCam.enabled = false;
        base.DisableMinigameObject();
        sky1.gameObject.SetActive(false);
        sky0.gameObject.SetActive(true);
        birds.gameObject.SetActive(false);
    }
}
