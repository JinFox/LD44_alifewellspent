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
    int _profit;
    public int Profit
    {
        get { return _profit; }
        set
        {
            if (value != _profit) {
                _profit = value;
                profitPanel.text = profitPanelPrefix + value;
            }
        }
    }

    int _age;
    public int Age
    {
        get { return _age; }
        set
        {
            if (value != _age) {
                _age = value;
                agePanel.text = agePanelPrefix + value;
            }
        }
     }

}
