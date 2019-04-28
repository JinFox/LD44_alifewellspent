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

    public void Add(GameReward other)
    {
        this.profit += other.profit;
        this.enjoyment += other.enjoyment;
        this.age += other.age;
    }
}

public abstract class Minigame : MonoBehaviour
{
    public Interactor               interactor;
    public GameReward TheReward;

    public virtual void LaunchMinigame()
    {
        //INITIALISE

        GameManager.Instance.RegisterCurrentMinigame(this);
    }

    public abstract void updateMinigame();

    public virtual void EnableMinigameObject()
    {
        gameObject.SetActive(true);
        interactor.Arm();
    }

    public virtual void DisableMinigameObject()
    {
        this.gameObject.SetActive(false);
    }
    
}