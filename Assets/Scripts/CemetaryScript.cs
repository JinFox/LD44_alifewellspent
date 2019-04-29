using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CemetaryScript : MonoBehaviour
{
    public CinemachineVirtualCamera cemetaryCam;

    public Transform flowerRoot;
    List<Transform> flowers;

    [SerializeField] public string profitPanelPrefix;
    [SerializeField] public Text profitPanel;
    [SerializeField] public string funPanelPrefix;
    [SerializeField] public Text funPanel;
    [SerializeField] GameObject deadCanvas;


    public GameObject[] tombstones;

    private void Start()
    {
        cemetaryCam.enabled = false;
        flowers = new List<Transform>();
        foreach (Transform T in flowerRoot) {
            flowers.Add(T);
            T.gameObject.SetActive(false);
        }
        
    }
    
    void UpdatePanels (GameReward scores)
    {
       profitPanel.text = profitPanelPrefix + scores.profit;
       funPanel.text = funPanelPrefix + scores.enjoyment;  
    }

    private void DisplayCorrectFlowers(float scoreEnjoy)
    {

        foreach (var flower in flowers) {
            if (UnityEngine.Random.Range(0f, 1f) <= scoreEnjoy) {
                flower.gameObject.SetActive(true);
            }
            else {
                flower.gameObject.SetActive(false);
            }
        }
    }

    private void DisplayCorrectTombstone(float profitScore)
    {
        if (tombstones == null || tombstones.Length < 1)
            return;
        foreach (GameObject tomb in tombstones) {
            tomb.SetActive(false);
        }
        int indexToEnable = Mathf.Min(
                Mathf.FloorToInt(Mathf.Clamp01(profitScore) * tombstones.Length),
                tombstones.Length - 1
            );

        tombstones[indexToEnable].SetActive(true);
    }

    public void EnableCemetary(GameReward scores, GameReward objectives)
    {
        gameObject.SetActive(true);

        flowerRoot.gameObject.SetActive(true);
        cemetaryCam.enabled = true;
        deadCanvas.SetActive(true);
        GameManager.Instance.mainVCam.enabled = false;

        UpdatePanels(scores);
        //will set the value between 0 and 1 depending if the objective is reached
         DisplayCorrectTombstone((float)Mathf.Clamp(scores.profit, 0, objectives.profit) / objectives.profit);
        DisplayCorrectFlowers((float)Mathf.Clamp(scores.enjoyment, 0, objectives.enjoyment) / objectives.enjoyment);
       
    }
    public void DisableCemetary()
    {
        flowerRoot.gameObject.SetActive(false);
        foreach (GameObject tomb in tombstones) {
            tomb.SetActive(false);
        }
        deadCanvas.SetActive(false);

    }

    public void RestartGame()
    {
        GameManager.Instance.ReturnToMenu();
    }
    
}
