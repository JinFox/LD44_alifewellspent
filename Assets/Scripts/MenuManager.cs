using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] Transform menu;
    [SerializeField] Text text;
    [SerializeField] Transform panel;

    private float hiddenPosX;
    private float visiblePosX;
    private bool isActive;


    // Start is called before the first frame update
    void Start()
    {
        hiddenPosX = Screen.width + 100f; // changed from 1024f due to my screen not compatible
        visiblePosX = hiddenPosX - 300f;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.menuStage && !isActive) Appear();
        if (!gm.menuStage && isActive) Disappear();

    }

    void Appear()
    {
        menu.transform.DOMoveX(visiblePosX, 0.4f);
        isActive = true;

    }
    void Disappear()
    {
        isActive = false;
        // DOTween.Kill(menu.transform);
        menu.transform.DOMoveX(hiddenPosX, 0.4f);
        Debug.Log("Disappearing Menu");
    }

    public void Quit()
    {
        if (!GameManager.Instance.menuStage)
            return;
        Application.Quit();
    }
    public void StartGame()
    {
        if (!GameManager.Instance.menuStage)
            return;
        gm.StartGame();
    }
    public void Understand()
    {
        if (!GameManager.Instance.menuStage)
            return;
        if (!panel.gameObject.activeSelf)
        {
            Debug.Log("Summoning the 'about' panel");
            text.text = @"A Life Well Spent

In order to live a fulfilling life, you must obtain
the most dollar signs possibe, everyone knows
that!Do not distract yourself with trivialities
that are there for hippies and like.

Written by Joaquim Renard
...and Michael laskowski

for Ludum Dare 44";

            panel.gameObject.SetActive(true);

        } else
        {
            text.text = "";
            panel.gameObject.SetActive(false);
        }
    }
}
