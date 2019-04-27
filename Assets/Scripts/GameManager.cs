using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager Instance { get; private set; }

    private MyCharacterController _thePlayer;
    GameReward currentScore;

    public int initalAge = 20;
    public int finalAge = 80;
    public float timePerYear = 1f;


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
        _thePlayer = FindObjectOfType<MyCharacterController>();
        currentScore = new GameReward(0, 0, initalAge);

        foreach (Minigame g in minigames) {
            g.DisableMinigameObject();
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

        if (this.currentScore.age > finalAge) {
            gameOver = true;
            _thePlayer.Die();
        }
        timeToUpdate -= Time.deltaTime;
        if (timeToUpdate <= 0f) {
            timeToUpdate = .2f;
        }
        
       
        if (currentMinigame) {
            currentMinigame.updateMinigame();
        } else {
            _thePlayer.UpdateCharacter();
        }
        if (!aMinigameIsActive) {
            EnableNextMinigame();
        }
    }

    private void UpdateAge()
    {
        if (timerUntilNextAge <= 0f) {
            timerUntilNextAge = timePerYear;
            this.currentScore.age++;
            // here is the deeply metaphorical sunset:
            li.Rotate(Vector3.left, 0.53f);
            UpdateScores();
        }
        
        timerUntilNextAge -= Time.deltaTime;
    }

    private void UpdateScores()
    {
        ScorePanel.Instance.SetAge(this.currentScore.age);
        ScorePanel.Instance.SetProfit(this.currentScore.profit);
    }

    private void OnMinigameFinished(GameReward obj)
    {
        this.currentScore.age += obj.age;
        this.currentScore.profit += obj.profit;
        this.currentScore.enjoyment += obj.enjoyment;
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


}
