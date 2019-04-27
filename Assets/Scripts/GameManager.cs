using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float timeToUpdate = 1f;
    // Update is called once per frame
    void Update()
    {
        timeToUpdate -= Time.deltaTime;
        if (timeToUpdate <= 0f) {
            timeToUpdate = 1f;
            ScorePanel.Instance.Age = UnityEngine.Random.Range(1, 80);
            ScorePanel.Instance.Profit = UnityEngine.Random.Range(1, 80000000);
        }
        
    }
}
