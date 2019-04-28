using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMinigame : Minigame
{

    public float startTimer = 30f;
    float timer;
    [SerializeField] Cinemachine.CinemachineVirtualCamera mainCam;
    // [SerializeField] Camera bwCam;
    [SerializeField] Cinemachine.CinemachineVirtualCamera bwCam;
    [SerializeField] Transform birds;
    float cooldown = 2f; // initial delay

    public override void LaunchMinigame()
    { 
        base.LaunchMinigame(); // this stays
        // INITIALISATION
        GameReward rew = TheReward;
        rew.profit = 0;


        timer = startTimer;
        MyCharacterController p = GameManager.Instance.thePlayer;
        p.StopWalking();
        p.PickupObject();
        mainCam.enabled = false;
        bwCam.enabled = true; 
        birds.gameObject.SetActive(true);
        foreach (Transform child in birds.transform)
        {
            child.gameObject.SetActive(true);
        }

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
                hit.transform.gameObject.SetActive(false);
                cooldown = UnityEngine.Random.Range(0.5f, 1.0f);
            }
                
        }
        
        // this minigame dont allow walking so we don't update 
        // GameManager.Instance.thePlayer.UpdateCharacter();

        if (timer <= 0) { // winning condition has been reached
            Debug.Log("FoodMinigame finished");
            GameManager.Instance.OnMinigameFinished(this);
        }
    }



    public override void DisableMinigameObject()
    {
        mainCam.enabled = true;
        bwCam.enabled = false;
        base.DisableMinigameObject();

        birds.gameObject.SetActive(false);
    }
}
