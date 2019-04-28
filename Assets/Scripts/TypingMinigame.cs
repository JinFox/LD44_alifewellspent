using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingMinigame : Minigame
{
    string[] potentialWords = {
        "Analysis",
        "Reports",
        "Spreadsheet",
        "Meetings",
        "Delay",
        "Shareholder",
        "Career",
        "Delays",
        "workload",
        "Tables",
        "Presentation",
    };

    [SerializeField]
    GameObject typingUIPanel;

    [SerializeField]
    Text typedWordListUI;
    string typedWords;
    [SerializeField]
    Text wordToTypeUI;
    string currentWordToType;

    [SerializeField]
    InputField input;
    [SerializeField]
    Text timerUI;


    public float minigameDuration = 5f;
    float timer;
    public GameReward goodWordProfitAmount = new GameReward(100, 0, 0);
    public GameReward badWordProfitAmount = new GameReward(-50, 0, 0);

    void UpdateTimerUI()
    {
        timerUI.text = String.Format("{0:0.#}s remaining to type", timer);
    }
    void ReInitInputField()
    {
        input.text = "";
        input.Select();
        input.ActivateInputField();
    }

    void AddBadWordTyped(string typedWord)
    {
        typedWords += "<color=\"red\">Bad Input: " + typedWord + "</color> -$"+ Mathf.Abs(badWordProfitAmount.profit) + "\n";
        TheReward.Add(goodWordProfitAmount);

        typedWordListUI.text = typedWords;
        currentWordToType = null;
        DrawNewWordToType();
        ReInitInputField();
    }
    void AddGoodWordTyped(string typedWord)
    {
        typedWords += "<color=\"green\">" + typedWord + "</color> +$" + Mathf.Abs(goodWordProfitAmount.profit) + "\n";

        typedWordListUI.text = typedWords;
        currentWordToType = null;
        DrawNewWordToType();
        ReInitInputField();
    }

    public override void LaunchMinigame()
    {
        base.LaunchMinigame(); // this stays
        GameReward rew = TheReward;
        rew.profit = 0;


        SetInputPanelActive(true);
        input.onValueChanged.AddListener(onValueChangedOnInput);
        input.onEndEdit.AddListener(onInputSubmit);

        timer = minigameDuration;
        typedWords = "";
        typedWordListUI.text = typedWords;

        MyCharacterController p = GameManager.Instance.thePlayer;
        p.StopWalking();
        p.TypeOnKeyboard(interactor.transform);
        ReInitInputField();

    }

    private void SetInputPanelActive(bool v)
    {
        RectTransform rt = (typingUIPanel.transform as RectTransform);
        if (v) { // make panel appear
            rt.localPosition = new Vector3(0f, rt.localPosition.y);
            typingUIPanel.SetActive(true);
            //rt.localPosition = new Vector3(-1024f, rt.localPosition.y);
        } else { // make panel disappear
            typingUIPanel.SetActive(false);
            //rt.localPosition = new Vector3(-1024f, rt.localPosition.y);
        }
        
        
    }

    void UnblockInput()
    {
        input.readOnly = false;
    }
    void BlockInputTemporally()
    {
        input.readOnly = true;
        Invoke("UnblockInput", 0.5f);
    }

    public override void EnableMinigameObject()
    {
        base.EnableMinigameObject();
    }

    private void onInputSubmit(string arg0)
    {
    //    throw new NotImplementedException();
    }

    private void onValueChangedOnInput(string arg0)
    {
        string entered = arg0.ToLower();
        if (currentWordToType != null && entered != "") {
          //  Debug.Log("value changed " + arg0);
            //currentWordToType.Substring(0, arg0.Length);

            if (entered == currentWordToType.ToLower()) {
                AddGoodWordTyped(currentWordToType);
                AudioManager.Instance.Play("Success");
            }
            else if (currentWordToType.StartsWith(entered, StringComparison.InvariantCultureIgnoreCase)) {
                // continue
                // PLay good sound
                AudioManager.Instance.Play("Typewriter");
            }
            else {
                // bad word
                // play bad sound
                AudioManager.Instance.Play("Error");
                BlockInputTemporally();
                AddBadWordTyped(entered);
            }
        }
    }

    // called every frame
    public override void updateMinigame()
    {
        if (timer <= 0) { // winning condition has been reached
            Debug.Log("Minigame finished");
            AudioManager.Instance.Play("Victory");
            GameManager.Instance.OnMinigameFinished(this);
        }

        if (currentWordToType == null) {
            DrawNewWordToType();
            ReInitInputField();
        }
        UpdateTimerUI();
        timer -= Time.deltaTime;

        // this minigame dont allow walking so we don't update 
        // GameManager.Instance.thePlayer.UpdateCharacter();

  
    }

    private void DrawNewWordToType()
    {
        currentWordToType = potentialWords[UnityEngine.Random.Range(0, potentialWords.Length)];
        wordToTypeUI.text = "\"" + currentWordToType + "\"";
    }

    public override void DisableMinigameObject()
    {

        base.DisableMinigameObject();
        SetInputPanelActive(false);
        input.DeactivateInputField();
        input.onValueChanged.RemoveListener(onValueChangedOnInput);
        input.onEndEdit.RemoveListener(onInputSubmit);
    }
}
