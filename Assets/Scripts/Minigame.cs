using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameReward
{
    public int profit;
    public int enjoyment;
    public int age;

    public GameReward(int profit, int enjoyment, int age)
    {
        this.profit = profit;
        this.enjoyment = enjoyment;
        this.age = age;
    }
}

public abstract class Minigame : MonoBehaviour
{
    protected Action<GameReward> onMinigameFinished;

    
    public virtual void LaunchMinigame(Action<GameReward> onMinigameFinished)
    {
        //INITIALISE

        this.onMinigameFinished = onMinigameFinished;
        GameManager.Instance.RegisterCurrentMinigame(this);
    }

    public abstract void updateMinigame();

    public virtual void EnableMinigameObject()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableMinigameObject()
    {
        this.onMinigameFinished = null;
    }
    
}