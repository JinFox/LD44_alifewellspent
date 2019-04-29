using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainVCam;

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
    [SerializeField]
    CemetaryScript cemetaryScript;
    [SerializeField]
    GameReward objectiveScore;
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
        InitialiseGameManager();
    }

    private void InitialiseGameManager()
    {
        AudioManager.Instance.PlayMusic("Music");
        
        currentScore = new GameReward(0, 0, initalAge);
        gameOver = false;
        foreach (Minigame g in minigames) {
            g.DisableMinigameObject();
        }
        UpdateScores();
        cemetaryScript.DisableCemetary();
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
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            return;
        }
        if (gameOver) {
            // Debug.Log("Game ended");
            //menuStage = true;
            return ;
        }
        UpdateAge();

        // Joaquim : I tried and it did not looked that good
        //li.Rotate(Vector3.left, Time.deltaTime);

        if (this.currentScore.age > finalAge) {
            gameOver = true;
            if (currentMinigame) {
                currentMinigame.DisableMinigameObject();
            }
            thePlayer.Die();
            AudioManager.Instance.StopMusic();
            Invoke("DisplayCemetary", 2f);
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
    void DisplayCemetary()
    {
        Debug.Log("DisplayCemeraty");
        thePlayer.gameObject.SetActive(false);
        cemetaryScript.EnableCemetary(currentScore, objectiveScore);
    }
    public void ReturnToMenu()
    {
        InitialiseGameManager();
        thePlayer.ResetCharacter();
        SceneManager.LoadScene("SampleScene");
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
        ScorePanel.Instance.SetFun(this.currentScore.enjoyment);
    }

    public void OnMinigameFinished(Minigame minigame)
    {
        this.currentScore.age += minigame.TheReward.age;
        this.currentScore.profit += minigame.TheReward.profit;
        this.currentScore.enjoyment += minigame.TheReward.enjoyment;
        UpdateScores();
        currentMinigame.DisableMinigameObject();
        var save = currentMinigame;
        currentMinigame = null;
        
        aMinigameIsActive = false;
        EnableNextMinigame(save);
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
