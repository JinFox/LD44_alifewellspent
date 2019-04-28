using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public MyCharacterController thePlayer;
    GameReward currentScore;

    public int initalAge = 20;
    public int finalAge = 80;
    public float timePerYear = 1f;

    public bool menuStage;

    [SerializeField] Transform li;
    [SerializeField]
    List<Minigame> minigames;


    Minigame currentMinigame;
    private bool aMinigameIsActive;

    public bool InAMinigame()
    {
        
        return currentMinigame != null;
    }



    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        

    }
    private void Start()
    {
        thePlayer = FindObjectOfType<MyCharacterController>();
        currentScore = new GameReward(0, 0, initalAge);

        foreach (Minigame g in minigames) {
            g.DisableMinigameObject();
        }
        UpdateScores();
        menuStage = true;
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
            // Debug.Log("Game ended");
            menuStage = true;
            return ;
        }
        UpdateAge();

        // Joaquim : I tried and it did not looked that good
        //li.Rotate(Vector3.left, Time.deltaTime);

        if (this.currentScore.age > finalAge) {
            gameOver = true;
            thePlayer.Die();
        }
        timeToUpdate -= Time.deltaTime;
        if (timeToUpdate <= 0f) {
            timeToUpdate = .2f;
        }
        
       
        if (currentMinigame) {
            currentMinigame.updateMinigame();
        } else {
            thePlayer.UpdateCharacter();
        }
        if (!aMinigameIsActive) {
            EnableNextMinigame();
        }

    }

    private void UpdateAge()
    {
        if (timerUntilNextAge <= 0f) {
            timerUntilNextAge = timePerYear;
            if (!menuStage) this.currentScore.age++;
            UpdateScores();
        }
        
        timerUntilNextAge -= Time.deltaTime;
    }

    private void UpdateScores()
    {
        ScorePanel.Instance.SetAge(this.currentScore.age);
        ScorePanel.Instance.SetProfit(this.currentScore.profit);
    }

    public void OnMinigameFinished(Minigame minigame)
    {
        this.currentScore.age += minigame.TheReward.age;
        this.currentScore.profit += minigame.TheReward.profit;
        this.currentScore.enjoyment += minigame.TheReward.enjoyment;
        UpdateScores();
        currentMinigame.DisableMinigameObject();
        currentMinigame = null;
        aMinigameIsActive = false;
        EnableNextMinigame();
    }

    private void EnableNextMinigame(Minigame toAvoid = null)
    {
        int maxAttempt = 10;
        int nbAttempt = 0;

        int gameIndexToAvoid = minigames.IndexOf(toAvoid);
        int drawnIndex = gameIndexToAvoid;

        while (drawnIndex == gameIndexToAvoid && nbAttempt++ < maxAttempt) {
            drawnIndex = UnityEngine.Random.Range(0, minigames.Count);
        }

        aMinigameIsActive = true;
        minigames[drawnIndex].EnableMinigameObject();
    }

    public void StartGame()
    {
        menuStage = false;
        gameOver = false;

    }
}
