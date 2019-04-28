using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotocopyMinigame : Minigame
{

    public float startTimer = 5f;
    float timer;
   

    public override void LaunchMinigame()
    {
        base.LaunchMinigame(); // this stays
        // INITIALISATION
        timer = startTimer;
        MyCharacterController p = GameManager.Instance.thePlayer;
        p.StopWalking();
        p.PickupObject();
        AudioManager.Instance.Play("Photocopy");
    }

    // called every frame
    public override void updateMinigame()
    {
        timer -= Time.deltaTime;

        // this minigame dont allow walking so we don't update 
        // GameManager.Instance.thePlayer.UpdateCharacter();

        if (timer <= 0) { // winning condition has been reached
            Debug.Log("photocopy finished");
            AudioManager.Instance.Play("Victory");
            GameManager.Instance.OnMinigameFinished(this);
        }
    }

    public override void DisableMinigameObject()
    {
        base.DisableMinigameObject();
    }
}
