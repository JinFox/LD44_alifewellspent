using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager Instance { get; private set; }

    GameReward currentScore;

    public int initalAge = 20;
    public int finalAge = 80;
    public float timePerYear = 1f;

    public bool buttonMashed = false;

    [SerializeField]
    Minigame[] minigames;

    Minigame currentMinigame;

    public bool InAMinigame()
    {
        return currentMinigame != null;
    }


    // Start is called before the first frame update
    void Awake()
    {
        currentScore = new GameReward(0, 0, initalAge);
        if (Instance == null) {
            Instance = this;
        }
    }
    
    public void RegisterCurrentMinigame(Minigame game)
    {
        currentMinigame = game;
    }

    float timerUntilNextAge;

    float timeToUpdate = .1f;
    private bool gameOver = false;

    void Update()
    {
        if (gameOver) {
            Debug.Log("Game ended");
            return ;
        }

        UpdateAge();
        timeToUpdate -= Time.deltaTime;
        if (timeToUpdate <= 0f) {
            timeToUpdate = .2f;
        }
        
        if (Input.GetKeyDown(KeyCode.K)) { // For tests
            minigames[0].LaunchMinigame(this.onMinigameFinished);
        }

        if (currentMinigame) {
            currentMinigame.updateMinigame();
        }
    }

    private void UpdateAge()
    {
        if (timerUntilNextAge <= 0f) {
            timerUntilNextAge = timePerYear;
            this.currentScore.age++;
            UpdateScores();
        }
        timerUntilNextAge -= Time.deltaTime;
    }

    private void UpdateScores()
    {
        ScorePanel.Instance.SetAge(this.currentScore.age);
        ScorePanel.Instance.SetProfit(this.currentScore.profit);
    }

    private void onMinigameFinished(GameReward obj)
    {
        this.currentScore.age += obj.age;
        this.currentScore.profit += obj.profit;
        this.currentScore.enjoyment += obj.enjoyment;
        UpdateScores();
        currentMinigame.DisableMinigameObject();
        currentMinigame = null;
        EnableNextMinigame();
    }

    private void EnableNextMinigame()
    {
        
    }
}
