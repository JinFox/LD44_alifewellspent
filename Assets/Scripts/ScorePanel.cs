using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] public string profitPanelPrefix;
    [SerializeField] public Text profitPanel;
    [SerializeField] public string agePanelPrefix;
    [SerializeField] public Text agePanel;
    [HideInInspector]
    public static ScorePanel Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
    }
    public void SetProfit(int profit)
    {
        profitPanel.text = profitPanelPrefix + profit;
    }

   
    public void SetAge(int age)
    {
        agePanel.text = agePanelPrefix + age;
            
     }

}
