﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdWatchingMinigame : Minigame
{

    public float startTimer = 30f;
    float timer;
    [SerializeField] Camera mainCam;
    [SerializeField] Camera bwCam;
    [SerializeField] Transform sky0;
    [SerializeField] Transform sky1;


    public override void LaunchMinigame()
    {
        base.LaunchMinigame(); // this stays
        // INITIALISATION
        timer = startTimer;
        MyCharacterController p = GameManager.Instance.thePlayer;
        p.StopWalking();
        p.PickupObject();
        mainCam.enabled = false;
        bwCam.enabled = true;
        sky0.gameObject.SetActive(false);
        sky1.gameObject.SetActive(true);

    }

    // called every frame
    public override void updateMinigame()
    {
        timer -= Time.deltaTime;

        // this minigame dont allow walking so we don't update 
        // GameManager.Instance.thePlayer.UpdateCharacter();

        if (timer <= 0) { // winning condition has been reached
            Debug.Log("BirdWatchingMinigame finished");
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
    }
}
