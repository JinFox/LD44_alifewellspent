using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMinigame : Minigame
{
    public GameReward theReward;

    public float startTimer = 5f;
    float timer;

    public override void LaunchMinigame(Action<GameReward> onMinigameFinished)
    {
        base.LaunchMinigame(onMinigameFinished); // this stays
        // INITIALISATION
        timer = startTimer;
        
    }

    // called every frame
    public override void updateMinigame()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) { // winning condition has been reached
            Debug.Log("Minigame finished");
            this.onMinigameFinished(theReward);
        }
    }

    public override void DisableMinigameObject()
    {
        base.DisableMinigameObject();
    }
}
