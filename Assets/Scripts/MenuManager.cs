using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] Transform menu;
    [SerializeField] Text text;
    [SerializeField] Transform panel;

    private float hiddenPosX;
    private float visiblePosX;
    private bool isActive;


    RectTransform menuTransform;

    float GetHiddenPosX()
    {
        return 100f;
    }
    float GetVisiblePosX()
    {
       
        return -200f;
    }
    // Start is called before the first frame update
    void Start()
    {
        menuTransform = menu.transform as RectTransform;
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
        menu.gameObject.SetActive(true);
        menuTransform.DOAnchorPosX(GetVisiblePosX(), 0.2f);
        isActive = true;

    }
    void Disappear()
    {
        isActive = false;
        // DOTween.Kill(menu.transform);

        menuTransform.DOAnchorPosX(GetHiddenPosX(), 0.2f).OnComplete(DisableMenuElements);
        Debug.Log("Disappearing Menu");
    }

    private void DisableMenuElements()
    {
        menu.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        panel.gameObject.SetActive(false);
        gm.StartGame();
    }
    public void Understand()
    {
        if (!panel.gameObject.activeSelf)
        {
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

            panel.gameObject.SetActive(false);
        }
    }
}
