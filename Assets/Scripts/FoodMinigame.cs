using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMinigame : Minigame
{
    public Text timerUI;
    public float startTimer = 30f;
    float timer;
    [SerializeField] Cinemachine.CinemachineVirtualCamera mainCam;
    // [SerializeField] Camera bwCam;
    [SerializeField] Cinemachine.CinemachineVirtualCamera bwCam;
    [SerializeField] Transform birds;
    float cooldown = 2f; // initial delay
    int numberToPick;
    public GameReward eatSomethingReward;

    public override void LaunchMinigame()
    { 
        base.LaunchMinigame(); // this stays
        // INITIALISATION
        GameReward rew = TheReward;
        rew.profit = 0;


        timer = startTimer;
        MyCharacterController p = GameManager.Instance.thePlayer;
        p.StopWalking();
      
        mainCam.enabled = false;
        bwCam.enabled = true; 
        birds.gameObject.SetActive(true);
        numberToPick = 0;
        foreach (Transform child in birds.transform)
        {
            child.gameObject.SetActive(true);
            numberToPick++;
        }
        SetInputPanelActive(true);

    }

    void UpdateTimerUI()
    {
        timerUI.text = String.Format("{0:0.#}s remaining for meal", timer);
    }

    // called every frame
    public override void updateMinigame()
    {
        timer -= Time.deltaTime;
        // Debug.DrawRay(Camera.main.transform.position, Input.mousePosition, Color.green, 1);
        cooldown -= Time.deltaTime;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (cooldown < 0 && (Physics.Raycast(ray, out hit, 10.0f)))
        {
            //    Debug.Log("hit " + hit.transform.name);
                // hit.transform.position += Vector3.right * speed * time.deltaTime; // << declare public speed and set it in inspector
           if (hit.transform.CompareTag("food"))
            {
                AudioManager.Instance.Play("Success");
                GameManager.Instance.thePlayer.PickupObject();
                TheReward.enjoyment += eatSomethingReward.enjoyment;
                TheReward.profit += eatSomethingReward.profit;
                TheReward.age += eatSomethingReward.age;
                numberToPick--;
                hit.transform.gameObject.SetActive(false);
                
                cooldown = UnityEngine.Random.Range(0.5f, 1.0f);
            }
                
        }

        // this minigame dont allow walking so we don't update 
        // GameManager.Instance.thePlayer.UpdateCharacter();
        UpdateTimerUI();
       
        if (timer <= 0 || numberToPick <= 0) { // winning condition has been reached
            Debug.Log("FoodMinigame finished");
            AudioManager.Instance.Play("Victory");
            GameManager.Instance.OnMinigameFinished(this);
        }
    }

    private void SetInputPanelActive(bool v)
    {
        if (v) { // make panel appear
            timerUI.gameObject.SetActive(true);
        }
        else { // make panel disappear
            timerUI.gameObject.SetActive(false);
        }
    }

    public override void DisableMinigameObject()
    {
        mainCam.enabled = true;
        bwCam.enabled = false;
        SetInputPanelActive(false);
        base.DisableMinigameObject();

        birds.gameObject.SetActive(false);
    }
}
